using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Text;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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

    //ACCION BUTTON GUARDAR
    public ICommand GuardarEgresoCommand { get; }

    //ACCION PAGINA RESETEAR
    public ICommand ResetearEgresoCommand { get; }

    //CONTRUCTOR DEL VIEWMODEL

    public EgresoViewModel()
    {
        Logica.ResetGlobalSolicitado += ResetGlobal;

        GuardarEgresoCommand = new Command(setPropEgreso);
        ResetearEgresoCommand = new Command(ResetearEgreso);
    }

    // METODOS
    void setPropEgreso()
    {
        var destino = Destino ?? string.Empty;
        var descripcion = Descripcion ?? string.Empty;
        var monto = Monto ?? string.Empty;

        if (Logica.ValidarEgreso(destino, descripcion, monto))
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

        Destino = null;
        Descripcion = null;
        Monto = null;
        Informe = null;
        ColorInforme = Colors.Transparent;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public void ResetearEgreso()
    {
        //accion del usuario 

        Destino = null;
        Descripcion = null;
        Monto = null;
        Informe = null;
        ColorInforme = Colors.Transparent;
        Logica.ResetearEgreso();
    }
}
