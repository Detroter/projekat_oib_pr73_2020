using Autoservis.Model;

namespace Autoservis.Service.Interface
{
    public interface IAuthService
    {
        void Register(User user);
        bool Login(string nickname, string password);
    }
}
