using Gi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;

namespace Gi.ViewModels;

internal class EgresoViewModel : INotifyPropertyChanged
{
    //PROPIEDADES EGRESO (OnProperty para q la Ui se resetee con un boton)

    //campo
    string? _destino;
    //calor en la Ui
    public string? Destino
    {
        get => _destino;
        set { _destino = value; OnPropertyChanged(); }
    }

    //campo
    string? _descripcion;
    //valor en la UI
    public string? Descripcion
    {
        get => _descripcion;
        set { _descripcion = value; OnPropertyChanged(); }
    }

    //campo
    string? _monto;
    //valor en la Ui
    public string? Monto
    {
        get => _monto;
        set { _monto = value; OnPropertyChanged(); }
    }

    //esatdo de grilla
    bool _usarDetalles;
    //(activa/desactiva)
    public bool UsarDetalles
    {
        get => _usarDetalles;
        set
        {
            if (_usarDetalles == value)
                return;

            _usarDetalles = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(MontoEditable));

            //limpiar informe
            Informe = null;
            ColorInforme = Colors.Transparent;

            //reset lógico del monto
            if (_usarDetalles)
            {
                // activar grilla y mostrar 0 en Monto
                if (string.IsNullOrWhiteSpace(Monto))
                    Monto = "0";

                RecalcularMonto();
            }
            else
            {
                // desactivar grilla y limpiar detalles
                LimpiarDetalles();
            }
        }
    }

    //bloqueo monto si detalles activos
    public bool MontoEditable => !UsarDetalles;

    //datos grilla
    public ObservableCollection<DetalleItem> Detalles { get; set; }

    
    
    

    //INFORME DE VALIDACION

    string? _informe;
    public string? Informe
    {
        get => _informe;
        set { _informe = value; OnPropertyChanged(); }
    }

    //q no se vea al principio pe hasno
    Color _colorInforme = Colors.Transparent;
    public Color ColorInforme
    {
        get => _colorInforme;
        set { _colorInforme = value; OnPropertyChanged(); }
    }


    //ACCIONES

    //button guardar
    public ICommand GuardarEgresoCommand { get; }
    //pagina resetear
    public ICommand ResetearEgresoCommand { get; }
    //ingreso detalles
    public ICommand ActivarDetallesCommand { get; }
    //el nombre ya lo dice
    public ICommand AgregarFilaCommand { get; }
    //es obvio
    public ICommand QuitarFilaCommand { get; }




    //CONTRUCTOR DEL VIEWMODEL

    public EgresoViewModel()
    {
        Logica.ResetGlobalSolicitado += ResetGlobal;

        Detalles = new ObservableCollection<DetalleItem>();
        Detalles.CollectionChanged += (_, __) =>
        {
            if (UsarDetalles)
                RecalcularMonto();
        };

        AgregarFila();

        GuardarEgresoCommand = new Command(setPropEgreso);
        ResetearEgresoCommand = new Command(ResetearEgreso);

        ActivarDetallesCommand = new Command(() => UsarDetalles = !UsarDetalles);

        AgregarFilaCommand = new Command(AgregarFila);
        QuitarFilaCommand = new Command(QuitarFila);
    }

    // METODOS

    bool TryGetMonto(out decimal monto)
    {
        monto = 0m;

        if (string.IsNullOrWhiteSpace(Monto))
            return false;

        return decimal.TryParse(
            Monto,
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out monto
        ) && monto > 0m;

    }
    void RecalcularMonto()
    {
        if (!UsarDetalles)
            return;

        decimal total = 0m;
        foreach (var d in Detalles)
            total += d.CantidadNumerica * d.PrecioUnitarioNumerico;

        Monto = total.ToString("0.##", CultureInfo.InvariantCulture);
    }
    void AgregarFila()
    {
        var item = new DetalleItem();
        item.PropertyChanged += (_, __) =>
        {
            if (UsarDetalles)
                RecalcularMonto();
        };

        Detalles.Add(item);
    }
    void QuitarFila()
    {
        if (Detalles.Count <= 1)
            return;

        Detalles.RemoveAt(Detalles.Count - 1);

        if (UsarDetalles)
            RecalcularMonto();
    }
    void LimpiarDetalles()
    {
        Detalles.Clear();
        AgregarFila();

        if (UsarDetalles)
            Monto = "0";
        else
            Monto = null;
    }
    void setPropEgreso()
    {
        bool detallesValidos = true;

        // validar grilla si está activa
        if (UsarDetalles)
        {
            foreach (var d in Detalles)
            {
                if (string.IsNullOrWhiteSpace(d.Nombre)
                    || d.CantidadNumerica <= 0
                    || d.PrecioUnitarioNumerico <= 0)
                {
                    detallesValidos = false;
                    break;
                }
            }
        }

        // validar monto (viene del entry o de la grilla)
        if (!TryGetMonto(out decimal monto))
            detallesValidos = false;
        

        bool egresoValido = detallesValidos && Logica.ValidarEgreso(Destino ?? string.Empty, Descripcion ?? string.Empty, monto);

        if (egresoValido)
        {
            //copiar detalles si el ingreso es valido
            Logica.DetallesEgreso = UsarDetalles ? Detalles.Select(d => d.Clone()).ToList() : new();

            Informe = "Guardado Exitoso";
            ColorInforme = Colors.Green;
        }
        else
        {
            Informe = "Datos Incompletos";
            ColorInforme = Colors.Red;
        }

        Logica.Egreso = egresoValido;
    }
    public void ResetearEgreso()
    {
        //accion del usuario

        Destino = null;
        Descripcion = null;
        Informe = null;
        ColorInforme = Colors.Transparent;

        UsarDetalles = false; // esto llamará automáticamente a LimpiarDetalles() y pondrá Monto = null

        Logica.ResetearEgreso();
    }
    void ResetGlobal()
    {
        //reaccion a un evento en la app

        Destino = null;
        Descripcion = null;
        Monto = null;
        Informe = null;
        ColorInforme = Colors.Transparent;

        UsarDetalles = false;

        //Detalles.Clear();
        //AgregarFila();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

}
