using Banks.Models.Accounts;
using Banks.Models.Time;
using Banks.Repositories;
using Banks.Services;
using Banks.Tools;
using NUnit.Framework;

namespace Banks.Tests
{
    [TestFixture]
    public class BanksTest
    {
        private BanksRepository _banksRepository;
        private ClientsRepository _clientsRepository;
        private AccountsRepository _accountsRepository;
        private TransactionsRepository _transactionsRepository;
        private FutureTime _futureTime;
        private BankService _bankService;

        [SetUp]
        public void Setup()
        {
            _banksRepository = new BanksRepository();
            _clientsRepository = new ClientsRepository();
            _accountsRepository = new AccountsRepository();
            _transactionsRepository = new TransactionsRepository();
            _futureTime = new FutureTime();
            _bankService = new BankService(_banksRepository, _clientsRepository, _accountsRepository,
                _transactionsRepository, _futureTime);
        }

        [Test]
        public void CreateAccountForClientWithoutPassportOrAddress_ClientIsSuspect()
        {
            _bankService.CreateBank("Tinkoff", 1000, 100000, 500, 10, 7);
            _bankService.CreateClient("Radik", "Kulikov");
            _bankService.CreateAccount(0, 0, AccountType.DebitAccount, 0);
            Assert.AreEqual(true, _clientsRepository.GetClient(0).IsSuspect);
        }

        [Test]
        public void PutMoneyToAccount_BalanceHasChanged()
        {
            _bankService.CreateBank("Tinkoff", 1000, 100000, 500, 10, 7);
            _bankService.CreateClient("Radik", "Kulikov", "8718822423", "Komendantskiy pr. 62");
            _bankService.CreateAccount(0, 0, AccountType.DebitAccount, 0);
            _bankService.Put(0, 10000);
            _bankService.ExecuteTransaction(0);
            Assert.AreEqual(10000, _bankService.CheckAccountSum(0));
        }

        [Test]
        public void WithdrawMoneyFromAccount_BalanceHasChanged()
        {
            _bankService.CreateBank("Tinkoff", 1000, 100000, 500, 10, 7);
            _bankService.CreateClient("Radik", "Kulikov", "8718822423", "Komendantskiy pr. 62");
            _bankService.CreateAccount(0, 0, AccountType.DebitAccount, 0);
            _bankService.Put(0, 10000);
            _bankService.ExecuteTransaction(0);
            _bankService.Withdraw(0, 1000);
            _bankService.ExecuteTransaction(1);
            Assert.AreEqual(9000, _bankService.CheckAccountSum(0));
        }

        [Test]
        public void TransferMoneyFromAccountToAnother_BalanceHasChanged()
        {
            _bankService.CreateBank("Tinkoff", 1000, 100000, 500, 10, 7);
            _bankService.CreateClient("Radik", "Kulikov", "8718822423", "Komendantskiy pr. 62");
            _bankService.CreateAccount(0, 0, AccountType.DebitAccount, 0);
            _bankService.CreateAccount(0, 0, AccountType.CreditAccount, 50000);
            _bankService.Put(0, 10000);
            _bankService.ExecuteTransaction(0);
            _bankService.Transfer(0, 1, 5000);
            _bankService.ExecuteTransaction(1);
            Assert.AreEqual(5000, _bankService.CheckAccountSum(0));
            Assert.AreEqual(55000, _bankService.CheckAccountSum(1));
        }

        [Test]
        public void CancelTransactions_BalanceHasChanged()
        {
            _bankService.CreateBank("Tinkoff", 1000, 100000, 500, 10, 7);
            _bankService.CreateClient("Radik", "Kulikov", "8718822423", "Komendantskiy pr. 62");
            _bankService.CreateAccount(0, 0, AccountType.DebitAccount, 0);
            _bankService.CreateAccount(0, 0, AccountType.CreditAccount, 50000);

            _bankService.Put(0, 10000);
            _bankService.ExecuteTransaction(0);
            _bankService.CancelTransaction(0);
            Assert.AreEqual(0, _bankService.CheckAccountSum(0));

            _bankService.Withdraw(1, 10000);
            _bankService.ExecuteTransaction(1);
            _bankService.CancelTransaction(1);
            Assert.AreEqual(50000, _bankService.CheckAccountSum(1));

            _bankService.Put(0, 10000);
            _bankService.ExecuteTransaction(2);
            _bankService.Transfer(0, 1, 5000);
            _bankService.ExecuteTransaction(3);
            _bankService.CancelTransaction(3);
            Assert.AreEqual(10000, _bankService.CheckAccountSum(0));
            Assert.AreEqual(50000, _bankService.CheckAccountSum(1));
        }
        
        [Test]
        public void CreditAccountCommission_BalanceMinusCommission()
        {
            _bankService.CreateBank("Tinkoff", 1000, 100000, 500, 10, 7);
            _bankService.CreateClient("Radik", "Kulikov", "8718822423", "Komendantskiy pr. 62");
            _bankService.CreateAccount(0, 0, AccountType.CreditAccount, 50000);

            _bankService.Withdraw(0, 55000);
            _bankService.ExecuteTransaction(0);
            _bankService.CreditCommission(0);
            _bankService.ExecuteTransaction(1);
            Assert.AreEqual(-5500, _bankService.CheckAccountSum(0));
        }

        [Test]
        public void TransferAndWithdrawMoneyMoreLimitForSuspectClient_ThrowException()
        {
            _bankService.CreateBank("Tinkoff", 1000, 100000, 500, 10, 7);
            _bankService.CreateClient("Radik", "Kulikov");
            _bankService.CreateAccount(0, 0, AccountType.DebitAccount, 0);
            _bankService.CreateAccount(0, 0, AccountType.CreditAccount, 50000);
            _bankService.Put(0, 10000);
            _bankService.ExecuteTransaction(0);

            Assert.Catch<BanksException>(() =>
            {
                _bankService.Withdraw(0, 5000);
                _bankService.ExecuteTransaction(1);
            });
            
            Assert.Catch<BanksException>(() =>
            {
                _bankService.Transfer(0, 1, 5000);
                _bankService.ExecuteTransaction(2);
            });
        }
        
        [Test]
        public void TransactionsToNonExistentAccount_ThrowException()
        {
            _bankService.CreateBank("Tinkoff", 1000, 100000, 500, 10, 7);
            _bankService.CreateClient("Radik", "Kulikov");
            _bankService.CreateAccount(0, 0, AccountType.DebitAccount, 0);
            _bankService.CreateAccount(0, 0, AccountType.CreditAccount, 50000);
            Assert.Catch<BanksException>(() =>
            {
                _bankService.Put(2, 10000);
                _bankService.ExecuteTransaction(0);
            });
            
            Assert.Catch<BanksException>(() =>
            {
                _bankService.Withdraw(2, 5000);
                _bankService.ExecuteTransaction(1);
            });
            
            Assert.Catch<BanksException>(() =>
            {
                _bankService.Transfer(2, 3, 5000);
                _bankService.ExecuteTransaction(2);
            });
        }
    }
}