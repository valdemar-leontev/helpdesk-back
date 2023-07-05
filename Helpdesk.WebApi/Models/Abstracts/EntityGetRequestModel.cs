namespace Helpdesk.WebApi.Models.Abstracts;

public class EntityGetRequestModel : EntityTypeInfoModel
{
    public string? Includes { get; set; }

    public string? Filter { get; set; }
}