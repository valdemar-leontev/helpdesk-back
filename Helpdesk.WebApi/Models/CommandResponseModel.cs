namespace Helpdesk.WebApi.Models;

public class CommandResponseModel<T>
{
    public T? Content { get; set; }

    public string? ErrorDetail { get; set; }

    public int? StatusCode { get; set; }
}