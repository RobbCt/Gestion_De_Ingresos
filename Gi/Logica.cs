using System;
namespace Gi;
//par exportacion al exel
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;


public static class Logica
{
    //PROPIEDADES
    public static bool Referencia { get; set; } = false;
        public static DateTime Fecha { get; set; }
        public static string? TipoDePago { get; set; }
        public static string? Motivo { get; set; }



    public static bool Ingreso { get; set; } = false;
        public static string? Origen { get; set; }
        public static float MontoIngreso { get; set; }



    public static bool Egreso { get; set; } = false;
        public static string? Destino { get; set; }
        public static string? DescDelEgreso { get; set; }
        public static float MontoEgreso { get; set; }

    //UBICACION DE LOS ARCHIVOS
    private static readonly string RutaArchMovimientos =
       Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"Movimientos.xlsx");
        //ruta a la carpeta Documents inaccesible interna en Andrid, nombre y tipo del archivo

    //public static readonly string RutaArchMovimientos =
        //Path.Combine(FileSystem.Current.GetDirectoryPath(Environment.SpecialFolder.MyDocuments),"Movimientos.xlsx");
        //ruta a la carpeta Documents accesible en Andrid, nombre y tipo del archivo

    private static readonly string RutaArchDeudas =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Movimientos.xlsx"
        );//ruta a la carpeta Documents accesible en Andrid, nombre y tipo del archivo


    //METODOS
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
                DescDelEgreso = descDelGasto;
            else
                DescDelEgreso = "-";
        }
        else
            Egreso = false;

        return Egreso;
    }
    public static async Task GuardarArchMovimientos()
    {
        //alguien hoy me empezo a joder con el trycatch...
        try
        {
            //declaracion clase q maneja el exel (obj)
            XLWorkbook workbook = constructorArch(RutaArchMovimientos);

            //exel maneja hojas, solo necesito una con el nombre de Datos
            // ?? (Izq == NULL -> se ejetuca Der, si no Izq) elegancia de C#
            var hoja = workbook.Worksheets.FirstOrDefault() ?? workbook.Worksheets.Add("Datos");

            int ultimaFila = hoja.LastRowUsed()?.RowNumber() + 1 ?? 1;


            //formato ingreso: fecha|tipoDeMovimiento|tipoDePago|motivo|origen |             |monto|
            //formato egreso:  fecha|tipoDeMovimiento|tipoDePago|motivo|destino|descDelEgreso|monto|


            //string fecha listo
            string tipoMovimiento = Ingreso ? "Ingreso" : "Egreso";
            //string tipoDePago listo
            //string motivo listo
            string origenDestino = Ingreso ? "De: " + Origen : "Hacia: " + Destino;
            string descDelEgreso = Ingreso ? "-" : DescDelEgreso!; //! para deajar en claro q esta validado
            float monto = Ingreso ? MontoIngreso : MontoEgreso;

            //escribo los datos en la ultima fila libre
            hoja.Cell(ultimaFila, 1).Value = Fecha.ToString("dd/MM/yyyy");
            hoja.Cell(ultimaFila, 2).Value = tipoMovimiento;
            hoja.Cell(ultimaFila, 3).Value = TipoDePago;
            hoja.Cell(ultimaFila, 4).Value = Motivo;
            hoja.Cell(ultimaFila, 5).Value = origenDestino;
            hoja.Cell(ultimaFila, 6).Value = descDelEgreso;
            hoja.Cell(ultimaFila, 7).Value = monto;

            //actualiza (y si es necesario crea en la ruta) el achivo de movimientos
            workbook.SaveAs(RutaArchMovimientos);

            //quiero ver la ruta exacta del archivo en mi dispositivo
            await Application.Current!.Windows[0].Page!
            .DisplayAlert("Ruta del archivo", RutaArchMovimientos, "OK");

        }
        catch (Exception ex)
        {
            await Application.Current!.Windows[0].Page!.DisplayAlert("Error al guardar", ex.Message, "OK");
        }

    }
    public static XLWorkbook constructorArch(string rutaArch)
    {
        //instancio el XLWorkbook (archivo de exel en memoria temporal) con la
        //info el archivo existente o uno vacio (si no existe)
        return File.Exists(rutaArch) ? new XLWorkbook(rutaArch) : new XLWorkbook();
    }
    public static async Task AbrirArchMovimientos()
    {
        if (!File.Exists(RutaArchMovimientos))
        {
            await Application.Current!.Windows[0].Page!.DisplayAlert("Archivo no encontrado",
                                                                     "Todavía no existe el archivo de Movimientos.",
                                                                     "OK");
            return;
        }

        //alguien hoy me empezo a joder con el trycatch...
        try
        {
            //esperar q andorid abra el archivo en rt
            await Launcher.Default.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(RutaArchMovimientos)
            });
        }
        catch (Exception ex)
        {
            await Application.Current!.Windows[0].Page!.DisplayAlert("Error al abrir",ex.Message,"OK");
        }
    }




    //provisorio (futuro boton de compartir exel)
    //public static async Task CompartirArchMovimientos()
    //{
    //    string ruta = Path.Combine(FileSystem.AppDataDirectory, "Movimientos.xlsx");
    //
    //    if (!File.Exists(ruta))
    //        return;
    //
    //    await Share.RequestAsync(new ShareFileRequest
    //    {
    //        Title = "Exportar movimientos",
    //        File = new ShareFile(ruta)
    //        
    //    });
    //}
}
