using Autoservis.Model;
using Autoservis.Service.Interface;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Autoservis.Service.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly string filePath = "C:/Users/HP/Documents/GitHub/projekat_oib_pr73_2020/Autoservis/DB/vehicles.json";
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        public List<Vehicle> GetAll()
        {
            if (!File.Exists(filePath))
            {
                return new List<Vehicle>();
            }
            var json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<Vehicle>>(json, options) ?? new List<Vehicle>();
        }
        public List<Vehicle> GetUnserviced()
        {
            var vehicles = GetAll();
            return vehicles.Where(v => !v.Serviced).ToList();
        }
        public void Add(Vehicle vehicle)
        {
            var vehicles = GetAll();

            vehicles.Add(vehicle);


            var json = JsonSerializer.Serialize(vehicles, options);
            File.WriteAllText(filePath, json);
        }
        public void Update(Vehicle vehicle)
        {
            var vehicles = GetAll();
            
            int id = vehicles.FindIndex(v => v.Id == vehicle.Id);
            if (id >= 0)
            {
                vehicles[id] = vehicle;
            }
                var json = JsonSerializer.Serialize(vehicles, options);
                File.WriteAllText(filePath, json);
        }

    }

}