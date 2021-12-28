using System;
using Banks.Interfaces;
using Banks.Models.Banks;
using Banks.Tools;

namespace Banks.Models.Accounts
{
    public class CreditAccount : Account
    {
        private int MinLimit { get; set; }
        private int MaxLimit { get; set; }
        private double Commission { get; set; }
        private DateTime DateCommission { get; set; }

        public override double Put(double sum)
        {
            Sum += sum;
            return sum;
        }

        public override double Withdraw(double sum)
        {
            Sum -= sum;
            return sum;
        }

        public override double CreditCommission(ITime time)
        {
            if (Sum > MinLimit && DateCommission.AddDays(1) > time.Time)
            {
                throw new BanksException("Commission deduction error");
            }

            Sum -= Commission;
            DateCommission = time.Time;
            return Sum;
        }

        public override double PercentOnBalance(ITime time)
        {
            return 0;
        }

        public override Account CreateAccount(int accountId, int bankId, int clientId, Bank bank, double sum)
        {
            var account = new CreditAccount
            {
                AccountId = accountId,
                BankId = bankId,
                ClientId = clientId,
                Sum = sum,
                MinLimit = 0,
                MaxLimit = bank.CreditLimit,
                Commission = bank.Commission,
                DateCommission = DateCommission,
            };
            return account;
        }
    }
}