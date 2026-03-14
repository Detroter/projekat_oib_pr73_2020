using System;
using Autoservis.Model;

namespace Autoservis.Service.Interface
{
    public interface IVehicleRepository
    {
        List<Vehicle> GetAll();
        List<Vehicle> GetUnserviced();
        void Add(Vehicle vehicle);
        void Update(Vehicle vehicle);
    }
}