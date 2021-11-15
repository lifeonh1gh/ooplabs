using System.Collections.Generic;
using System.Linq;
using Banks.Models.Accounts;

namespace Banks.Repositories
{
    public class AccountsRepository
    {
        public List<Account> Accounts { get; } = new List<Account>();

        public void AddAccount(Account account)
        {
            Accounts.Add(account);
        }

        public Account GetAccount(int id)
            => Accounts.FirstOrDefault(a => a.AccountId == id);

        public IEnumerable<Account> GetAccounts()
            => Accounts;
    }
}