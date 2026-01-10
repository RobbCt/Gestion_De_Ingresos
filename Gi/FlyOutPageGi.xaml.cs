using System.Xml;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Maui.Graphics.Text;

namespace Gi;

public partial class FlyOutPageGi : FlyoutPage
{
    public FlyOutPageGi()
    {
        InitializeComponent();

        //Agregar un Toolbar
        ToolbarItems.Add(new ToolbarItem
        {
            IconImageSource = "menu.svg",
            Command = new Command(OnMenuButtonClicked),
        });
    }
    private async Task manejarExepciones((bool estado,string? msj) resultado)
    {
        if (resultado.estado)
            await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Éxito", resultado.msj, "Aceptar");
        else
            await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Error", resultado.msj, "Aceptar");
    }
    private void OnMenuButtonClicked()
    {
        //Abre el Flyout al presionar el botón del toolbar
        IsPresented = !IsPresented;
    }

    //////#EVENTOS/////
    private async void exportarArchMovimientos(object sender, EventArgs e)
    {
        bool band = false;

        if (Logica.Referencia)
        {
            if (Logica.Ingreso && !Logica.Egreso)
            {
                //referencia + ingreso validos

                visFecha.Text = $"Fecha: {Logica.Fecha.ToString("dd/MM/yyyy")}";
                visFecha.TextColor = Colors.White;

                visTipoDePago.Text = $"Tipo De Pago: {Logica.TipoDePago}";
                visTipoDePago.TextColor = Colors.White;

                visMotivo.Text = $"Motivo: {Logica.Motivo}";
                visMotivo.TextColor = Colors.White;

                visOrigen.Text = $"Origen: {Logica.Origen}";
                visOrigen.TextColor = Colors.White;

                visMontoIngreso.Text = $"Monto (ingreso): {Logica.MontoIngreso}";
                visMontoIngreso.TextColor = Colors.White;

                Logica.Destino = "--";
                visDestino.Text = $"Destino: {Logica.Destino}";
                visDestino.TextColor = Colors.White;

                Logica.DescDelEgreso = "--";
                visDescripcionDelEgreso.Text = $"Descripcion Del Egreso: {Logica.DescDelEgreso}";
                visDescripcionDelEgreso.TextColor = Colors.White;

                Logica.MontoEgreso = 0;
                visMontoEgreso.Text = $"Monto (egreso): {Logica.MontoEgreso}";
                visMontoEgreso.TextColor = Colors.White;

                band = true;
            }

            if (!Logica.Ingreso && Logica.Egreso)
            {
                //referencia + egreso validos

                visFecha.Text = $"Fecha: {Logica.Fecha.ToString("dd/MM/yyyy")}";
                visFecha.TextColor = Colors.White;

                visTipoDePago.Text = $"Tipo De Pago: {Logica.TipoDePago}";
                visTipoDePago.TextColor = Colors.White;

                visMotivo.Text = $"Motivo: {Logica.Motivo}";
                visMotivo.TextColor = Colors.White;

                Logica.Origen = "--";
                visOrigen.Text = $"Origen: {Logica.Origen}";
                visOrigen.TextColor = Colors.White;

                Logica.MontoIngreso = 0;
                visMontoIngreso.Text = $"Monto (ingreso): {Logica.MontoIngreso}";
                visMontoIngreso.TextColor = Colors.White;

                visDestino.Text = $"Destino: {Logica.Destino}";
                visDestino.TextColor = Colors.White;

                visDescripcionDelEgreso.Text = $"Descripcion Del Egreso: {Logica.DescDelEgreso}";
                visDescripcionDelEgreso.TextColor = Colors.White;

                visMontoEgreso.Text = $"Monto (egreso): {Logica.MontoEgreso}";
                visMontoEgreso.TextColor = Colors.White;

                band = true;
            }

            if (!Logica.Ingreso && !Logica.Egreso) //solo para visualizar, eliminable
            {
                //solo refernecia valida

                visFecha.Text = $"Fecha: {Logica.Fecha.ToString("dd/MM/yyyy")}";
                visFecha.TextColor = Colors.White;

                visTipoDePago.Text = $"Tipo De Pago: {Logica.TipoDePago}";
                visTipoDePago.TextColor = Colors.White;

                visMotivo.Text = $"Motivo: {Logica.Motivo}";
                visMotivo.TextColor = Colors.White;

                Logica.Origen = "--";
                visOrigen.Text = $"Origen: {Logica.Origen}";
                visOrigen.TextColor = Colors.White;

                Logica.MontoIngreso = 0;
                visMontoIngreso.Text = $"Monto (ingreso): {Logica.MontoIngreso}";
                visMontoIngreso.TextColor = Colors.White;

                Logica.Destino = "--";
                visDestino.Text = $"Destino: {Logica.Destino}";
                visDestino.TextColor = Colors.White;

                Logica.DescDelEgreso = "--";
                visDescripcionDelEgreso.Text = $"Descripcion Del Egreso: {Logica.DescDelEgreso}";
                visDescripcionDelEgreso.TextColor = Colors.White;

                Logica.MontoEgreso = 0;
                visMontoEgreso.Text = $"Monto (egreso): {Logica.MontoEgreso}";
                visMontoEgreso.TextColor = Colors.White;

            }
            if (Logica.Ingreso && Logica.Egreso) //solo para visualizar, eliminable
            {
                visFecha.Text = $"Fecha: {Logica.Fecha.ToString("dd/MM/yyyy")}";
                visFecha.TextColor = Colors.White;

                visTipoDePago.Text = $"Tipo De Pago: {Logica.TipoDePago}";
                visTipoDePago.TextColor = Colors.White;

                visMotivo.Text = $"Motivo: {Logica.Motivo}";
                visMotivo.TextColor = Colors.White;

                visOrigen.Text = $"Origen: {Logica.Origen}";
                visOrigen.TextColor = Colors.White;

                visMontoIngreso.Text = $"Monto (ingreso): {Logica.MontoIngreso}";
                visMontoIngreso.TextColor = Colors.White;

                visDestino.Text = $"Destino: {Logica.Destino}";
                visDestino.TextColor = Colors.White;

                visDescripcionDelEgreso.Text = $"Descripcion Del Egreso: {Logica.DescDelEgreso}";
                visDescripcionDelEgreso.TextColor = Colors.White;

                visMontoEgreso.Text = $"Monto (egreso): {Logica.MontoEgreso}";
                visMontoEgreso.TextColor = Colors.White;
            }

        }
        else //solo para visualizar, eliminable
        {
            if (Logica.Ingreso && !Logica.Egreso)//solo para visualizar, eliminable
            {
                //solo ingreso valido

                visFecha.Text = visFecha.Text = $"Fecha: --";
                visFecha.TextColor = Colors.White;

                Logica.TipoDePago = "--";
                visTipoDePago.Text = $"Tipo De Pago: {Logica.TipoDePago}";
                visTipoDePago.TextColor = Colors.White;

                Logica.Motivo = "--";
                visMotivo.Text = $"Motivo: {Logica.Motivo}";
                visMotivo.TextColor = Colors.White;

                visOrigen.Text = $"Origen: {Logica.Origen}";
                visOrigen.TextColor = Colors.White;

                visMontoIngreso.Text = $"Monto (ingreso): {Logica.MontoIngreso}";
                visMontoIngreso.TextColor = Colors.White;

                Logica.Destino = "--";
                visDestino.Text = $"Destino: {Logica.Destino}";
                visDestino.TextColor = Colors.White;

                Logica.DescDelEgreso = "--";
                visDescripcionDelEgreso.Text = $"Descripcion Del Egreso: {Logica.DescDelEgreso}";
                visDescripcionDelEgreso.TextColor = Colors.White;

                Logica.MontoEgreso = 0;
                visMontoEgreso.Text = $"Monto (egreso): {Logica.MontoEgreso}";
                visMontoEgreso.TextColor = Colors.White;

            }
            if (!Logica.Ingreso && Logica.Egreso) //solo para visualizar, eliminable
            {
                //solo egreso valido

                visFecha.Text = visFecha.Text = $"Fecha: --";
                visFecha.TextColor = Colors.White;

                Logica.TipoDePago = "--";
                visTipoDePago.Text = $"Tipo De Pago: {Logica.TipoDePago}";
                visTipoDePago.TextColor = Colors.White;

                Logica.Motivo = "--";
                visMotivo.Text = $"Motivo: {Logica.Motivo}";
                visMotivo.TextColor = Colors.White;

                Logica.Origen = "--";
                visOrigen.Text = $"Origen: {Logica.Origen}";
                visOrigen.TextColor = Colors.White;

                Logica.MontoIngreso = 0;
                visMontoIngreso.Text = $"Monto (ingreso): {Logica.MontoIngreso}";
                visMontoIngreso.TextColor = Colors.White;

                visDestino.Text = $"Destino: {Logica.Destino}";
                visDestino.TextColor = Colors.White;

                visDescripcionDelEgreso.Text = $"Descripcion Del Egreso: {Logica.DescDelEgreso}";
                visDescripcionDelEgreso.TextColor = Colors.White;

                visMontoEgreso.Text = $"Monto (egreso): {Logica.MontoEgreso}";
                visMontoEgreso.TextColor = Colors.White;
            }
            if (!Logica.Ingreso && !Logica.Egreso) //solo para visualizar, eliminable
            {
                //nada valido

                visFecha.Text = visFecha.Text = $"Fecha: --";
                visFecha.TextColor = Colors.White;

                Logica.TipoDePago = "--";
                visTipoDePago.Text = $"Tipo De Pago: {Logica.TipoDePago}";
                visTipoDePago.TextColor = Colors.White;

                Logica.Motivo = "--";
                visMotivo.Text = $"Motivo: {Logica.Motivo}";
                visMotivo.TextColor = Colors.White;

                Logica.Origen = "--";
                visOrigen.Text = $"Origen: {Logica.Origen}";
                visOrigen.TextColor = Colors.White;

                Logica.MontoIngreso = 0;
                visMontoIngreso.Text = $"Monto (ingreso): {Logica.MontoIngreso}";
                visMontoIngreso.TextColor = Colors.White;

                Logica.Destino = "--";
                visDestino.Text = $"Destino: {Logica.Destino}";
                visDestino.TextColor = Colors.White;

                Logica.DescDelEgreso = "--";
                visDescripcionDelEgreso.Text = $"Descripcion Del Egreso: {Logica.DescDelEgreso}";
                visDescripcionDelEgreso.TextColor = Colors.White;

                Logica.MontoEgreso = 0;
                visMontoEgreso.Text = $"Monto (egreso): {Logica.MontoEgreso}";
                visMontoEgreso.TextColor = Colors.White;
            }
            else if (Logica.Ingreso && Logica.Egreso) //solo para visualizar, eliminable
            {
                //ingreso + egreso validos

                visFecha.Text = visFecha.Text = $"Fecha: --";
                visFecha.TextColor = Colors.White;

                Logica.TipoDePago = "--";
                visTipoDePago.Text = $"Tipo De Pago: {Logica.TipoDePago}";
                visTipoDePago.TextColor = Colors.White;

                Logica.Motivo = "--";
                visMotivo.Text = $"Motivo: {Logica.Motivo}";
                visMotivo.TextColor = Colors.White;

                visOrigen.Text = $"Origen: {Logica.Origen}";
                visOrigen.TextColor = Colors.White;

                visMontoIngreso.Text = $"Monto (ingreso): {Logica.MontoIngreso}";
                visMontoIngreso.TextColor = Colors.White;

                visDestino.Text = $"Destino: {Logica.Destino}";
                visDestino.TextColor = Colors.White;

                visDescripcionDelEgreso.Text = $"Descripcion Del Egreso: {Logica.DescDelEgreso}";
                visDescripcionDelEgreso.TextColor = Colors.White;

                visMontoEgreso.Text = $"Monto (egreso): {Logica.MontoEgreso}";
                visMontoEgreso.TextColor = Colors.White;

            }
        }

        //band significa la exportacion de la infromacion actual (una convinacion valida
        if (band)
        {
            visInformeReferencia.Text = "exportacion exitosa";
            visInformeReferencia.TextColor = Colors.Green;

            var resultado = await Logica.GuardarArchMovimientos();

            if (resultado.estado)
                await manejarExepciones((true, "Archivo Movimientos guardado"));
            else
                await manejarExepciones(resultado);

            Logica.ResetearMovimiento();
        }
        else if (Logica.Referencia && Logica.Ingreso && Logica.Egreso)//de las combinaciones validas, completo las 3
        {
            visInformeReferencia.Text = "exportacion fallida";
            visInformeReferencia.TextColor = Colors.Red;

            await manejarExepciones((false, "No se pudo exportar el archivo, complete solo los campos correspondientes"));
        }
        else //des las combinaciones validas, completo 1
        {
            visInformeReferencia.Text = "exportacion fallida";
            visInformeReferencia.TextColor = Colors.Red;

            await manejarExepciones((false, "No se pudo exportar el archivo, complete los campos correspondientes minimos"));
        }
        //la visualizacion muestra la informacion actual

    }
    private async void irAlArchMovimientos(object sender, EventArgs e)
    {
        var resultado = await Logica.IrAlArchMovimientos();
        if (!resultado.estado)
            await manejarExepciones(resultado);
    }
    private async void compartirArchMovimientos(object sender, EventArgs e)
    {
        var resultado = await Logica.CompartirArchMovimientos();
        if(!resultado.estado)
             await manejarExepciones(resultado);
    }


    /*private async void exportarArchDeudas(object sender, EventArgs e)
    {
        //fijate validaciones de datos antes de poder eportar
    
        var resultado = await Logica.GuardarArchDeudas();

        if (resultado.estado)
            await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Éxito", "Archivo Deudas guardado correctamente", "OK");
        else
            await manejarExepciones(resultado);
    }*/
    /*private async void irAlArchDeudas(object sender, EventArgs e)
    {
        var resultado = await Logica.irAlArchDeudas();
        await manejarExepciones(resultado);
    }*/
    /*private async void compartirArchDeudas(object sender, EventArgs e)
    {
        var resultado = await Logica.CompartirArchDeudas();
        await manejarExepciones(resultado);
    }*/


}