using Banks.Interfaces;
using Banks.Models.Banks;

namespace Banks.Models.Accounts
{
    public enum AccountType
    {
        DebitAccount,
        DepositAccount,
        CreditAccount,
    }

    public class Account : IAccount
    {
        public int AccountId { get; set; }
        public int BankId { get; protected set; }
        public int ClientId { get; protected set; }
        public double Sum { get; set; }

        public virtual double Put(double sum)
        {
            return sum;
        }

        public virtual double Withdraw(double sum)
        {
            return sum;
        }

        public virtual Account CreateAccount(int accountId, int bankId, int clientId, Bank bank, double sum)
        {
            return this;
        }

        public virtual double CreditCommission(ITime time)
        {
            return 0;
        }

        public virtual double PercentOnBalance(ITime time)
        {
            return 0;
        }
    }
}