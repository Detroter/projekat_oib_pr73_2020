using Autoservis.Service.Interface;
namespace Autoservis.Service.Services
{
    public class AfternoonPayment : IPaymentType
    {
        public double Total(double amount)
        {
            return amount * 1.10;
        }
    }
}