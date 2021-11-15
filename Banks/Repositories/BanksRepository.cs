using System.Collections.Generic;
using System.Linq;
using Banks.Models.Banks;

namespace Banks.Repositories
{
    public class BanksRepository
    {
        public List<Bank> Banks { get; } = new List<Bank>();

        public void AddBank(Bank bank)
        {
            Banks.Add(bank);
        }

        public Bank GetBank(int id) =>
            Banks.FirstOrDefault(b => b.Id == id);

        public IEnumerable<Bank> GetBanks()
            => Banks;
    }
}