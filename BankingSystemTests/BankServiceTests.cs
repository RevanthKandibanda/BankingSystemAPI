using BankingSystemAPI.Model;
using BankingSystemAPI.Services;
using Moq;
using NUnit.Framework;
using System.Linq;

[TestFixture]
public class BankServiceTests
{
    private Mock<IBankService> mockBankService;

    [SetUp]
    public void Setup()
    {
        mockBankService = new Mock<IBankService>();
    }
    [Test]
    public void User_CanHaveMultipleAccounts()
    {
        var user = new User { UserId = 1 };
        Assert.IsEmpty(user.Accounts);
    }
    [Test]
    public void User_CanCreateAndDeleteAccounts()
    {
        var user = new User { UserId = 1 };
        var account = new Account { AccountId = 1 };

        user.Accounts.Add(account);

        Assert.AreEqual(1, user.Accounts.Count);
       
        user.Accounts.Remove(account);

        Assert.IsEmpty(user.Accounts);
    }
    
    [Test]
    public void CreateAccount_ShouldCreateAccountForUser()
    {
        // Arrange
        var user = new User { UserId = 1 };
        var account = new Account { AccountId = 1 };
        mockBankService.Setup(b => b.CreateAccount(user)).Returns(account);

        // Act
        var result = mockBankService.Object.CreateAccount(user);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(account.AccountId, result.AccountId);
    }

    [Test]
    public void Deposit_ShouldIncreaseAccountBalance()
    {
        // Arrange
        var account = new Account { AccountId = 1, Balance = 1000 };
        decimal depositAmount = 500;
        mockBankService.Setup(b => b.Deposit(account, depositAmount)).Returns(true);

        // Act
        var result = mockBankService.Object.Deposit(account, depositAmount);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Withdraw_ShouldDecreaseAccountBalance()
    {
        // Arrange
        var account = new Account { AccountId = 1, Balance = 1000 };
        decimal withdrawalAmount = 500;
        mockBankService.Setup(b => b.Withdraw(account, withdrawalAmount)).Returns(true);

        // Act
        var result = mockBankService.Object.Withdraw(account, withdrawalAmount);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Withdraw_ShouldNotAllowOver90PercentWithdrawal()
    {
        // Arrange
        var account = new Account { AccountId = 1, Balance = 1000 };
        decimal withdrawalAmount = 1000 * 0.91m;
        mockBankService.Setup(b => b.Withdraw(account, withdrawalAmount)).Returns(false);

        // Act
        var result = mockBankService.Object.Withdraw(account, withdrawalAmount);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(1000, account.Balance); // Balance should not change
    }

    [Test]
    public void Withdraw_ShouldNotAllowBalanceBelowMinimum()
    {
        // Arrange
        var account = new Account { AccountId = 1, Balance = 100 };
        decimal withdrawalAmount = 1;
        mockBankService.Setup(b => b.Withdraw(account, withdrawalAmount)).Returns(false);

        // Act
        var result = mockBankService.Object.Withdraw(account, withdrawalAmount);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(100, account.Balance); // Balance should not change
    }

    [Test]
    public void Deposit_ShouldNotAllowDepositOver10000()
    {
        // Arrange
        var account = new Account { AccountId = 1, Balance = 0 };
        decimal depositAmount = 10001;
        mockBankService.Setup(b => b.Deposit(account, depositAmount)).Returns(false);

        // Act
        var result = mockBankService.Object.Deposit(account, depositAmount);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(0, account.Balance); // Balance should not change
    }
}
