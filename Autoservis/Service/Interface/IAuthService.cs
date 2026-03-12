using Autoservis.Model;

namespace Autoservis.Service.Interface
{
    public interface IAuthService
    {
        bool Login(string nickname, string password);
    }
}
