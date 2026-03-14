using System;
using System.Text.Json;
using Autoservis.Model;
using Autoservis.Service.Interface;

namespace Autoservis.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly ILoggerService loggerService;
        private readonly int maxFailedLogins;
        private readonly TimeSpan durationTimeout;

        public AuthService(IUserRepository userRepository, ILoggerService loggerService)
        {
            this.userRepository = userRepository;
            this.loggerService = loggerService;

            maxFailedLogins = 3;
            durationTimeout = TimeSpan.FromMinutes(5);
        }

        public bool Login(string nickname, string password)
        {
            var user = userRepository.GetUserByNickname(nickname);
            if (user == null)
                {
                loggerService.Log(ErrorType.WARNING, $"Neuspješna prijava: Korisnik '{nickname}' ne postoji.");
                return false;
                }

            if (user.TimeoutEndTime != null && user.TimeoutEndTime.Value > DateTime.UtcNow)
            {
                var remainingTime = user.TimeoutEndTime.Value - DateTime.UtcNow;
                loggerService.Log(ErrorType.WARNING, $"Korisnik je u timeoutu jos {remainingTime:mm\\:ss}.");
                throw new InvalidOperationException($"Korisnik je u timeoutu jos {remainingTime:mm\\:ss}.");

            }

            if (user.Password != password)
            {
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= maxFailedLogins)
                {
                    loggerService.Log(ErrorType.WARNING, $"Korisnik '{nickname}' je dostigao maksimalan broj neuspešnih prijava.");
                    user.TimeoutEndTime = DateTime.UtcNow.Add(durationTimeout);
                    user.FailedLoginAttempts = 0;
                }

                userRepository.UpdateUser(user);
                return false;
            }

            user.FailedLoginAttempts = 0;
            user.TimeoutEndTime = null;

            userRepository.UpdateUser(user);
            loggerService.Log(ErrorType.INFO, $"Korisnik '{nickname}' je uspešno prijavljen.");

            return true;
        }
    }
}
