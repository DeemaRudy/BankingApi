using BankingApp.BLL.Services.Implementation;
using BankingApp.DAL.Models;
using BankingApp.DAL.Repositories;
using BankingApp.DAL.UnitOfWork;
using Moq;

namespace BankingApp.BLL.Tests
{
    public class AccountTransactionsServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IAccountRepository> _mockAccountRepository;
        private AccountTransactionsService _service;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockAccountRepository = new Mock<IAccountRepository>();

            _mockUnitOfWork.Setup(u => u.Accounts).Returns(_mockAccountRepository.Object);

            _service = new AccountTransactionsService(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task DepositFundsIntoAccount_ShouldReturnError_WhenDepositFundsIsNegative()
        {
            // Arrange
            var accountId = 1;
            decimal depositFunds = -100;

            // Act
            var result = await _service.DepositFundsIntoAccount(accountId, depositFunds);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("Deposit funds cannot be less than zero", result.ErrorMessage);
        }

        [Test]
        public async Task DepositFundsIntoAccount_ShouldReturnError_WhenAccountDoesNotExist()
        {
            // Arrange
            var accountId = 1;
            decimal depositFunds = 100;

            _mockAccountRepository.Setup(a => a.GetByAccountIdAsync(accountId)).ReturnsAsync((Account)null);

            // Act
            var result = await _service.DepositFundsIntoAccount(accountId, depositFunds);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Account with id={accountId} doesn't exists", result.ErrorMessage);
        }

        [Test]
        public async Task DepositFundsIntoAccount_ShouldReturnSuccess_WhenAccountExists()
        {
            // Arrange
            var accountId = 1;
            decimal depositFunds = 100;

            var account = new Account { AccountId = accountId, Balance = 0 };
            _mockAccountRepository.Setup(a => a.GetByAccountIdAsync(accountId)).ReturnsAsync(account);
            _mockAccountRepository.Setup(a => a.AddAmountIntoAccount(account.AccountId, depositFunds)).ReturnsAsync(true);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.DepositFundsIntoAccount(accountId, depositFunds);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(string.Empty, result.ErrorMessage);
        }
        [Test]
        public async Task WithdrawFundsFromAccount_ShouldReturnError_WhenWithdrawFundsIsNegative()
        {
            // Arrange
            var accountId = 1;
            decimal withdrawFunds = -100;

            // Act
            var result = await _service.WithdrawFundsFromAccount(accountId, withdrawFunds);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("Withdraw funds cannot be negative", result.ErrorMessage);
        }

        [Test]
        public async Task WithdrawFundsFromAccount_ShouldReturnError_WhenAccountDoesNotExist()
        {
            // Arrange
            var accountId = 1;
            decimal withdrawFunds = 100;

            _mockAccountRepository.Setup(a => a.GetByAccountIdAsync(accountId)).ReturnsAsync((Account)null);

            // Act
            var result = await _service.WithdrawFundsFromAccount(accountId, withdrawFunds);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Account with id={accountId} doesn't exists", result.ErrorMessage);
        }

        [Test]
        public async Task WithdrawFundsFromAccount_ShouldReturnError_WhenInsufficientBalance()
        {
            // Arrange
            var accountId = 1;
            decimal withdrawFunds = 200;

            var account = new Account { AccountId = accountId, Balance = 100 };
            _mockAccountRepository.Setup(a => a.GetByAccountIdAsync(accountId)).ReturnsAsync(account);

            // Act
            var result = await _service.WithdrawFundsFromAccount(accountId, withdrawFunds);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"There are not enough funds in the account with id={accountId} to complete the withdrawal", result.ErrorMessage);
        }

        [Test]
        public async Task WithdrawFundsFromAccount_ShouldReturnSuccess_WhenAccountExistsAndFundsAreSufficient()
        {
            // Arrange
            var accountId = 1;
            decimal withdrawFunds = 100;

            var account = new Account { AccountId = accountId, Balance = 200 };
            _mockAccountRepository.Setup(a => a.GetByAccountIdAsync(accountId)).ReturnsAsync(account);
            _mockAccountRepository.Setup(a => a.AddAmountIntoAccount(account.AccountId, -withdrawFunds)).ReturnsAsync(true);

            // Act
            var result = await _service.WithdrawFundsFromAccount(accountId, withdrawFunds);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(string.Empty, result.ErrorMessage);
        }
        [Test]
        public async Task TransferFunds_ShouldReturnError_WhenTransferAmountIsNegativeOrZero()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            decimal transferAmount = -100;

            // Act
            var result = await _service.TransferFunds(fromAccountId, toAccountId, transferAmount);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("Transfer amount cannot be negative or zero", result.ErrorMessage);
        }

        [Test]
        public async Task TransferFunds_ShouldReturnError_WhenFromAccountDoesNotExist()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            decimal transferAmount = 100;

            _mockAccountRepository.Setup(a => a.GetByAccountIdAsync(fromAccountId)).ReturnsAsync((Account)null);

            // Act
            var result = await _service.TransferFunds(fromAccountId, toAccountId, transferAmount);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Account with id={fromAccountId} from which funds are to be transferred doesn't exist", result.ErrorMessage);
        }

        [Test]
        public async Task TransferFunds_ShouldReturnError_WhenToAccountDoesNotExist()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            decimal transferAmount = 100;

            var fromAccount = new Account { AccountId = fromAccountId, Balance = 200 };
            _mockAccountRepository.Setup(a => a.GetByAccountIdAsync(fromAccountId)).ReturnsAsync(fromAccount);
            _mockAccountRepository.Setup(a => a.GetByAccountIdAsync(toAccountId)).ReturnsAsync((Account)null);

            // Act
            var result = await _service.TransferFunds(fromAccountId, toAccountId, transferAmount);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Account with id={toAccountId} doesn't exists", result.ErrorMessage);
        }

        [Test]
        public async Task TransferFunds_ShouldReturnError_WhenInsufficientFunds()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            decimal transferAmount = 300;

            var fromAccount = new Account { AccountId = fromAccountId, Balance = 200 };
            var toAccount = new Account { AccountId = toAccountId, Balance = 0 };
            _mockAccountRepository.Setup(a => a.GetByAccountIdAsync(fromAccountId)).ReturnsAsync(fromAccount);
            _mockAccountRepository.Setup(a => a.GetByAccountIdAsync(toAccountId)).ReturnsAsync(toAccount);

            // Act
            var result = await _service.TransferFunds(fromAccountId, toAccountId, transferAmount);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"There are not enough funds in the account with id={fromAccountId} to complete the transfer", result.ErrorMessage);
        }

        [Test]
        public async Task TransferFunds_ShouldReturnSuccess_WhenAllConditionsAreMet()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            decimal transferAmount = 100;

            var fromAccount = new Account { AccountId = fromAccountId, Balance = 200 };
            var toAccount = new Account { AccountId = toAccountId, Balance = 0 };

            _mockAccountRepository.Setup(a => a.GetByAccountIdAsync(fromAccountId)).ReturnsAsync(fromAccount);
            _mockAccountRepository.Setup(a => a.GetByAccountIdAsync(toAccountId)).ReturnsAsync(toAccount);
            _mockAccountRepository.Setup(a => a.AddAmountIntoAccount(fromAccountId, -transferAmount)).ReturnsAsync(true);
            _mockAccountRepository.Setup(a => a.AddAmountIntoAccount(toAccountId, transferAmount)).ReturnsAsync(true);

            // Act
            var result = await _service.TransferFunds(fromAccountId, toAccountId, transferAmount);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(string.Empty, result.ErrorMessage);
        }
    }
}
