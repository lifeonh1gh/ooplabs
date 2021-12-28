using Banks.Models.Accounts;
using Banks.Models.Banks;

namespace Banks.Interfaces
{
    public interface IAccount
    {
        double Put(double sum);
        double Withdraw(double sum);
        Account CreateAccount(int accountId, int bankId, int clientId, Bank bank, double sum);
        double CreditCommission(ITime time);
        double PercentOnBalance(ITime time);
    }
}