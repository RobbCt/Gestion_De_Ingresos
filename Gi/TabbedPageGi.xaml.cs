namespace Gi;

public partial class TabbedPageGi : TabbedPage
{
	//lista para las opciones
	public List<string> TiposDePagos { get; set; }
	//opcion seleccionada
	public string? TiposDePagosSelec { get; set; }

	public TabbedPageGi()
	{
		InitializeComponent();

		//opciones de tipo de pago
		TiposDePagos = new List<string>
		{
			"Efectivo",
			"Transferencia",
			"Tarjeta",
			"Otro",
		};
		BindingContext = this;
	}

    //////#Eventos/////
    private void setPropReferencia(object sender, EventArgs e)
	{
		if (Logica.validarReferencia(fecha.Text, tipoDePago.SelectedItem, motivo.Text))
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
	}
	private void setPropIngreso(object sender, EventArgs e)
	{
		if (Logica.validarIngreso(origen.Text, montoIngreso.Text))
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
	}
    private void setPropEgreso(object sender, EventArgs e)
    {
        if (Logica.validarEgreso(destino.Text, descDelGasto.Text, montoEgreso.Text))
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
      
    }

}