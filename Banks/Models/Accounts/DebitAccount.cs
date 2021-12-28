using System;
using Banks.Interfaces;
using Banks.Models.Banks;
using Banks.Tools;

namespace Banks.Models.Accounts
{
    public class DebitAccount : Account
    {
        private double Percent { get; set; }
        private DateTime DateEnrollment { get; }

        public override double Put(double sum)
        {
            Sum += sum;
            return sum;
        }

        public override double Withdraw(double sum)
        {
            if (Sum <= 0)
                throw new BanksException("Not enough money to withdraw");
            Sum -= sum;
            return sum;
        }

        public override double CreditCommission(ITime time)
        {
            return 0;
        }

        public override double PercentOnBalance(ITime time)
        {
            if (time.Time < DateEnrollment.AddDays(1))
            {
                throw new BanksException("The term of accrual of interest on the balance has not yet come");
            }

            double sumEnrollment = Sum * (Percent / 365);
            return Sum + sumEnrollment;
        }

        public override Account CreateAccount(int accountId, int bankId, int clientId, Bank bank, double sum)
        {
            var account = new DebitAccount
            {
                AccountId = accountId,
                BankId = bankId,
                ClientId = clientId,
                Sum = sum,
                Percent = bank.Percent,
            };
            return account;
        }
    }
}