using System.Collections.Generic;
using System.Linq;
using Banks.Models.Clients;

namespace Banks.Repositories
{
    public class ClientsRepository
    {
        public List<Client> Clients { get; } = new List<Client>();

        public void AddClient(Client client)
        {
            Clients.Add(client);
        }

        public Client GetClient(int id)
            => Clients.Single(c => c.Id == id);

        public IEnumerable<Client> GetClients()
            => Clients;
    }
}