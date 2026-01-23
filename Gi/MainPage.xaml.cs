//using HealthKit;
namespace Gi;

public partial class MainPage : ContentPage
{
    FlyOutPageGi? _flyoutPrecargado;

    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //Precarga tel tabbedPage detras de la primera pantalla (una sola vez)
        //ver q tanto te conviene esto ahora con Gi terminado
        if (_flyoutPrecargado != null)
            return;

        Task.Run(() =>
        {
            _flyoutPrecargado = new FlyOutPageGi();

            //Forzar inicialización interna si hiciera falta
            _ = _flyoutPrecargado.Title;
        });
    }

    //////#Eventos/////
    private async void IniciarInterfaz(object sender, EventArgs e)//evento del unico boton para inicar la app
    {
        //animación de salida
        await this.TranslateToAsync(-Width, 0, 650);

        //navegación instantánea si ya está precargado (mas natural)
        await Navigation.PushAsync(_flyoutPrecargado ?? new FlyOutPageGi());

        TranslationX = 0;
    }
}