using Helpdesk.Domain.Models.Business;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.DataAccess.ModelBuilders.Business;

public static class UserAnswerModelBuilder
{
  public static ModelBuilder BuildUserAnswerModel(this ModelBuilder modelBuilder)
  {
    var entity = modelBuilder
      .Entity<UserAnswerDataModel>()
      .ToTable("user_answer", "business");

    entity
      .HasOne(userAnswer => userAnswer.Question)
      .WithMany(question => question.UserAnswers)
      .OnDelete(DeleteBehavior.Cascade);

    entity
      .HasOne(userAnswer => userAnswer.Profile)
      .WithMany(user => user.UserAnswers)
      .OnDelete(DeleteBehavior.Cascade);

    return modelBuilder;
  }
}