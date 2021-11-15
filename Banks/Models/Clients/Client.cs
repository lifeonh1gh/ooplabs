using Banks.Tools;

namespace Banks.Models.Clients
{
    public class Client
    {
        public Client(int id, string name, string surname)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BanksException("Client should be with a name");
            }

            if (string.IsNullOrWhiteSpace(surname))
            {
                throw new BanksException("Client should be with a surname");
            }

            Id = id;
            Name = name;
            Surname = surname;
        }

        public bool IsSuspect { get; set; }
        public int Id { get; }
        public string Name { get; set; }
        public string Surname { get; }
        public string Address { get; set; }
        public string Passport { get; set; }
    }
}