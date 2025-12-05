namespace Gi;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

                      //usamos NavigationPage para implementar
                      //una navegacion de pantallas
        //var navPage = new NavigationPage(new MainPage());

        //color de fondo
        //navPage.BarBackground = Color.FromArgb("#3b7170");

        //el XAML de MainPage es la pagina inicial
        //MainPage = navPage;
    }
    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Usamos NavigationPage para implementar
        // una navegación de pantallas
        var navPage = new NavigationPage(new MainPage());

        // Color de fondo
        navPage.BarBackground = Color.FromArgb("#3b7170");

        // El XAML de MainPage es la página inicial
        return new Window(navPage);
    }
}