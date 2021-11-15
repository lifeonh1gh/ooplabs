using Banks.Interfaces;
using Banks.Models.Accounts;
using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Models.Time;
using Banks.Models.Transactions;
using Banks.Repositories;
using Banks.Tools;

namespace Banks.Services
{
    public class BankService : IBankService
    {
        public BankService(BanksRepository banks, ClientsRepository clients, AccountsRepository accounts, TransactionsRepository transactions, FutureTime time)
        {
            Banks = banks;
            Clients = clients;
            Accounts = accounts;
            Transactions = transactions;
            Time = time;
        }

        public FutureTime Time { get; }
        private BanksRepository Banks { get; }
        private ClientsRepository Clients { get; }
        private AccountsRepository Accounts { get; }
        private TransactionsRepository Transactions { get; }

        public void CreateClient(string name, string surname, string passport = null, string address = null)
        {
            var client = new ClientConstructor();
            client.Id = Clients.Clients.Count;
            client.SetName(name);
            client.SetSurname(surname);
            client.SetPassport(passport);
            client.SetAddress(address);
            Clients.AddClient(client.ClientConstruct());
        }

        public void AddPassportToClient(int clientId, string passport)
        {
            var client = Clients.GetClient(clientId);
            if (client.Id != clientId)
            {
                throw new BanksException("Error adding passport data to client, client not found");
            }

            client.Passport = passport;
        }

        public void AddAddressToClient(int clientId, string address)
        {
            var client = Clients.GetClient(clientId);
            if (client.Id != clientId)
            {
                throw new BanksException("Error adding address data to client, client not found");
            }

            client.Address = address;
        }

        public void CreateBank(string name, int suspectLimit, int creditLimit, double commission, int periodDays, double percent)
        {
            var bank = new Bank(Banks.Banks.Count, name, suspectLimit, creditLimit, commission, periodDays, percent);
            Banks.AddBank(bank);
        }

        public void UpdateSuspectLimit(int bankId, int newLimit)
        {
            var bank = Banks.GetBank(bankId);
            if (bank.Id != bankId)
            {
                throw new BanksException("Error update limit for suspect account, bank not found");
            }

            bank.SuspectLimit = newLimit;
        }

        public void CreateAccount(int bankId, int clientId, AccountType type, double sum)
        {
            Account account;
            switch (type)
            {
                case AccountType.CreditAccount:
                    account = new CreditAccount();
                    break;
                case AccountType.DebitAccount:
                    account = new DebitAccount();
                    break;
                case AccountType.DepositAccount:
                    account = new DepositAccount();
                    break;
                default:
                    throw new BanksException("Account creation error, incorrect account type");
            }

            var client = Clients.GetClient(clientId);
            var bank = Banks.GetBank(bankId);
            if (client.Id != clientId)
            {
                throw new BanksException("Error adding an account, client not found");
            }

            if (bank.Id != bankId)
            {
                throw new BanksException("Error adding an account, bank not found");
            }

            Accounts.AddAccount(account.CreateAccount(Accounts.Accounts.Count, bankId, clientId, bank, sum));
        }

        public void ExecuteTransaction(int transactionId)
        {
            var transaction = Transactions.GetTransaction(transactionId);
            transaction.ConfirmTransaction();
        }

        public void CancelTransaction(int transactionId)
        {
            var transaction = Transactions.GetTransaction(transactionId);
            transaction.DenyTransaction();
        }

        public void Put(int accountId, double sum)
        {
            AccountsRepository accountsRepository = Accounts;
            var account = Accounts.GetAccount(accountId);
            if (account.AccountId != accountId)
            {
                throw new BanksException("Money put transaction error, account not found");
            }

            var temp = new PutTransaction(Transactions.Transactions.Count, ref accountsRepository, accountId, sum, Time);
            Transactions.AddTransaction(temp);
        }

        public void Withdraw(int accountId, double sum)
        {
            AccountsRepository accountsRepository = Accounts;
            var bank = Banks.GetBank(Accounts.GetAccount(accountId).BankId);
            var client = Clients.GetClient(Accounts.GetAccount(accountId).ClientId);
            var temp = new WithdrawTransaction(Transactions.Transactions.Count, ref accountsRepository, client, bank, accountId, sum, Time);
            var account = Accounts.GetAccount(accountId);
            if (account.AccountId != accountId)
            {
                throw new BanksException("Money withdraw transaction error, account not found");
            }

            if (client.IsSuspect && sum > bank.SuspectLimit)
            {
                throw new BanksException("The account is suspicious, the withdrawal operation is prohibited");
            }

            Transactions.AddTransaction(temp);
        }

        public void Transfer(int accountOneId, int accountTwoId, double sum)
        {
            AccountsRepository accountsRepository = Accounts;
            var bank = Banks.GetBank(Accounts.GetAccount(accountOneId).BankId);
            var client = Clients.GetClient(Accounts.GetAccount(accountOneId).ClientId);
            var temp = new TransferTransaction(Transactions.Transactions.Count, ref accountsRepository, client, bank, accountOneId, accountTwoId, sum, Time);
            var accountOne = Accounts.GetAccount(accountOneId);
            var accountTwo = Accounts.GetAccount(accountTwoId);
            if (accountOne.AccountId != accountOneId && accountTwo.AccountId != accountTwoId)
            {
                throw new BanksException("Money transfer transaction error, account not found");
            }

            if (client.IsSuspect && sum > bank.SuspectLimit)
            {
                throw new BanksException("The account is suspicious, the transfer operation is prohibited");
            }

            Transactions.AddTransaction(temp);
        }

        public void CreditCommission(int accountId)
        {
            AccountsRepository accountsRepository = Accounts;
            var account = Accounts.GetAccount(accountId);
            var temp = new CreditCommissionTransaction(Transactions.Transactions.Count, ref accountsRepository, accountId, Time);
            if (account.AccountId != accountId)
            {
                throw new BanksException("Account not found");
            }

            Transactions.AddTransaction(temp);
        }

        public void PercentOnBalance(int accountId)
        {
            AccountsRepository accountsRepository = Accounts;
            var account = Accounts.GetAccount(accountId);
            var temp = new PercentOnBalanceTransaction(Transactions.Transactions.Count, ref accountsRepository, accountId, Time);
            if (account.AccountId != accountId)
            {
                throw new BanksException("Account not found");
            }

            Transactions.AddTransaction(temp);
        }

        public double CheckAccountSum(int accountId)
        {
            return Accounts.GetAccount(accountId).Sum;
        }
    }
}