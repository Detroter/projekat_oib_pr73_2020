using Autoservis.Model;
using Autoservis.Service.Interface;
namespace Autoservis.Service.Interface
{
    public class LoggerService : ILoggerService
    {
        public void Log(ErrorType errorType, string message)
        {
            var log = $"[{DateTime.Now}] {errorType}: {message}";
            
            File.AppendAllText("log.txt",log + Environment.NewLine);
        }
    }
}