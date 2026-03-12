using Autoservis.Model;
namespace Autoservis.Service.Interface
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User GetUserByNickname(string nickname);
    }
}

