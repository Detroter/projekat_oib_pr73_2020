using System;
using System.Text.Json;
using System.Linq;
using Autoservis.Model;
using Autoservis.Service.Interface;

namespace Autoservis.Service.Logic
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly int maxFailedLogins;
        private readonly TimeSpan durationTimeout;

        public AuthService(IUserRepository userRepository)
        {            
            this.userRepository = userRepository;

            maxFailedLogins = 3;
            durationTimeout = TimeSpan.FromMinutes(5);
        }

        public bool Login(string nickname, string password)
        {
            var user = userRepository.GetUserByNickname(nickname);
            if (user == null)
                return false;

            if (user.TimeoutEndTime != null && user.TimeoutEndTime.Value > DateTime.UtcNow)
            {
                var remainingTime = user.TimeoutEndTime.Value - DateTime.UtcNow;
                throw new InvalidOperationException($"Korisnik je u timeoutu jos {remainingTime:mm\\:ss}.");
            }

            if (user.Password != password)
            {
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= maxFailedLogins)
                {
                    user.TimeoutEndTime = DateTime.UtcNow.Add(durationTimeout);
                    user.FailedLoginAttempts = 0;
                }

                var errUsers = userRepository.GetAllUsers();
                int id = errUsers.FindIndex(u => u.Nickname == user.Nickname);
                if (id >= 0)
                {
                    errUsers[id] = user;
                }
                File.WriteAllText("C:/Users/HP/Documents/GitHub/projekat_oib_pr73_2020/Autoservis/DB/users.json", JsonSerializer.Serialize(errUsers));

                return false;
            }

            user.FailedLoginAttempts = 0;
            user.TimeoutEndTime = null;

            var allUsers = userRepository.GetAllUsers();
            int i = allUsers.FindIndex(u => u.Nickname == user.Nickname);
                if (i >= 0)
                {
                    allUsers[i] = user;
                }
            File.WriteAllText("C:/Users/HP/Documents/GitHub/projekat_oib_pr73_2020/Autoservis/DB/users.json", JsonSerializer.Serialize(allUsers));

            return true;
        }
    }
}
