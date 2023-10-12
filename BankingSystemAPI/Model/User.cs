namespace BankingSystemAPI.Model
{
    public class User
    {
        public int UserId { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}
