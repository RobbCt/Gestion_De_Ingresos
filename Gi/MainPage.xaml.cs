//using HealthKit;
namespace Gi;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }


    //////#Eventos/////
    private async void IniciarInterfaz(object sender, EventArgs e)//evento del unico boton para inicar la app
    {
        await this.TranslateToAsync(-Width, 0, 650);
        await Navigation.PushAsync(new FlyOutPageGi());
        TranslationX = 0;
    }

    ///////////////////
}
