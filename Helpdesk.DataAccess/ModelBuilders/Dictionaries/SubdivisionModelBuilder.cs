using Helpdesk.Domain.Models.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Dictionaries;

public static class SubdivisionModelBuilder
{
    public static ModelBuilder BuildSubdivisionModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<SubdivisionDataModel>()
            .ToTable("subdivision", "dictionaries");

        entity
            .Property(s => s.Description)
            .HasMaxLength(256);

        entity
            .HasIndex(s => new
            {
                s.Description
            })
            .IsUnique();

        entity
            .HasData(new[]
            {
                new SubdivisionDataModel { Id = 1, Description = "Helpdesk International" },
                new SubdivisionDataModel { Id = 2, Description = "Helpdesk IT" }
            });

        return modelBuilder;
    }
}