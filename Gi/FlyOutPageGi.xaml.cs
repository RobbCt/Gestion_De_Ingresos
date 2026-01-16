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

        //Combinaciones válidas para exportar:
        //1. Referencia + Ingreso válidos
        //2. Referencia + Egreso válidos

        if (Logica.Referencia)
        {
            if (Logica.Ingreso && !Logica.Egreso)
            {
                // referencia + ingreso validos
                band = true;
            }
            else if (!Logica.Ingreso && Logica.Egreso)
            {
                // referencia + egreso validos
                band = true;
            }
        }

        //band significa la exportacion de la infromacion actual (una convinacion valida)
        if (band)
        {
            var resultado = await Logica.GuardarArchMovimientos();

            if (resultado.estado)
                await manejarExepciones((true, "Archivo Movimientos guardado"));
            else
                await manejarExepciones(resultado);

            Logica.ResetearMovimiento();
        }
        else if (Logica.Referencia && Logica.Ingreso && Logica.Egreso)//de las combinaciones validas, completo las 3
        {
            await manejarExepciones((false, "No se pudo exportar el archivo, complete solo los campos correspondientes"));
        }
        else //de las combinaciones validas, completo 1
        {
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


    // En el constructor o donde tengas otros botones, agrega:
    // var analizarBtn = new Button { Text = "🔍 Analizar Plantilla" };
    // analizarBtn.Clicked += AnalizarPlantilla_Clicked;
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