using Autoservis.Service.Interface;

namespace Autoservis.Service.Services
{
    public class MorningPayment : IPaymentType
    {
        public double Total(double amount)
        {
            return amount * 0.85;
        }
    }
}