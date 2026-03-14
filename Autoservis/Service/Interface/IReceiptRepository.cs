using Autoservis.Model;
using Autoservis.Service.Interface;
namespace Autoservis.Service.Interface
{
    public interface IReceiptRepository
    {
        List<Receipt> GetAll();
        void Add(Receipt receipt);
    }
}