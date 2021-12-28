using Banks.Tools;

namespace Banks.Models.Banks
{
    public class Bank
    {
        public Bank(int id, string name, int suspectLimit, int creditLimit, double commission, int periodDays, double percent)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BanksException("Bank should be with a name");
            }

            Id = id;
            Name = name;
            SuspectLimit = suspectLimit;
            CreditLimit = creditLimit;
            Commission = commission;
            PeriodDays = periodDays;
            Percent = percent;
        }

        public int Id { get; }
        public string Name { get; set; }
        public int SuspectLimit { get; set; }
        public int CreditLimit { get; }
        public double Commission { get; }
        public int PeriodDays { get; }
        public double Percent { get; set; }
    }
}