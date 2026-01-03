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
    //////#EVENTOS/////
    private void OnMenuButtonClicked()
    {
        //Abre el Flyout al presionar el botón del toolbar
        IsPresented = !IsPresented;
    }
    private async void visualizarTodosLosDatos(object sender, EventArgs e)
    {
        bool band = false;

        if (Logica.Referencia)
        {
            if (Logica.Ingreso && !Logica.Egreso)
            {
                //referencia + ingreso validos

                visFecha.Text = visFecha.Text = $"Fecha: {Logica.Fecha.ToString("dd/MM/yyyy")}";
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

                visFecha.Text = visFecha.Text = $"Fecha: {Logica.Fecha.ToString("dd/MM/yyyy")}";
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

                visFecha.Text = visFecha.Text = $"Fecha: {Logica.Fecha.ToString("dd/MM/yyyy")}";
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
                visFecha.Text = visFecha.Text = $"Fecha: {Logica.Fecha.ToString("dd/MM/yyyy")}";
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
            if(!Logica.Ingreso && Logica.Egreso) //solo para visualizar, eliminable
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
            if(!Logica.Ingreso && !Logica.Egreso) //solo para visualizar, eliminable
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
            
           
            await Logica.GuardarArchMovimientos();

            //await Logica.CompartirArchMovimientos();
        }
        else
        {
            visInformeReferencia.Text = "exportacion fallida";
            visInformeReferencia.TextColor = Colors.Red;
        }
        //la visualizacion muestra la informacion actual

    }
    private async void irAlArchMovimientos(object sender, EventArgs e)
    {
        await Logica.AbrirArchMovimientos();
    }

    //private async void irAlArchDeudas(object sender, EventArgs e)
    //{
    //    await Logica.AbrirArchDeudas();
    //}
}