using BankingSystemAPI.Model;

namespace BankingSystemAPI.Services
{
    public interface IBankService
    {
        User GetUserById(int userId);
        User CreateUser();
        void DeleteAccount(User user, int accountId);
        Account CreateAccount(User user);
        bool Deposit(Account account, decimal amount);
        bool Withdraw(Account account, decimal amount);
    }
}
