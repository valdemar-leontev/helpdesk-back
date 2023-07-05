namespace Helpdesk.WebApi.Models.Abstracts;

public class EntityPutRequestModel : EntityTypeInfoModel
{
    public required string Json { get; set; }
}