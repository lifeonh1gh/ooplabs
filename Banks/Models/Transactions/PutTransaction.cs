using Banks.Interfaces;
using Banks.Repositories;

namespace Banks.Models.Transactions
{
    public class PutTransaction : Transaction
    {
        public PutTransaction(int id, ref AccountsRepository accounts, int accountId, double sum, ITime time)
        {
            Id = id;
            Accounts = accounts;
            AccountId = accountId;
            Sum = sum;
            Time = time;
        }

        private AccountsRepository Accounts { get; }
        private int AccountId { get; }
        private double Sum { get; }

        public override bool ConfirmTransaction()
        {
            Accounts.GetAccount(AccountId).Put(Sum);
            return true;
        }

        public override bool DenyTransaction()
        {
            Accounts.GetAccount(AccountId).Withdraw(Sum);
            return true;
        }
    }
}