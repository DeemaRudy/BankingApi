using BankingApp.BLL.Services.Implementation;
using BankingApp.DAL.Models;
using BankingApp.DAL.Repositories;
using BankingApp.DAL.UnitOfWork;
using Moq;

namespace BankingApp.BLL.Tests
{
    public class AccountManagementServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IAccountRepository> _mockAccountRepository;
        private AccountManagementService _service;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockAccountRepository = new Mock<IAccountRepository>();

            _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUserRepository.Object);
            _mockUnitOfWork.Setup(u => u.Accounts).Returns(_mockAccountRepository.Object);

            _service = new AccountManagementService(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task CreateNewAccountWithInitialBalanceAsync_ShouldReturnError_WhenBalanceIsLessThanZero()
        {
            // Arrange
            var userId = 1;
            decimal initialBalance = -100;

            // Act
            var result = await _service.CreateNewAccountWithInitialBalanceAsync(userId, initialBalance);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("Balance cannot be less than zero", result.ErrorMessage);
        }

        [Test]
        public async Task CreateNewAccountWithInitialBalanceAsync_ShouldReturnError_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            decimal initialBalance = 100;

            _mockUserRepository.Setup(u => u.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _service.CreateNewAccountWithInitialBalanceAsync(userId, initialBalance);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"The user with id={userId} doesn't exists", result.ErrorMessage);
        }

        [Test]
        public async Task CreateNewAccountWithInitialBalanceAsync_ShouldReturnSuccess_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            decimal initialBalance = 100;
            var user = new User { UserId = userId };
            _mockUserRepository.Setup(u => u.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _service.CreateNewAccountWithInitialBalanceAsync(userId, initialBalance);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.IsEmpty(result.ErrorMessage);
        }

        [Test]
        public async Task GetAccountDetailsAsync_ShouldReturnError_WhenAccountDoesNotExist()
        {
            // Arrange
            var accountId = 1;
            _mockAccountRepository.Setup(a => a.GetByAccountIdAsync(accountId)).ReturnsAsync((Account)null);

            // Act
            var result = await _service.GetAccountDetailsAsync(accountId);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Account with id={accountId} doesn't exists", result.ErrorMessage);
        }

        [Test]
        public async Task GetAccountDetailsAsync_ShouldReturnAccountDetails_WhenAccountExists()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { AccountId = accountId, Balance = 100 };
            _mockAccountRepository.Setup(a => a.GetByAccountIdAsync(accountId)).ReturnsAsync(account);

            // Act
            var result = await _service.GetAccountDetailsAsync(accountId);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(accountId, result.Account.AccountId);
            Assert.AreEqual(account.Balance, result.Account.Balance);
        }

        [Test]
        public async Task GetAllUserAccountsAsync_ShouldReturnError_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(u => u.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _service.GetAllUserAccountsAsync(userId);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"User with id={userId} doesn't exists", result.ErrorMessage);
        }

        [Test]
        public async Task GetAllUserAccountsAsync_ShouldReturnError_WhenUserHasNoAccounts()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId };
            _mockUserRepository.Setup(u => u.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _mockAccountRepository.Setup(a => a.GetAllUserAccountsAsync(userId)).ReturnsAsync(new List<Account>());

            // Act
            var result = await _service.GetAllUserAccountsAsync(userId);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"User with id={userId} doesn't have any accounts yet", result.ErrorMessage);
        }

        [Test]
        public async Task GetAllUserAccountsAsync_ShouldReturnAccounts_WhenUserHasAccounts()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId };
            var accounts = new List<Account>
            {
                new Account { AccountId = 1, UserId = userId, Balance = 100 },
                new Account { AccountId = 2, UserId = userId, Balance = 200 }
            };
            _mockUserRepository.Setup(u => u.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _mockAccountRepository.Setup(a => a.GetAllUserAccountsAsync(userId)).ReturnsAsync(accounts);

            // Act
            var result = await _service.GetAllUserAccountsAsync(userId);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(2, result.Accounts.Count());
        }
    }
}
