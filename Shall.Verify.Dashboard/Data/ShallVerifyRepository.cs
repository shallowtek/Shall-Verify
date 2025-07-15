using Shall.Verify.Dashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Shall.Verify.Dashboard.Data;

public class ShallVerifyRepository(LocalContext ctx, ILogger<ShallVerifyRepository> logger) : IShallVerifyRepository
{
    public async Task<List<Client>> GetClientsAsync(string category)
    {
        logger.LogInformation("Getting products in repository for {category}", category);

        List<string> validCategories = ["Live", "Test"];

        try
        {
            if (!validCategories.Contains(category))
            {
                throw new Exception($"Simulated exception for category {category}");
            }
            return await ctx.Clients.Where(p => p.Category == category || category == "Live").ToListAsync();
        }
        catch (Exception ex)
        {
            var newEx = new ApplicationException("Something bad happened in database", ex);
            newEx.Data.Add("Category", category);
            throw newEx;
        }
    }

    public async Task<Client?> GetClientByIdAsync(int id)
    {
        return await ctx.Clients.FindAsync(id);
    }

    public Task<bool> IsClientNameUniqueAsync(string name)
    {
        return ctx.Clients.AllAsync(p => p.Name != name);
    }

    public async Task<Client> CreateClientAsync(Client client)
    {
        client.Name = client.Name!.Trim();
        ctx.Clients.Add(client);
        await ctx.SaveChangesAsync();
        return client;
    }
}