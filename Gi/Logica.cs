using System;
namespace Gi;

public static class Logica
{
    //propiedades
    public static bool Referencia { get; set; } = false;
        public static DateTime Fecha { get; set; }
        public static string? TipoDePago { get; set; }
        public static string? Motivo { get; set; }



    public static bool Ingreso { get; set; } = false;
        public static string? Origen { get; set; }
        public static float MontoIngreso { get; set; }



    public static bool Egreso { get; set; } = false;
        public static string? Destino { get; set; }
        public static string? DescDelIngreso { get; set; }
        public static float MontoEgreso { get; set; }


    //metodos
    public static bool validarReferencia(string fecha,object tipoDePago,string motivo)
    {
        bool fechaValida = DateTime.TryParse(fecha, out DateTime auxFecha);
        bool tipoDePagoValida = tipoDePago != null;
        bool motivoValido = !string.IsNullOrWhiteSpace(motivo);
       
        if (fechaValida && tipoDePagoValida && motivoValido)
        {
            Referencia = true;
            Fecha = auxFecha;
            TipoDePago = tipoDePago!.ToString();
            Motivo = motivo;
        }
        else
            Referencia = false;

        return Referencia;
    }
    public static bool validarIngreso(string origen,string monto)
    {
        bool origenValido = !string.IsNullOrWhiteSpace(origen);
        bool montoValido = false;

        if (float.TryParse(monto, out float auxMonto))
        {
            if (auxMonto > 0)
                montoValido = true;
        }

        if (origenValido && montoValido)
        {
            Ingreso = true;
            Origen = origen;
            MontoIngreso = auxMonto;
        }
        else
            Ingreso = false;

        return Ingreso;
    }
    public static bool validarEgreso(string destino,string descDelGasto,string monto)
    {
        bool destinoValido = !string.IsNullOrWhiteSpace(destino);
        bool descDelGastoValido = !string.IsNullOrWhiteSpace(descDelGasto);
        bool montoValido = false;

        if (float.TryParse(monto, out float auxMonto))
        {
            if (auxMonto > 0)
                montoValido = true;
        }

        if (destinoValido && montoValido)
        {
            Egreso = true;
            Destino = destino;
            MontoEgreso = - auxMonto;

            if (descDelGastoValido)
                DescDelIngreso = descDelGasto;
            else
                DescDelIngreso = "-";
        }
        else
            Egreso = false;

        return Egreso;
    }

    


    //verificacion de pusheo
}
