using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Autoservis.Model;
using Autoservis.Service.Interface;

namespace Autoservis.Service.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string filePath = "C:/Users/HP/Documents/GitHub/projekat_oib_pr73_2020/Autoservis/DB/users.json";
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            WriteIndented = true
        };
        public List<User> GetAllUsers()
        {
            if (!File.Exists(filePath))
            {
                return new List<User>();
            }

            var json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<User>>(json, options) ?? new List<User>();
        }
        public User GetUserByNickname(string nickname)
        {
            var users = GetAllUsers();
            return users.FirstOrDefault(u => u.Nickname.Equals(nickname));
        }
        public void UpdateUser(User user)
        {
            var users = GetAllUsers();

            int id = users.FindIndex(u => u.Nickname == user.Nickname);
            if (id >= 0)
            {
                users[id] = user;
            }
            var json = JsonSerializer.Serialize(users, options);
            File.WriteAllText(filePath, json);
        }
    }
}