using Autoservis.Model;
using Autoservis.Service.Repository;
using Autoservis.Service.Interface;
using System.Runtime.CompilerServices;

namespace Autoservis.Service.Services
{
    public class VehicleService
    {
        private readonly IVehicleRepository ivehicleRepository;
        private readonly ILoggerService iloggerService;

        public VehicleService(IVehicleRepository vehicleRepository,ILoggerService loggerService)
        {
            ivehicleRepository = vehicleRepository;
            iloggerService = loggerService;
        }


        public List<Vehicle> GetAll()
        {
            var vehicles = ivehicleRepository.GetAll();
            return vehicles;
        }

        public List<Vehicle> GetUnserviced()
        {
            var vehicles = ivehicleRepository.GetAll().Where(v => !v.Serviced).ToList();
            return vehicles;
        }
    
        public void Add(Vehicle vehicle)
        {
            var vehicles = ivehicleRepository.GetAll();

            if (vehicles.Count >= 10)
            {
                iloggerService.Log(ErrorType.ERROR, $"Pokusaj dodavanja vozila registracije {vehicle.Registration}, ali je dostignut maksimalan broj vozila (10).");
                throw new Exception("Max broj vozila je 10. Nije moguce dodati novo vozilo.");
            }

            ivehicleRepository.Add(vehicle);
        }
        
        public void Update(Vehicle vehicle)
        {
            ivehicleRepository.Update(vehicle);
            iloggerService.Log(ErrorType.INFO, $"Vozilo registracije {vehicle.Registration} je ažurirano.");
        }
    }
}