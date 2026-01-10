using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Text;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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
        set { _monto = value; OnPropertyChanged(); }
    }

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

    //CONSTRUCTOR DEL VIEWMODEL

    public IngresoViewModel()
    {
        Logica.ResetGlobalSolicitado += ResetGlobal;

        GuardarIngresoCommand = new Command(setPropIngreso);
        ResetearIngresoCommand = new Command(ResetearIngreso);
    }

    //METODOS

    void setPropIngreso()
    {
        var origen = Origen ?? string.Empty;
        var monto = Monto ?? string.Empty;

        if (Logica.ValidarIngreso(origen, monto))
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
    }
}
