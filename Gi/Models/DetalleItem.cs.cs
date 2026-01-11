using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Gi.Models;

public class DetalleItem : INotifyPropertyChanged
{
    //metodos y propiedades de la grilla de deatlles
    
    string? _nombre;
    public string? Nombre
    {
        get => _nombre;
        set { _nombre = value; OnPropertyChanged(); }
    }

    float _cantidad;
    public float Cantidad
    {
        get => _cantidad;
        set
        {
            _cantidad = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Subtotal));
        }
    }

    float _precioUnitario;
    public float PrecioUnitario
    {
        get => _precioUnitario;
        set
        {
            _precioUnitario = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Subtotal));
        }
    }

    public float Subtotal => Cantidad * PrecioUnitario;

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
