using NUnit.Framework;
using Moq;
using Autoservis.Model;
using Autoservis.Service.Repository;
using Autoservis.Service.Services;
using Autoservis.Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Autoservis.Tests
{
    [TestFixture]
    public class VehicleServiceTests
    {
        private Mock<IVehicleRepository> _mockRepo;
        private Mock<ILoggerService> _mockLogger;
        private VehicleService _vehicleService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IVehicleRepository>();
            _mockLogger = new Mock<ILoggerService>();
            _vehicleService = new VehicleService(_mockRepo.Object, _mockLogger.Object);
        }

        [Test]
        public void GetAllUnservicedVehicles()
        {
            var vehicles = new List<Vehicle>
            {
                new Vehicle("ST131AN", "Golf", "Volkswagen", TypeOfVehicle.Putnicko, 1000) { Serviced = true },
                new Vehicle("PB111SX", "FAP3000", "FAP", TypeOfVehicle.Teretno, 35000) { Serviced = false },
                new Vehicle("NI887KT", "Subaru", "Kawasaki", TypeOfVehicle.Motocikl, 7500){ Serviced = false }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(vehicles);

            var result = _vehicleService.GetUnserviced();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Registration, Is.EqualTo("PB111SX"));
            Assert.That(result[1].Mark, Is.EqualTo("Kawasaki"));
        }

        [Test]
        public void AddWhenUnderLimit()
        {
            var vehicles = new List<Vehicle>();
            _mockRepo.Setup(r => r.GetAll()).Returns(vehicles);

            var newVehicle = new Vehicle("ST131AN", "Golf", "Volkswagen", TypeOfVehicle.Putnicko, 1000);

            _vehicleService.Add(newVehicle);

            _mockRepo.Verify(r => r.Add(newVehicle), Times.Once);
        }

        [Test]
        public void AddWhenMoreThen10()
        {
            var vehicles = Enumerable.Range(1, 10)
                .Select(i => new Vehicle($"R{i}", "Model", "Mark", TypeOfVehicle.Putnicko, 100))
                .ToList();

            _mockRepo.Setup(r => r.GetAll()).Returns(vehicles);

            var newVehicle = new Vehicle("PB111SX", "FAP3000", "FAP", TypeOfVehicle.Teretno, 35000);

            Assert.That(() => _vehicleService.Add(newVehicle),
                        Throws.Exception.With.Message.EqualTo("Max broj vozila je 10. Nije moguce dodati novo vozilo."));
        }
    }
}
