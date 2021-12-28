using System;
using Banks.Interfaces;
using Banks.Models.Banks;
using Banks.Tools;

namespace Banks.Models.Accounts
{
    public class DepositAccount : Account
    {
        private double Percent { get; set; }
        private DateTime PeriodDays { get; set; }
        private DateTime DateEnrollment { get; }

        public override double Put(double sum)
        {
            Sum += sum;
            return sum;
        }

        public override double Withdraw(double sum)
        {
            if (PeriodDays >= DateTime.Today)
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
            if (time.Time < DateEnrollment.AddMonths(1))
            {
                throw new BanksException(
                    "An error in calculating interest on the balance, the accrual date did not come up");
            }

            double sumEnrollment = (Sum * Percent) / 100;
            return Sum + sumEnrollment;
        }

        public override Account CreateAccount(int accountId, int bankId, int clientId, Bank bank, double sum)
        {
            var account = new DepositAccount
            {
                AccountId = accountId,
                BankId = bankId,
                ClientId = clientId,
                Sum = sum,
                Percent = SetPercent(),
                PeriodDays = PeriodDays,
            };
            return account;
        }

        private double SetPercent()
        {
            if (Sum < 50000) return 3;
            else if (Sum >= 50000 && Sum <= 100000) return 3.5;
            else return 4;
        }
    }
}