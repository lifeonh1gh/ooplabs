using System;
using Banks.Interfaces;
using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Repositories;
using Banks.Tools;

namespace Banks.Models.Transactions
{
    public class WithdrawTransaction : Transaction
    {
        public WithdrawTransaction(int id, ref AccountsRepository accounts, Client client, Bank bank, int accountId, double sum, ITime time)
        {
            Id = id;
            Accounts = accounts;
            Client = client;
            if (Client == null)
            {
                throw new BanksException("client is null");
            }

            Bank = bank;
            if (Bank == null)
            {
                throw new BanksException("bank is null");
            }

            AccountId = accountId;
            Sum = sum;
            Time = time;
        }

        private AccountsRepository Accounts { get; }
        private Client Client { get; }
        private Bank Bank { get; }
        private int AccountId { get; }
        private double Sum { get; }

        public override bool ConfirmTransaction()
        {
            if (Client.IsSuspect && Bank.SuspectLimit < Sum)
            {
                return false;
            }

            Accounts.GetAccount(AccountId).Withdraw(Sum);
            return true;
        }

        public override bool DenyTransaction()
        {
            Accounts.GetAccount(AccountId).Put(Sum);
            return true;
        }
    }
}