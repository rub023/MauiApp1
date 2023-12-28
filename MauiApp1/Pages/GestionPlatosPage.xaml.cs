using MauiApp1.ConexionDatos;
using MauiApp1.Models;
using System.Diagnostics;

namespace MauiApp1.Pages;
[QueryProperty(nameof(plato), "Plato")]
public partial class GestionPlatosPage : ContentPage
{
    private readonly IRestConexionDatos restConexionDatos;
    private Plato _plato;
    private bool _esNuevo;

    public Plato plato
    {
        get => _plato;
        set
        {
            _esNuevo = esNuevo(value);
            _plato = value;
            OnPropertyChanged();
        }
    }
    public GestionPlatosPage(IRestConexionDatos restConexionDatos)
    {
        InitializeComponent();
        this.restConexionDatos = restConexionDatos;
        BindingContext = this;
    }

    bool esNuevo(Plato plato)
    {
        if (plato.Id == 0)
            return true;
        return false;
    }
    async void OnCancelarClic(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    async void OnGuardarClic(object sender, EventArgs e)
    {
        if (_esNuevo)
        {
            Debug.WriteLine("[REGISTRO] Agregando nuevo plato.");
            await restConexionDatos.AddPlatoAsync(plato);
        }
        else
        {
            Debug.WriteLine("[REGISTRO] Editando un plato.");
            await restConexionDatos.UpdatePlatoAsync(plato);
        }
        await Shell.Current.GoToAsync("..");
    }
    async void OnEliminarClic(object sender, EventArgs e)
    {
        await restConexionDatos.DeletePlato(plato.Id);
        await Shell.Current.GoToAsync("..");
    }
}