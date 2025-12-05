using System;

namespace Gi
{
    public static class Logica
    {
        public static string textoValidado { get; set; }

        public static string recibirTexto(string textoo)
        {
            if (string.IsNullOrWhiteSpace(textoo))
            {
                textoValidado = "¡Texto vacío!";
            }
            else
            {
                textoValidado = textoo;
            }

            return textoValidado;
        }
    }
}
