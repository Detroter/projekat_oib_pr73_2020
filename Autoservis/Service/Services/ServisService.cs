using Autoservis.Model;
using Autoservis.Service.Interface;

namespace Autoservis.Service.Services
{
    public class ServisService
    {
        private readonly IVehicleRepository ivehicleRepository;
        private readonly IPaymentType ipaymentType;
        private readonly IReceiptRepository ireceiptRepository;
        private readonly ILoggerService iloggerService;


        public ServisService(IVehicleRepository ivehicleRepository, IPaymentType ipaymentType, IReceiptRepository ireceiptRepository, ILoggerService iloggerService )
        {
            this.ivehicleRepository = ivehicleRepository;
            this.ipaymentType = ipaymentType;
            this.ireceiptRepository = ireceiptRepository;
            this.iloggerService = iloggerService;
        }

        public void FinishServis(Guid vehicleGuid, string mechanicName)
        {
            var vehicle = ivehicleRepository.GetAll().FirstOrDefault(v => v.Id == vehicleGuid);
            vehicle.Serviced = true;

            ivehicleRepository.Update(vehicle);
            iloggerService.Log(ErrorType.INFO, $"Servis vozila {vehicle.Registration} je zavrsen od strane mehanicara {mechanicName}.");

            var total = ipaymentType.Total(vehicle.Price);

            var receipt = new Receipt(mechanicName, DateTime.Now, total);

            ireceiptRepository.Add(receipt);
            iloggerService.Log(ErrorType.INFO, $"Dodat je novi racun za servis vozila {vehicle.Registration}.");


        }
    }
}
