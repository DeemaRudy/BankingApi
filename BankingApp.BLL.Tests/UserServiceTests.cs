using BankingApp.BLL.Services.Implementation;
using BankingApp.DAL.Models;
using BankingApp.DAL.Repositories;
using BankingApp.DAL.UnitOfWork;
using Moq;

namespace BankingApp.BLL.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private UserService _userService;
        private Mock<IUserRepository> _mockUserRepository;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _userService = new UserService(_mockUnitOfWork.Object);
            _mockUserRepository = new Mock<IUserRepository>();
        }

        [Test]
        public async Task RegisterUserAsync_ShouldReturnUserDto_WhenUserDoesNotExist()
        {
            // Arrange
            var userName = "newUser";
            _mockUnitOfWork.SetupSequence(uow => uow.Users.GetUserByNameAsync(userName))
                          .ReturnsAsync((User)null)
                          .ReturnsAsync(new User { Name = userName });
            _userService = new UserService(_mockUnitOfWork.Object);

            // Act
            var result = await _userService.RegisterUserAsync(userName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userName, result.User.Name);
            Assert.IsFalse(result.HasError);
        }

        [Test]
        public async Task RegisterUserAsync_ShouldReturnError_WhenUserAlreadyExists()
        {
            // Arrange
            var userName = "existingUser";

            _mockUnitOfWork.Setup(uow => uow.Users.GetUserByNameAsync(userName))
                           .ReturnsAsync(new User { Name = userName });

            // Act
            var result = await _userService.RegisterUserAsync(userName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("The user already exists", result.ErrorMessage);
        }
    }
}
