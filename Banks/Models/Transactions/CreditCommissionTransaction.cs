using Banks.Interfaces;
using Banks.Repositories;

namespace Banks.Models.Transactions
{
    public class CreditCommissionTransaction : Transaction
    {
        public CreditCommissionTransaction(int id, ref AccountsRepository accounts, int accountId, ITime time)
        {
            Id = id;
            Accounts = accounts;
            AccountId = accountId;
            Time = time;
        }

        private int AccountId { get; }
        private AccountsRepository Accounts { get; }

        public override bool ConfirmTransaction()
        {
            Accounts.GetAccount(AccountId).CreditCommission(Time);
            return true;
        }
    }
}