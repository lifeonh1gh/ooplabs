using Banks.Models.Accounts;

namespace Banks.Interfaces
{
    public interface IBankService
    {
        void CreateClient(string name, string surname, string passport = null, string address = null);
        void AddPassportToClient(int clientId, string passport);
        void AddAddressToClient(int clientId, string address);
        void CreateBank(string name, int suspectLimit, int creditLimit, double commission, int periodDays, double percent);
        void CreateAccount(int bankId, int clientId, AccountType type, double sum);
        void ExecuteTransaction(int transactionId);
        void Put(int accountId, double sum);
        void Withdraw(int accountId, double sum);
        void Transfer(int accountOneId, int accountTwoId, double sum);
        void CancelTransaction(int transactionId);
        void PercentOnBalance(int accountId);
        void CreditCommission(int accountId);
        double CheckAccountSum(int accountId);
    }
}