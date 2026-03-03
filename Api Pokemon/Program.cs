using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== POKEAPI CONSOLE APP ===");
        Console.WriteLine("1 - Listar 10 Pokémon");
        Console.WriteLine("2 - Buscar Pokémon por nombre");
        Console.Write("Seleccione una opción: ");
        string opcion = Console.ReadLine();

        if (opcion == "1")
        {
            await ListarPokemon();
        }
        else if (opcion == "2")
        {
            Console.Write("Ingrese el nombre del Pokémon: ");
            string nombre = Console.ReadLine().ToLower();
            await BuscarPokemon(nombre);
        }
        else
        {
            Console.WriteLine("Opción no válida.");
        }
    }

    static async Task ListarPokemon()
    {
        using (HttpClient client = new HttpClient())
        {
            string url = "https://pokeapi.co/api/v2/pokemon?limit=10";
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    var results = doc.RootElement.GetProperty("results");

                    Console.WriteLine("\nLista de Pokémon:");
                    foreach (var pokemon in results.EnumerateArray())
                    {
                        Console.WriteLine("- " + pokemon.GetProperty("name").GetString());
                    }
                }
            }
            else
            {
                Console.WriteLine("Error al conectar con la API.");
            }
        }
    }

    static async Task BuscarPokemon(string nombre)
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"https://pokeapi.co/api/v2/pokemon/{nombre}";
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    Console.WriteLine("\nInformación del Pokémon:");
                    Console.WriteLine("Nombre: " + doc.RootElement.GetProperty("name").GetString());
                    Console.WriteLine("Altura: " + doc.RootElement.GetProperty("height").GetInt32());
                    Console.WriteLine("Peso: " + doc.RootElement.GetProperty("weight").GetInt32());
                }
            }
            else
            {
                Console.WriteLine("Pokémon no encontrado.");
            }
        }
    }
}