using System.Collections.Generic;
using System.Linq;
using Banks.Models.Transactions;

namespace Banks.Repositories
{
    public class TransactionsRepository
    {
        public List<Transaction> Transactions { get; } = new List<Transaction>();

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }

        public Transaction GetTransaction(int id)
            => Transactions.FirstOrDefault(t => t.Id == id);

        public IEnumerable<Transaction> GetTransactions()
            => Transactions;
    }
}