using Helpdesk.Domain.Models.Business;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class FileModelBuilder
{
    public static ModelBuilder BuildFileModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<FileDataModel>()
            .ToTable("file", "business");

        entity
            .Property(file => file.Uid)
            .HasMaxLength(32);

        entity
            .Property(file => file.Name)
            .HasMaxLength(256);

        entity
            .Property(file => file.Hash)
            .HasMaxLength(64);

        entity
            .HasIndex(file => new
            {
                file.Uid
            })
            .IsUnique();

        entity
            .HasOne(file => file.UploadUser)
            .WithMany(user => user.Files)
            .OnDelete(DeleteBehavior.Cascade);

        var now = DateTimeOffset.UtcNow;

        entity
            .HasData(new[]
            {
                new FileDataModel
                {
                    Id = 1,
                    Uid = "c7f82c174aae445aa4ebe1d4f3a54ace",
                    Name = "Инструкция для пользования Helpdesk.pdf",
                    UploadUserId = 2,
                    CreationDate = now,
                    Hash = ""
                }
            });

        return modelBuilder;
    }
}