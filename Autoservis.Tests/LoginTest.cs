using NUnit.Framework;
using Autoservis.Model;
using Moq;
using Autoservis.Service.Interface;
using Autoservis.Service.Services;
using System.Reflection;

namespace Autoservis.Tests
{
    [TestFixture]
    public class LoginTest
    {
        private Mock<IUserRepository> _mockRepo;
        private Mock<ILoggerService> _mockLogger;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILoggerService>();

            _authService = new AuthService(_mockRepo.Object,_mockLogger.Object);
        }

        [Test]
        public void LoginCorrectPassword()
        {
            var name = "noNameNeeded";
            var surname = "noSurnameNeeded";
            var nickname = "testtest";
            var password = "correctpassword";
            var user = new User {Name = name, Surname = surname, Nickname = nickname, Password = "correctpassword" };
            _mockRepo.Setup(ur => ur.GetUserByNickname(nickname)).Returns(user);

            var result = _authService.Login(nickname, password);

            Assert.That(result,Is.True);
        }

        [Test]
        public void LoginWrongPassword()
        {
            var name = "noNameNeeded";
            var surname = "noSurnameNeeded";
            var nickname = "testtest";
            var password = "lapsuslingua";
            var user = new User {Name = name, Surname = surname, Nickname = nickname, Password = "password123" };
            _mockRepo.Setup(ur => ur.GetUserByNickname(nickname)).Returns(user);

            var result = _authService.Login(nickname, password);

            Assert.That(result,Is.False);
        }

        [Test]
        public void NoUserExistingLogin()
        {
            var nickname = "noNickNeeded";
            var password = "correctpassword";
            _mockRepo.Setup(ur => ur.GetUserByNickname(nickname)).Returns((User)null);

            var result = _authService.Login(nickname, password);

            Assert.That(result,Is.False);
        }
    }
}