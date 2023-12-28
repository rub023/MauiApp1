using MauiApp1.ConexionDatos;
using MauiApp1.Models;
using MauiApp1.Pages;
using System.Diagnostics;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private readonly IRestConexionDatos conexionDatos;
        //int count = 0;

        public MainPage(IRestConexionDatos restConexionDatos)
        {
            InitializeComponent();
            this.conexionDatos = restConexionDatos;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            coleccionPlatosView.ItemsSource = await conexionDatos.GetPlatosAsync();
        }



        //Evento add
        async void OnAddPlatoClic(object sender, EventArgs e)
        {
            Debug.WriteLine("[EVENTO] Botón AddPlato clickeado");
            var param = new Dictionary<string, object> {
                {nameof(Plato), new Plato()}
            };
            await Shell.Current.GoToAsync(nameof(GestionPlatosPage), param);
        }
        // Evento clic sobre plato
        async void OnPlatoCambiadoClic(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("[EVENTO] Botón PlatoCambiado clickeado");
            var param = new Dictionary<string, object> {
                {nameof(Plato), e.CurrentSelection.FirstOrDefault() as Plato}
            };
            await Shell.Current.GoToAsync(nameof(GestionPlatosPage), param);
        }
    }

}
