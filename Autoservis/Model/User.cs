using System.Text.Json.Serialization;
namespace Autoservis.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? TimeoutEndTime { get; set; }

        public User() { }
        public User(string nickname, string password, string name, string surname, Role role)
        {
            Id = Guid.NewGuid();
            Nickname = nickname;
            Password = password;
            Name = name;
            Surname = surname;
            Role = role;
            FailedLoginAttempts = 0;
            TimeoutEndTime = null;
        }
    }
}
