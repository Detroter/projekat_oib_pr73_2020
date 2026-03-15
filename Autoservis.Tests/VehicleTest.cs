using NUnit.Framework;
using Autoservis.Model;

namespace Autoservis.Tests
{
    public class VehicleTest
    {
        [Test]
        public void VehicleProperties()
        {
            var vehicle = new Vehicle("UE196RU", "Golf", "Volkswagen", TypeOfVehicle.Putnicko, 1000);

            Assert.That(vehicle.Registration, Is.EqualTo("UE196RU"));
            Assert.That(vehicle.Model, Is.EqualTo("Golf"));
            Assert.That(vehicle.Mark, Is.EqualTo("Volkswagen"));
            Assert.That(vehicle.Type, Is.EqualTo(TypeOfVehicle.Putnicko));
            Assert.That(vehicle.Price, Is.EqualTo(1000));
            Assert.That(vehicle.Serviced, Is.False);
        }
    }
}
