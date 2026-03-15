using Autoservis.Model;
using NUnit.Framework;

namespace Autoservis.Tests
{
    public class UserTest
    {
        [Test]
        public void UserProperties()
        {
            var user = new User
            {
                Nickname = "jovan",
                Password = "oib",
                Name = "Jovan",
                Surname = "Bogdanovic",
                Role = Role.Mehanicar
            };

            
            Assert.That(user.Nickname, Is.EqualTo("jovan"));
            Assert.That(user.Password, Is.EqualTo("oib"));
            Assert.That(user.Name, Is.EqualTo("Jovan"));
            Assert.That(user.Surname, Is.EqualTo("Bogdanovic"));
            Assert.That(user.Role, Is.EqualTo(Role.Mehanicar));
        }
    }
}