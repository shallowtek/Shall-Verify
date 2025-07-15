using Shall.Verify.Dashboard.Data.Entities;

namespace Shall.Verify.Dashboard.Data;

public interface IShallVerifyRepository
{
    Task<List<Client>> GetClientsAsync(string category);
    Task<Client?> GetClientByIdAsync(int id);
    Task<bool> IsClientNameUniqueAsync(string name);
    Task<Client> CreateClientAsync(Client client);
}
