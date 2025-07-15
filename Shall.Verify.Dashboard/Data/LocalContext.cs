using Shall.Verify.Dashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Shall.Verify.Dashboard.Data;

public class LocalContext(DbContextOptions<LocalContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; } = null!;

    [ExcludeFromCodeCoverage]
    public void MigrateAndCreateData()
    {
        //Database.EnsureDeleted();
        Database.Migrate();

        if (Clients.Any())
        {
            Clients.RemoveRange(Clients);
            SaveChanges();
        };

        Clients.Add(new Client
        {
            Name = "Fanta",
            Category = "Live",
            Description = "Great support in this high-top to take you to great heights and trails.",
        });
        Clients.Add(new Client
        {
            Name = "Coke",
            Category = "Live",
            Description =
                "Easy in and out with this lightweight but rugged shoe with great ventilation to get your around shores, beaches, and boats.",
        });
        Clients.Add(new Client
        {
            Name = "Pepsi",
            Category = "Live",
            Description =
                "All the insulation and support you need when wandering the rugged trails of the woods and backcountry.",
        });
        Clients.Add(new Client
        {
            Name = "Lilt",
            Category = "Live",

            Description =
                "Get up and down rocky terrain like a billy-goat with these awesome high-top boots with outstanding support.",
        });
        Clients.Add(new Client
        {
            Name = "7up",
            Category = "Test",
            Description =
           "Manage and carry your gear with ease using this backpack with great lumbar support.",
        });
        Clients.Add(new Client
        {
            Name = "Sprite",
            Category = "Test",
            Description =
                "Navigate tricky waterways easily with this great and manageable kayak.",
        });

        SaveChanges();
    }
}