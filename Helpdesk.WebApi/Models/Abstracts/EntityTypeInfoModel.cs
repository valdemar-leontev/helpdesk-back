namespace Helpdesk.WebApi.Models.Abstracts;

public class EntityTypeInfoModel
{
    private readonly string _entityTypeName = null!;

    public required string EntityTypeName
    {
        get => _entityTypeName.Replace("DataModel", string.Empty);

        init => _entityTypeName = value;
    }
}