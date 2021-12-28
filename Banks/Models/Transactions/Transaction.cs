using Banks.Interfaces;

namespace Banks.Models.Transactions
{
    public class Transaction : ITransaction
    {
        public int Id { get; protected set; }
        protected ITime Time { get; set; }
        public virtual bool ConfirmTransaction()
        {
            return true;
        }

        public virtual bool DenyTransaction()
        {
            return true;
        }
    }
}