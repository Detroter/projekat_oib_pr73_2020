using NUnit.Framework;
using Moq;
using Autoservis.Service.Services;
using Autoservis.Service.Interface;
using Autoservis.Model;
using System.Collections.Generic;
using System.Data.Common;

namespace Autoservis.Tests
{
    public class ServisServiceTest
    {
        private Mock<IVehicleRepository> _mockVehicleRepo;
        private Mock<IReceiptRepository> _mockReceiptRepo;
        private Mock<ILoggerService> _mockLogger;

        private Mock<IPaymentType> _mockPayment;
        private ServisService _servisService;

        [SetUp]
        public void Setup()
        {
            _mockVehicleRepo = new Mock<IVehicleRepository>();
            _mockReceiptRepo = new Mock<IReceiptRepository>();
            _mockPayment = new Mock<IPaymentType>();
            _mockLogger = new Mock<ILoggerService>();
            _servisService = new ServisService(_mockVehicleRepo.Object, _mockPayment.Object, _mockReceiptRepo.Object, _mockLogger.Object);
        }

        [Test]
        public void FinishServis()
        {
            var vehicle = new Vehicle("ST178AN", "Golf", "Volkswagen", TypeOfVehicle.Putnicko,3000){ Id = Guid.Parse("c5bdda4b-899c-437e-82cf-f86d2f40b473")};
            _mockVehicleRepo.Setup(r => r.GetAll()).Returns(new List<Vehicle> { vehicle });

            _servisService.FinishServis(Guid.Parse("c5bdda4b-899c-437e-82cf-f86d2f40b473"), "Marko");

            _mockVehicleRepo.Verify(r => r.Update(It.Is<Vehicle>(v => v.Serviced)), Times.Once);
            _mockReceiptRepo.Verify(r => r.Add(It.IsAny<Receipt>()), Times.Once);
        }
    }
}
