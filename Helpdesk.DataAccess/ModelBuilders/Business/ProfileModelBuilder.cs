using Helpdesk.Domain.Models.Business;
using Helpdesk.Domain.Models.Dictionaries.Enums;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class ProfileModelBuilder
{
    public static ModelBuilder BuildProfileModel(this ModelBuilder modelBuilder)
    {
        var entity = modelBuilder
            .Entity<ProfileDataModel>()
            .ToTable("profile", "business");

        entity
            .Property(profile => profile.FirstName)
            .HasMaxLength(64);

        entity
            .Property(profile => profile.LastName)
            .HasMaxLength(64);

        entity
            .HasOne(p => p.User)
            .WithOne(u => u.Profile)
            .OnDelete(DeleteBehavior.Cascade);

        entity
            .HasOne(p => p.ProfileLinkSubdivision)
            .WithOne(s => s.Profile)
            .OnDelete(DeleteBehavior.Cascade);

        entity
          .HasData(new ProfileDataModel
              {
                Id = 1,
                FirstName="Vladimir",
                LastName="Leontev",
                PositionId=(int)Positions.Developer,
                UserId=1
              });

        entity
            .HasData(new ProfileDataModel {
                Id = 2,
                FirstName="Bill",
                LastName="Gates",
                PositionId=(int)Positions.Sdh,
                UserId=2
            });

        return modelBuilder;
    }
}