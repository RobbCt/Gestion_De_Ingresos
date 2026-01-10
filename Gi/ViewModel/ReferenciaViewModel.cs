using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Text;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Gi.ViewModels;

internal class ReferenciaViewModel : INotifyPropertyChanged
{
    //PROPIEDADES REFERENCIA (OnProperty para q la Ui se resetee con un boton)

    //campo
    string? _fecha;
    //valor en la UI
    public string? Fecha
    {
        get => _fecha;
        set { _fecha = value; OnPropertyChanged();}
    }

    //campo
    string? _tipoDePago;
    //valor en la UI
    public string? TipoDePago
    {
        get => _tipoDePago;
        set { _tipoDePago = value; OnPropertyChanged();}
    }

    //campo
    string? _motivo;
    //valor en la UI
    public string? Motivo
    {
        get => _motivo;
        set { _motivo = value; OnPropertyChanged();}
    }

    //PICKER DE OPCIONES DE PAGO

    public List<string> TiposDePagos { get; }

    //INFORME DE VALIDACION

    string? _informe;
    public string? Informe
    {
        get => _informe;
        set { _informe = value; OnPropertyChanged(); }
    }

    //q no se vea al principio pe mi cuy
    Color _colorInforme = Colors.Transparent;
    public Color ColorInforme
    {
        get => _colorInforme;
        set { _colorInforme = value; OnPropertyChanged(); }
    }

    //ACCION BUTTON GUARDAR

    public ICommand GuardarReferenciaCommand { get; }

    //ACCION PAGINA RESETEAR
    public ICommand ResetearReferenciaCommand { get; }

    //CONTRUCTOR DEL VIEWMODEL

    public ReferenciaViewModel()
    {
        Logica.ResetGlobalSolicitado += ResetGlobal;

        TiposDePagos = new()
        {
            "Efectivo",
            "Transferencia",
            "Tarjeta",
            "Otro"
        };
        GuardarReferenciaCommand = new Command(setPropReferencia);

        ResetearReferenciaCommand = new Command(ResetearReferencia);

    }

    //METODOS

    void setPropReferencia()
    {
        var fecha = Fecha ?? string.Empty;
        var tipo = TipoDePago ?? string.Empty;
        var motivo = Motivo ?? string.Empty;

        if (Logica.ValidarReferencia(fecha, tipo, motivo))
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

        Fecha = null;
        TipoDePago = null;
        Motivo = null;
        Informe = null;
        ColorInforme = Colors.Transparent;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    public void ResetearReferencia()
    {
        //accion del usuario    

        Fecha = null;
        TipoDePago = null;
        Motivo = null;
        Informe = null;
        ColorInforme = Colors.Transparent;
        Logica.ResetearReferencia();
    }
}
