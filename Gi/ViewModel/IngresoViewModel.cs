using Gi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
//using System.Text;
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
            // Actualizamos Logica solo si no usamos detalles
            if (!UsarDetalles)
            {
                if (float.TryParse(_monto, NumberStyles.Float, CultureInfo.InvariantCulture, out float valor))
                    Logica.MontoIngreso = valor;
                else
                    Logica.MontoIngreso = 0;
            }
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

            // reset lógico y visual del monto
            Monto = null;
            Logica.MontoIngreso = 0;

            if (_usarDetalles)
            {
                RecalcularMonto(); // arrancará en 0 si no hay datos
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

    void RecalcularMonto()
    {
        if (!UsarDetalles) return;

        float total = 0;
        foreach (var d in Detalles)
            total += d.CantidadNumerica * d.PrecioUnitarioNumerico;

        Monto = total.ToString("0.##");
        Logica.MontoIngreso = total;
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
            foreach (var detalle in Detalles)
            {
                if (string.IsNullOrWhiteSpace(detalle.Nombre)
                    || detalle.CantidadNumerica <= 0
                    || detalle.PrecioUnitarioNumerico <= 0)
                {
                    detallesValidos = false;
                    break;//xd
                }
            }
        }

        //siempre actualizamos la lista de detalles en Logica
        Logica.Detalles = UsarDetalles ? Detalles.ToList() : new List<DetalleItem>();

        var origen = Origen ?? string.Empty;
        var montoStr = Monto ?? string.Empty;
        float montoNumerico = 0;
        float.TryParse(montoStr, System.Globalization.NumberStyles.Float,
                       System.Globalization.CultureInfo.InvariantCulture,
                       out montoNumerico);

        bool ingresoValido = Logica.ValidarIngreso(origen, montoNumerico.ToString(CultureInfo.InvariantCulture));

        //sincronizamos la validez de ingreso con los detalles
        if (UsarDetalles && !detallesValidos)
            ingresoValido = false;  //invalida el ingreso si los detalles no están completos

        
        if (ingresoValido)
        {
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
