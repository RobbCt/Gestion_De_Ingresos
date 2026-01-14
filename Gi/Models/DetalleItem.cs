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

    string? _cantidad;
    public string? Cantidad
    {
        get => _cantidad;
        set
        {
            if (_cantidad != value)
            {
                _cantidad = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CantidadNumerica));
                OnPropertyChanged(nameof(Subtotal));
            }
        }
    }

    string? _precioUnitario;
    public string? PrecioUnitario
    {
        get => _precioUnitario;
        set
        {
            if (_precioUnitario != value)
            {
                _precioUnitario = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PrecioUnitarioNumerico));
                OnPropertyChanged(nameof(Subtotal));
            }
        }
    }

    //propiedades strin a numericas para calculos del totoal
    public float CantidadNumerica
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_cantidad)) return 0;
            if (!float.TryParse(_cantidad, out float result)) return 0;
            return result > 0 ? result : 0; // Solo positivos
        }
    }
    public float PrecioUnitarioNumerico
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_precioUnitario)) return 0;
            if (!float.TryParse(_precioUnitario, out float result)) return 0;
            return result > 0 ? result : 0; // Solo positivos
        }
    }
    public float Subtotal => CantidadNumerica * PrecioUnitarioNumerico;

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public DetalleItem Clone()
    {
        return new DetalleItem
        {
            Nombre = this.Nombre,
            Cantidad = this.Cantidad,
            PrecioUnitario = this.PrecioUnitario
        };
    }
}
