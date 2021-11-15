using Banks.Repositories;

namespace Banks.Models.Clients
{
    public class ClientConstructor
    {
        private string _name;
        private string _surname;
        private string _address;
        private string _passport;
        public int Id { get; set; }

        public ClientConstructor SetName(string name)
        {
            _name = name;
            return this;
        }

        public ClientConstructor SetSurname(string surname)
        {
            _surname = surname;
            return this;
        }

        public ClientConstructor SetAddress(string address)
        {
            _address = address;
            return this;
        }

        public ClientConstructor SetPassport(string passport)
        {
            _passport = passport;
            return this;
        }

        public Client ClientConstruct()
        {
            var client = new Client(Id, _name, _surname);
            if (_address != null)
                client.Address = _address;
            client.IsSuspect = true;
            if (_passport != null)
                client.Passport = _passport;
            client.IsSuspect = true;
            if (_address != null && _passport != null)
            {
                client.IsSuspect = false;
            }

            return client;
        }
    }
}