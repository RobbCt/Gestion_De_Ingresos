using System;
namespace Gi;
//para exportacion del exel
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using Gi.ViewModels;
//using Kotlin.Contracts;
//using Kotlin;
using System.IO;
using Gi.Models;
using System.Collections.Generic;
using System.Globalization;


public static class Logica
{
    //PROPIEDADES
    public static bool Referencia { get; set; } = false;
    public static DateTime Fecha { get; set; } = DateTime.Now;
        public static string? TipoDePago { get; set; }
        public static string? Motivo { get; set; }



    public static bool Ingreso { get; set; } = false;
        public static string? Origen { get; set; }
        public static float MontoIngreso { get; set; }



    public static bool Egreso { get; set; } = false;
        public static string? Destino { get; set; }
        public static string? DescDelEgreso { get; set; }
        public static float MontoEgreso { get; set; }


    public static List<DetalleItem> Detalles { get; set; } = new();


    //METODOS
    public static bool ValidarReferencia(string fecha,object tipoDePago,string motivo)
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
    public static bool ValidarIngreso(string origen,string monto)
    {
        bool origenValido = !string.IsNullOrWhiteSpace(origen);
        bool montoValido = false;

        if (float.TryParse(monto, NumberStyles.Float, CultureInfo.InvariantCulture, out float auxMonto))
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
    public static bool ValidarEgreso(string destino,string descDelGasto,string monto)
    {
        bool destinoValido = !string.IsNullOrWhiteSpace(destino);
        bool descDelGastoValido = !string.IsNullOrWhiteSpace(descDelGasto);
        bool montoValido = false;

        if (float.TryParse(monto, NumberStyles.Float, CultureInfo.InvariantCulture, out float auxMonto))
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
                DescDelEgreso = descDelGasto;
            else
                DescDelEgreso = "-";
        }
        else
            Egreso = false;

        return Egreso;
    }

    //METODOS DE UBICACION DE LOS ARCHIVOS
    //AppDataDirectory y consultas al sistema se deben hacer luego de iniciar la app completamente (ej: inicio por primera vez)
    //y la propiedad como es static se inicializa (pide su valor) antes de iniciar la app completamente (y el sistema no puede devolver su valor, q es la ruta del archivo)
    //entonces lo q pasa es q pregunto en destiemnpo y la app crashea, y aca entran los metodos...
    public static string CarpetaArchivos()
    {
        var ruta = Path.Combine(FileSystem.AppDataDirectory, "archivos");
        // Asegurarse de que exista la carpeta en la ruta, sino hacer una carpeta con la ruta
        if (!Directory.Exists(ruta))
            Directory.CreateDirectory(ruta);
        return ruta;
        //ruta a la carpeta inaccesible interna en Andrid donde se alojan los archivos
    }
    public static string RutaArchMovimientos()
    {
        return Path.Combine(CarpetaArchivos(), "Movimientos.xlsx");
        //ruta al archivo, nombre y tipo
    }
    public static string RutaArchDeudas()
    {
        return Path.Combine(CarpetaArchivos(), "Deudas.xlsx");
        //ruta al archivo, nombre y tipo
    }
    public static XLWorkbook AbrirOcrearWorkbook(string rutaArch)
    {
        //instancio el XLWorkbook (archivo de exel en memoria temporal) con la
        //info el archivo existente (la op false podria quitarla, pero prefiero capturar los datos en cualquier caso)
        return File.Exists(rutaArch) ? new XLWorkbook(rutaArch) : new XLWorkbook();
    }
   
    public static void CrearArchMovimientos()
    {
        //crear ArchMovimientos en el arranque limpio (el pirmer inicio)
        
        string ruta = RutaArchMovimientos();

        if(!File.Exists(ruta))
        {
            //using = el archivo esta en memoria temporal solo dentro de las llaves...
            using (var workbook = new XLWorkbook())
            {
                var hoja = workbook.Worksheets.Add("Datos");




                //DESCRIPCIÓN (columnas 1 a 4)
                hoja.Range(1, 1, 1, 4).Merge().Value = "DESCRIPCIÓN";

                //INGRESO / EGRESO (columnas 5 a 7)
                hoja.Range(1, 5, 1, 7).Merge().Value = "INGRESO/EGRESO";

                //DETALLES (columnas 8 a 10)
                hoja.Range(1, 8, 1, 10).Merge().Value = "DETALLES";

                var filaTitulos = hoja.Range(1, 1, 1, 10);
                filaTitulos.Style.Font.Bold = true;
                filaTitulos.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                filaTitulos.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


                //ENCABEZADOS DE COLUMNA
                hoja.Cell(2, 1).Value = "Fecha";
                hoja.Cell(2, 2).Value = "Tipo de movimiento";
                hoja.Cell(2, 3).Value = "Tipo de pago";
                hoja.Cell(2, 4).Value = "Motivo";

                hoja.Cell(2, 5).Value = "Origen / Destino";
                hoja.Cell(2, 6).Value = "Descripción";
                hoja.Cell(2, 7).Value = "Monto";

                hoja.Cell(2, 8).Value = "Nombre";
                hoja.Cell(2, 9).Value = "Cantidad";
                hoja.Cell(2, 10).Value = "Precio C/u";

                
                //FORMATO DE CELDA CENTRADA
                var filaEncabezados = hoja.Range(2, 1, 2, 10);

                filaEncabezados.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                filaEncabezados.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                filaEncabezados.Style.Border.OutsideBorderColor = XLColor.Black;
                filaEncabezados.Style.Border.InsideBorderColor = XLColor.Black;

                filaEncabezados.Style.Fill.BackgroundColor = XLColor.FromHtml("#1b998b");
                filaEncabezados.Style.Font.FontColor = XLColor.Black;
                filaEncabezados.Style.Font.Bold = true;
                filaEncabezados.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                filaEncabezados.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                hoja.Columns().AdjustToContents();

                //ANCHO DE COLUMNAS CON MAS INFO
                hoja.Column(1).Width = 14;
                hoja.Column(4).Width = 14;
                hoja.Column(6).Width = 14;
                hoja.Column(7).Width = 14;


                workbook.SaveAs(ruta);
            }//...luego de escribirse se libera (optimizar tiempo)
        }
    }
    public static Task <(bool estado, string? msj)> GuardarArchMovimientos()
    {
        //alguien hoy me empezo a joder con el trycatch...
        try
        {
            //declaracion clase q maneja el exel (obj)
            using (XLWorkbook workbook = AbrirOcrearWorkbook(RutaArchMovimientos()))
            {

                //exel maneja hojas, solo necesito una con el nombre de Datos
                // ?? (Izq == NULL -> se ejetuca Der, si no Izq) elegancia de C#
                var hoja = workbook.Worksheets.FirstOrDefault() ?? workbook.Worksheets.Add("Datos");

                int filaInicio = hoja.LastRowUsed()?.RowNumber() + 1 ?? 3;



                //formato ingreso: fecha|tipoDeMovimiento|tipoDePago|motivo|origen |             |monto|nombre|cantidad|precioC/u|
                //formato egreso:  fecha|tipoDeMovimiento|tipoDePago|motivo|destino|descDelEgreso|monto|nombre|cantidad|precioC/u|


                //string fecha listo
                string tipoMovimiento = Ingreso ? "Ingreso" : "Egreso";
                //string tipoDePago listo
                //string motivo listo
                string origenDestino = Ingreso ? Origen! : Destino!;
                string descDelEgreso = Ingreso ? "-" : DescDelEgreso!; //! para deajar en claro q esta validado
                float monto = Ingreso ? MontoIngreso : MontoEgreso;
                //grilla con detalles listo



                //si hay datos de detalles los escribimos, sino se deja las celdas 8-10 vacias
                var detalles = Detalles ?? new List<DetalleItem>();

                //habra detalles
                int filas = detalles.Count > 0 ? detalles.Count : 1;

                //escribo en el exel la fila de referencia + ingreso/egreso, al final de la 
                //fila inserto la grilla de detalles (3 columnas, N filas cargadas por el usuario)
                for (int i = 0; i < filas; i++)
                {
                    int filaActual = filaInicio + i;
                    
                    //referencia + ingreso/egreso
                    if (i == 0)
                    {
                        //escribo los datos en la ultima fila libre
                        hoja.Cell(filaActual, 1).Value = Fecha.ToString("dd/MM/yyyy");
                        hoja.Cell(filaActual, 2).Value = tipoMovimiento;
                        hoja.Cell(filaActual, 3).Value = TipoDePago;
                        hoja.Cell(filaActual, 4).Value = Motivo;
                        hoja.Cell(filaActual, 5).Value = origenDestino;
                        hoja.Cell(filaActual, 6).Value = descDelEgreso;
                        hoja.Cell(filaActual, 7).Value = monto;
                    }

                    //grilla de detalles solo si los hay
                    if (detalles.Count > 0)
                    {
                        var detalle = detalles[i];
                        hoja.Cell(filaActual, 8).Value = detalle.Nombre;
                        hoja.Cell(filaActual, 9).Value = detalle.CantidadNumerica;
                        hoja.Cell(filaActual, 10).Value = detalle.PrecioUnitarioNumerico;
                    }
                    else
                    {
                        //celdas vacías si no los hay
                        hoja.Cell(filaActual, 8).Clear();
                        hoja.Cell(filaActual, 9).Clear();
                        hoja.Cell(filaActual, 10).Clear();

                    }

                }

                //centrar celdas escritas
                hoja.Range(filaInicio, 1, filaInicio + filas - 1, 10)
                    .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                //linea negrita divisoria de movimiento
                int filaFinal = filaInicio + filas - 1;
                hoja.Range(filaFinal, 1, filaFinal, 10)
                    .Style.Border.BottomBorder = XLBorderStyleValues.Medium;

                //formato de plata/biyuya/la lana/el money
                hoja.Range(filaInicio, 7, filaInicio + filas - 1, 7)
                    .Style.NumberFormat.Format = "$ #,##0.00";

                hoja.Range(filaInicio, 10, filaInicio + filas - 1, 10)
                    .Style.NumberFormat.Format = "$ #,##0.00";

                //actualiza (y si es necesario crea en la ruta) el achivo de movimientos
                workbook.SaveAs(RutaArchMovimientos());
            }

            return Task.FromResult<(bool estado, string? msj)>((true, null));
        }
        catch (Exception ex)
        {
            return Task.FromResult<(bool estado, string? msj)>((false, ex.Message));
        }
    }
    public static async Task<(bool estado, string? msj)> IrAlArchMovimientos()
    {
        if (!File.Exists(RutaArchMovimientos()))
            return (false, "Archivo Movimientos no encontrado");
        
        //alguien hoy me empezo a joder con el trycatch...
        try
        {
            //esperar q andorid abra el archivo en rt
            await Launcher.Default.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(RutaArchMovimientos())
            });

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
            //retornar la tupla para separar la UI y back
        }
    }
    public static async Task<(bool estado, string? msj)> CompartirArchMovimientos()
    {
        string ruta = RutaArchMovimientos();

        if (!File.Exists(ruta))
            return (false, "El archivo Movimientos no existe");

        try
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Compartir Movimientos.xlsx",
                File = new ShareFile(ruta)
            });
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
            //retornar la tupla para separar la UI y back
        }
    }

    /*public static void CrearArchDeudas()
    {
        //crear ArchMovimientos en el arranque limpio (el pirmer inicio)

        string ruta = RutaArchDeudas();

        if (!File.Exists(ruta))
        {
            //using = el archivo esta en memoria temporal solo dentro de las llaves...
            using (var workbook = new XLWorkbook())
            {
                var hoja = workbook.Worksheets.Add("Historial");

                hoja.Cell(1, 1).Value = 
                hoja.Cell(1, 2).Value = 
                hoja.Cell(1, 3).Value = 
                hoja.Cell(1, 4).Value = 
                hoja.Cell(1, 5).Value = 
                hoja.Cell(1, 6).Value = 
                hoja.Cell(1, 7).Value = 

                workbook.SaveAs(ruta);
            }
        }
    }*/
    /*public static Task <(bool estado, string? msj)> GuardarArchDeudas()
    {
        try
        {
            using (XLWorkbook workbook = AbrirOcrearWorkbook(RutaArchDeudas()))
            {
                var hoja = workbook.Worksheets.FirstOrDefault() ?? workbook.Worksheets.Add("Historia");

                int ultimaFila = hoja.LastRowUsed()?.RowNumber() + 1 ?? 2;

                hoja.Cell(ultimaFila, 1).Value = 
                hoja.Cell(ultimaFila, 2).Value = 
                hoja.Cell(ultimaFila, 3).Value = 
                hoja.Cell(ultimaFila, 4).Value = 
                hoja.Cell(ultimaFila, 5).Value = 
                hoja.Cell(ultimaFila, 6).Value = 
                hoja.Cell(ultimaFila, 7).Value = 

                workbook.SaveAs(RutaArchDeudas());
            }

            return Task.FromResult<(bool estado, string? msj)>((true, null));
        }
        catch (Exception ex)
        {
            return Task.FromResult<(bool estado, string? msj)>((false, ex.Message));
        }
    }*/
    /*public static async Task<(bool esatdo, string? msj)> irAlArchDeudas()
    {
        if (!File.Exists(RutaArchDeudas()))
            return (false, "Archivo Deudas no encontrado");
        
        try
        {
            await Launcher.Default.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(RutaArchDeudas())
            });

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }*/
    /*public static async Task<(bool estado, string? msj)> CompartirArchDeudas()
    {
        string ruta = Path.Combine(FileSystem.AppDataDirectory, "Deudas.xlsx");

        if (!File.Exists(ruta))
            return (false, "El archivo Deudas no existe");

        try
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Compartir Deudas.xlsx",
                File = new ShareFile(ruta)
            });
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }*/


    public static event Action? ResetGlobalSolicitado;

    public static void ResetearReferencia()
    {
        Referencia = false;
        Fecha = DateTime.Now;
        TipoDePago = null;
        Motivo = null;
    }

    public static void ResetearIngreso()
    {
        Ingreso = false;
        Origen = null;
        MontoIngreso = 0;
    }

    public static void ResetearEgreso()
    {
        Egreso = false;
        Destino = null;
        DescDelEgreso = null;
        MontoEgreso = 0;
    }

    public static void ResetearMovimiento()
    {
        ResetearReferencia();
        ResetearIngreso();
        ResetearEgreso();
        Detalles.Clear();

        ResetGlobalSolicitado?.Invoke();
    }



    

    //FlyOutPage.cs:
    //obtener ruta✅
    //crear exel✅
    //actualizar ordenadamente el exel✅
    //abrir exel✅
    //incluir recurso de plantilla bonita de exel e implementar en el archivo🔴3
    //aunque no estoy seguro de como implenmentar Deudas.xlsx, dejar el codigo listo para su implementacion en todas las formas de Movimientos.xlsx✅
    //limpiar el evento de exportar (me refiero a la banda de if q tiene)

    //FlyOutPage.xaml:
    //quitar la visualisacion de todos los datos en el flyout
    //dejarle un button de exportar ✅
    //incluir button para compartir exel✅

    //TabbedPage.cs:
    //implementar una grilla de "mas detalles" con un chekbox para el ingreso de varios articulos🔴1
    //contemplar todas las posibles validaciones q trae la grilla (valores numericos en cantidades, nombres no opcionales, combinaciones de bugs, etc)🔴1
    //boton para agregar filas a la grilla dinamica🔴1
    //hacer un ViewModel a cada pestana de tabbed, bindear todos los entrys✅

    //TabbedPage.xaml:
    //incluir button resetear en cada pestana✅
    //bindear las pestana del TabbedPage✅
    //una vez exportado un movimiento, q todos los entry se borren (y poner en false/cero/null todas las properties por security)✅

    //FlyOutPage.cs:
    //hcaer/encontrar un algoritmo mas eficiente para encontrar las posibles combinaciones para poder exportar
    //sin tantos if🔴2
}
