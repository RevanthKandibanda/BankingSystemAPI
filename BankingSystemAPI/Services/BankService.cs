using BankingSystemAPI.Model;

namespace BankingSystemAPI.Services
{
    public class BankService : IBankService
    {
        private List<User> users;
        private int accountIdCounter = 1;

        public BankService()
        {
            // Initialize dummy data
            InitializeDummyData();
        }

        private void InitializeDummyData()
        {
            users = new List<User>
        {
            new User
            {
                UserId = 1,
                Accounts = new List<Account>
                {
                    new Account { AccountId = GetNextAccountId(), Balance = 1000 }
                }
            },
            new User
            {
                UserId = 2,
                Accounts = new List<Account>
                {
                    new Account { AccountId = GetNextAccountId(), Balance = 5000 },
                    new Account { AccountId = GetNextAccountId(), Balance = 3000 }
                }
            }
        };
        }

        public User GetUserById(int userId)
        {
            return users.FirstOrDefault(user => user.UserId == userId);
        }

        public User CreateUser()
        {
            var user = new User { UserId = users.Count + 1, Accounts = new List<Account>() };
            users.Add(user);
            return user;
        }

        public void DeleteAccount(User user, int accountId)
        {
            var account = user.Accounts.FirstOrDefault(a => a.AccountId == accountId);
            if (account != null)
            {
                user.Accounts.Remove(account);
            }
        }

        public Account CreateAccount(User user)
        {
            var account = new Account { AccountId = GetNextAccountId(), Balance = 100 }; // Initial balance $100
            user.Accounts.Add(account);
            return account;
        }

        public bool Deposit(Account account, decimal amount)
        {
            if (amount > 10000) // Maximum deposit limit is $10,000
                return false;

            account.Balance += amount;
            return true;
        }

        public bool Withdraw(Account account, decimal amount)
        {
            if (amount > account.Balance * 0.9m) // Cannot withdraw more than 90% of the balance
                return false;

            if (account.Balance - amount < 100) // Cannot have less than $100 in the account
                return false;

            account.Balance -= amount;
            return true;
        }

        private int GetNextAccountId()
        {
            return accountIdCounter++;
        }
        public Account GetAccountById(int accountId)
        {
            foreach (var user in users)
            {
                var account = user.Accounts.FirstOrDefault(a => a.AccountId == accountId);
                if (account != null)
                    return account;
            }

            return null;  // Account not found
        }
    }
}
