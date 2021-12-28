using System;
using Banks.Interfaces;
using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Repositories;
using Banks.Tools;

namespace Banks.Models.Transactions
{
    public class TransferTransaction : Transaction
    {
        public TransferTransaction(int id, ref AccountsRepository accounts, Client client, Bank bank, int accountOneId, int accountTwoId, double sum, ITime time)
        {
            Id = id;
            Accounts = accounts;
            if (Client == null)
            {
                throw new BanksException("client is null");
            }

            Client = client;
            if (Bank == null)
            {
                throw new BanksException("bank is null");
            }

            Bank = bank;
            AccountOneId = accountOneId;
            AccountTwoId = accountTwoId;
            Sum = sum;
            Time = time;
        }

        private AccountsRepository Accounts { get; }
        private Client Client { get; }
        private Bank Bank { get; }
        private int AccountOneId { get; }
        private int AccountTwoId { get; }
        private double Sum { get; }

        public override bool ConfirmTransaction()
        {
            if (Client.IsSuspect && Bank.SuspectLimit < Sum)
            {
                return false;
            }

            Accounts.GetAccount(AccountOneId).Withdraw(Sum);
            Accounts.GetAccount(AccountTwoId).Put(Sum);

            return true;
        }

        public override bool DenyTransaction()
        {
            Accounts.GetAccount(AccountTwoId).Withdraw(Sum);
            Accounts.GetAccount(AccountOneId).Put(Sum);
            return true;
        }
    }
}