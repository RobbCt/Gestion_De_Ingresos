using Gi.ViewModels;

namespace Gi;

public partial class TabbedPageGi : TabbedPage
{
    public TabbedPageGi()
    {
        InitializeComponent();
    }

    /*private void setPropReferencia(object sender, EventArgs e)
	{
		if (Logica.ValidarReferencia(fecha.Text, tipoDePago.SelectedItem, motivo.Text))
		{
			informeReferencia.Text = "Guardado Exitoso";
			informeReferencia.TextColor = Colors.Green;
            DisplayAlertAsync("", "Datos correctos guardados", "Aceptar");
        }
		else
		{
			informeReferencia.Text = "Datos Incompletos"; 
			informeReferencia.TextColor = Colors.Red;
            DisplayAlertAsync("", "Datos incorrectos", "Aceptar");
        }
	}*/
    /*private void setPropIngreso(object sender, EventArgs e)
    {
        if (Logica.ValidarIngreso(origen.Text, montoIngreso.Text))
        {
            informeIngreso.Text = "Guardado Exitoso";
            informeIngreso.TextColor = Colors.Green;
            DisplayAlertAsync("", "Datos correctos guardados", "Aceptar");
        }
        else
        {
            informeIngreso.Text = "Datos Incompletos";
            informeIngreso.TextColor = Colors.Red;
            DisplayAlertAsync("", "Datos incorrectos", "Aceptar");
        }
    }*/
    /*private void setPropEgreso(object sender, EventArgs e)
    {
        if (Logica.ValidarEgreso(destino.Text, descDelGasto.Text, montoEgreso.Text))
        {
            informeEgreso.Text = "Guardado Exitoso";
            informeEgreso.TextColor = Colors.Green;
            DisplayAlertAsync("", "Datos correctos guardados", "Aceptar");
        }
        else
        {
            informeEgreso.Text = "Datos Incompletos";
            informeEgreso.TextColor = Colors.Red;
            DisplayAlertAsync("", "Datos incorrectos", "Aceptar");
        }
    }*/
}