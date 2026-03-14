using Autoservis.Model;
using Autoservis.Service.Interface;
namespace Autoservis.Service.Interface
{
    public interface ILoggerService
    {
        void Log(ErrorType errorType, string message);
    }
}