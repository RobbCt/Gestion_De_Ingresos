using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Text;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Gi.Models;

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
                if (float.TryParse(_monto, out float valor))
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

            if (!_usarDetalles)
                LimpiarDetalles();
            else
                RecalcularMonto();
        }
    }



    public bool MontoEditable => !UsarDetalles;

    //datos grilla
    public ObservableCollection<DetalleItem> Detalles { get; set; }public





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

    //ACCION BUTTON GUARDAR

    public ICommand GuardarIngresoCommand { get; }

    //ACCION PAGINA RESETEAR
    public ICommand ResetearIngresoCommand { get; }

    //ACCION INGERESO DETALLES
    public ICommand ActivarDetallesCommand { get; }

    //CONSTRUCTOR DEL VIEWMODEL

    public IngresoViewModel()
    {
        Logica.ResetGlobalSolicitado += ResetGlobal;

        Detalles = new ObservableCollection<DetalleItem>();
        Detalles.CollectionChanged += (_, __) => RecalcularMonto();

        AgregarFila();

        GuardarIngresoCommand = new Command(setPropIngreso);
        ResetearIngresoCommand = new Command(ResetearIngreso);

        ActivarDetallesCommand = new Command(() =>
        {
            UsarDetalles = !UsarDetalles;
        });
    }

    //METODOS

    void LimpiarDetalles()
    {
        Detalles.Clear();
        AgregarFila();
    }

    void AgregarFila()
    {
        var item = new DetalleItem();
        item.PropertyChanged += (_, __) => RecalcularMonto();
        Detalles.Add(item);
    }

    void RecalcularMonto()
    {
        if (!UsarDetalles) return;

        float total = 0;
        foreach (var d in Detalles)
            total += d.Cantidad * d.PrecioUnitario;

        Monto = total.ToString("0.##");
        Logica.MontoIngreso = total;
    }

    void setPropIngreso()
    {
        Logica.Detalles = UsarDetalles ? Detalles.ToList() : new List<DetalleItem>();



        var origen = Origen ?? string.Empty;
        var montoStr = Monto ?? string.Empty;

        if (Logica.ValidarIngreso(origen, montoStr))
        {
            Informe = "Guardado Exitoso";
            ColorInforme = Colors.Green;
        }
        else
        {
            Informe = "Datos Incompletos";
            ColorInforme = Colors.Red;
        }
    }

    private void ResetGlobal()
    {
        //reaccion de un evento en la app

        Origen = null;
        Monto = null;
        Informe = null;
        ColorInforme = Colors.Transparent;
        Detalles.Clear();
        Detalles.Add(new DetalleItem());
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

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
}
