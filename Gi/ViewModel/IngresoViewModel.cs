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

internal class IngresoViewModel : INotifyPropertyChanged
{
    
    //PROPIEDADES INGRESO (OnProperty para q la Ui se resetee con un boton)

    //campo
    string? _origen;
    //valor en la Ui
    public string? Origen
    {
        get => _origen;
        set { _origen = value; OnPropertyChanged(); }
    }

    //campo
    string? _monto;
    //valor en la UI
    public string? Monto
    {
        get => _monto;
        set
        {
            _monto = value;
            OnPropertyChanged();
        }
    }

    //estado de grilla 
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

            //limpiar informe de guardado label
            Informe = null;
            ColorInforme = Colors.Transparent;

            //reset lógico y visual del monto
            Monto = null;
            Logica.MontoIngreso = 0m;

            if (_usarDetalles)
            {
                RecalcularMonto(); //arrancará en 0 si no hay datos
            }
            else
            {
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

    //q no se vea al principio pe mi causa
    Color _colorInforme = Colors.Transparent;
    public Color ColorInforme
    {
        get => _colorInforme;
        set { _colorInforme = value; OnPropertyChanged(); }
    }

    
    //ACCIONES

    //button guardar
    public ICommand GuardarIngresoCommand { get; }
    //pagina resetear
    public ICommand ResetearIngresoCommand { get; }
    //ingreso detalles
    public ICommand ActivarDetallesCommand { get; }
    //el nombre ya lo dice
    public ICommand AgregarFilaCommand { get; }
    //es obvio
    public ICommand QuitarFilaCommand { get; }




    //CONSTRUCTOR DEL VIEWMODEL

    public IngresoViewModel()
    {
        Logica.ResetGlobalSolicitado += ResetGlobal;

        Detalles = new ObservableCollection<DetalleItem>();
        Detalles.CollectionChanged += (_, __) =>
        {
            if (UsarDetalles)
                RecalcularMonto();
        };

        AgregarFila();

        GuardarIngresoCommand = new Command(setPropIngreso);
        ResetearIngresoCommand = new Command(ResetearIngreso);

        ActivarDetallesCommand = new Command(() => UsarDetalles = !UsarDetalles);

        AgregarFilaCommand = new Command(AgregarFila);
        QuitarFilaCommand = new Command(QuitarFila);
    }

    //METODOS

    bool TryGetMonto(out decimal monto)
    {
        monto = 0m;
    
        if (string.IsNullOrWhiteSpace(Monto))
            return false;

        return decimal.TryParse(
        Monto,
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
            //Cuando cualquier propiedad cambie y los detalles estén activos
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
    }
    void setPropIngreso()
    {
        bool detallesValidos = true;

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

        if (!TryGetMonto(out decimal monto))
            detallesValidos = false;

        bool ingresoValido = detallesValidos && Logica.ValidarIngreso(Origen ?? string.Empty, monto);


        if (ingresoValido)
        {
            //solo copiar detalles si es valido
            Logica.DetallesIngreso = UsarDetalles ? Detalles.Select(d => d.Clone()).ToList() : new();

            Informe = "Guardado Exitoso";
            ColorInforme = Colors.Green;
        }
        else
        {
            Informe = "Datos Incompletos";
            ColorInforme = Colors.Red;
        }

        Logica.Ingreso = ingresoValido;
    }
    public void ResetearIngreso()
    {
        //accion del usuario

        Origen = null;
        Monto = null;
        Informe = null;
        ColorInforme = Colors.Transparent;

        Logica.ResetearIngreso();

        Detalles.Clear();
        AgregarFila();
    }
    private void ResetGlobal()
    {
        //reaccion de un evento en la app

        Origen = null;
        Monto = null;
        Informe = null;
        ColorInforme = Colors.Transparent;

        UsarDetalles = false;

        Detalles.Clear();
        AgregarFila();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

}
