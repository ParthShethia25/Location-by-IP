using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

// first type dotnet build to check for errors 
// then type dotnet run to run the code
namespace Geolocate
{
    public class Data
    {
        public string? City { get; set; } = "DefaultCity";
        public string? Region {get; set; } = "DefaultRegion";
        public string? Country { get; set; } = "DefaultCountry";
        public string? Loc { get; set; } = "DefaultLoc";
        public string? Postal { get; set; } = "DefaultPostal";
        public string? Org { get; set; } = "DefaultOrg";
    }

    public class Program
    {
        public static void Main(string[] args) // Corrected to be static and non-async
        {
            // Ensure the async method is called properly
            MainAsync(args).GetAwaiter().GetResult();
        }

        public static async Task MainAsync(string[] args) // Async method that does the work
        {
            Console.Title = "Geolocator";
            Console.Write("Enter IP Address: ");
            string? ip = Console.ReadLine(); 

            string url = $"https://ipinfo.io/{ip}/json";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine("[+] Request Successfully Made");

                    string? responseData = await response.Content.ReadAsStringAsync();

                    Data? ipInfo = JsonConvert.DeserializeObject<Data>(responseData);

                    if (ipInfo != null) // Check if deserialization succeeded
                    {
                        Console.Clear();

                        // Display information with null checks and defaults
                        Console.WriteLine($"Country: {ipInfo.Country ?? "Unknown"}");
                        Console.WriteLine($"City: {ipInfo.City ?? "Not specified"}");
                        Console.WriteLine($"Coordinates: {ipInfo.Loc ?? "Unknown"}");
                        Console.WriteLine($"Postal Code: {ipInfo.Postal ?? "Unknown"}");
                        Console.WriteLine($"Region: {ipInfo.Region ?? "Unknown"}");
                        Console.WriteLine($"ASN: {ipInfo.Org ?? "Unknown"}");

                        string loc = ipInfo.Loc ?? "0,0";
                        string[] coords = loc.Split(',');

                        // Ensure that coordinates are valid before using them
                        if (coords.Length == 2)
                        {
                            Console.WriteLine($"Google Maps: https://www.google.com/maps/?q={coords[0]},{coords[1]}");
                        }
                        else
                        {
                            Console.WriteLine("Coordinates format is incorrect.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Could not parse location data.");
                    }
                }
                catch (HttpRequestException ex) // Handle HTTP errors
                {
                    Console.WriteLine($"HTTP Error: {ex.Message}");
                }
                catch (Exception ex) // Handle other exceptions
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}


