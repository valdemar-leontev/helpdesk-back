namespace Helpdesk.WebApi.Models.Abstracts;

public class EntityPostRequestModel : EntityPutRequestModel
{
    public required string[] UpdatedProperties { get; set; }
}