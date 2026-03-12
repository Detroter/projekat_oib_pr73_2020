using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Autoservis.Model;
using Autoservis.Service.Interface;

namespace Autoservis.Service.Logic
{
    public class UserRepository : IUserRepository
    {
        private readonly string filePath = "C:/Users/HP/Documents/GitHub/projekat_oib_pr73_2020/Autoservis/DB/users.json";
        public List<User> GetAllUsers()
        {
            if (!File.Exists(filePath))
            {
                return new List<User>();
            }

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<User>>(json,options) ?? new List<User>();
        }
        public User GetUserByNickname(string nickname)
        {
            var users = GetAllUsers();
            if(users.FirstOrDefault(u => u.Nickname.Equals(nickname)) == null)
            {
                throw new InvalidOperationException("Ne postoji korisnik sa tim nicknamom.");
            }else{
            return users.FirstOrDefault(u => u.Nickname.Equals(nickname));
            }
        }
    }
}