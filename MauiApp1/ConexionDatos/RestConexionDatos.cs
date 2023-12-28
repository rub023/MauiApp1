using MauiApp1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiApp1.ConexionDatos
{
    public class RestConexionDatos : IRestConexionDatos
    {
        public readonly HttpClient HttpClient;
        private readonly string dominio;
        private readonly string url;
        private readonly JsonSerializerOptions opcionesJson;
        public RestConexionDatos(HttpClient httpClient)
        {
            HttpClient = httpClient;//new HttpClient();
            dominio = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:7210" : "http://localhost:7210";
            url = $"{dominio}/api";
            opcionesJson = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        public async Task AddPlatoAsync(Plato plato)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("[RED] Sin acceso a la red.");
                return;
            }
            try
            {
                //Serializamos el objeto plato
                string platoSer = JsonSerializer.Serialize<Plato>(plato, opcionesJson);
                StringContent contenido = new StringContent(platoSer, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await HttpClient.PostAsync($"{url}/plato", contenido);
                if (respuesta.IsSuccessStatusCode)
                    Debug.WriteLine("[SERVER] Se registró correctamente.");
                else
                    Debug.WriteLine("[SERVER] Sin respuesta HTTP satisfactoria (2XX).");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] {ex.Message}");
            }
            return;
        }

        public async Task DeletePlato(int id)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("[RED] Sin acceso a la red.");
                return;
            }
            try
            {
                HttpResponseMessage respuesta = await HttpClient.DeleteAsync($"{url}/plato/{id}");
                if (respuesta.IsSuccessStatusCode)
                    Debug.WriteLine("[SERVER] Se modificó correctamente.");
                else
                    Debug.WriteLine("[SERVER] Sin respuesta HTTP satisfactoria (2XX).");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] {ex.Message}");
            }
            return;
        }

        public async Task<List<Plato>> GetPlatosAsync()
        {
            List<Plato> platos = new List<Plato>();
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("[RED] Sin acceso a la red.");
                return platos;
            }
            try
            {
                HttpResponseMessage response = await HttpClient.GetAsync($"{url}/plato");
                if (response.IsSuccessStatusCode)
                {
                    //Deserializamos
                    var contenido = await response.Content.ReadAsStringAsync();
                    platos = JsonSerializer.Deserialize<List<Plato>>(contenido, opcionesJson);
                }
                else
                {
                    Debug.WriteLine("[SERVER] Sin respuesta HTTP satisfactoria (2XX).");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[ERROR] {e.Message}");
            }
            return platos;
        }

        public async Task UpdatePlatoAsync(Plato plato)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("[RED] Sin acceso a la red.");
                return;
            }
            try
            {
                //Serializamos el objeto plato
                string platoSer = JsonSerializer.Serialize<Plato>(plato, opcionesJson);
                StringContent contenido = new StringContent(platoSer, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await HttpClient.PutAsync($"{url}/plato/{plato.Id}", contenido);
                if (respuesta.IsSuccessStatusCode)
                    Debug.WriteLine("[SERVER] Se modificó correctamente.");
                else
                    Debug.WriteLine("[SERVER] Sin respuesta HTTP satisfactoria (2XX).");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] {ex.Message}");
            }
            return;
        }
    }
}
