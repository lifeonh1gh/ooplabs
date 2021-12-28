namespace Banks.Interfaces
{
    public interface ITransaction
    {
        bool ConfirmTransaction();
        bool DenyTransaction();
    }
}