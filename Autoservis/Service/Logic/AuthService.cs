using System;
using System.Collections.Generic;
using System.Linq;
using Autoservis.Model;
using Autoservis.Service.Interface;

public class AuthService : IAuthService
    {
        private readonly List<User> users = new List<User>();//mora naknadno da se implementira baza iz koje ce da cita a ne da pravi listu
        private readonly int maxFailedLogins;
        private readonly TimeSpan durationTimeout;
        public AuthService() : this(3, TimeSpan.FromMinutes(5)) { }

        public AuthService(int failedLogins, TimeSpan timeout)
        {
            if (failedLogins < 1) throw new ArgumentOutOfRangeException(nameof(failedLogins));
            if (timeout < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(timeout));

            maxFailedLogins = failedLogins;
            durationTimeout = timeout;
        }

        public void Register(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            if (users.Any(u => u.Nickname.Equals(user.Nickname)))
                throw new InvalidOperationException("A user with the same nickname already exists.");

            users.Add(user);
        }

        public bool Login(string nickname, string password)
        {
            var user = users.Find(u => u.Nickname.Equals(nickname));
            if (user == null)
                return false; 

            if (user.TimeoutEndTime != null && user.TimeoutEndTime.Value > DateTime.UtcNow)
            {
                throw new InvalidOperationException($"You are in a timeout until {user.TimeoutEndTime.Value:u}.");
            }

            if (user.Password != password)
            {
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= maxFailedLogins)
                {
                    user.TimeoutEndTime = DateTime.UtcNow.Add(durationTimeout);
                    user.FailedLoginAttempts = 0; 
                }

                return false;
            }

            user.FailedLoginAttempts = 0;
            user.LockoutEndTime = null;
            return true;
        }
}
