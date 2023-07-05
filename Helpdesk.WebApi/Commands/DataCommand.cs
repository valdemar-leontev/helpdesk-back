using System.ComponentModel;
using System.Reflection;
using AutoMapper;
using Helpdesk.DataAccess;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Utilities;

namespace Helpdesk.WebApi.Commands;

public abstract class DataCommand
{
    protected readonly AppDatabaseContext AppDatabaseContext;

    protected readonly IMapper Mapper;

    protected readonly IHttpContextAccessor HttpContextAccessor;

    protected int UserId => HttpContextAccessor.HttpContext!.GetUserId();

    protected DataCommand(AppDatabaseContext appDatabaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        AppDatabaseContext = appDatabaseContext;
        Mapper = mapper;
        HttpContextAccessor = httpContextAccessor;
    }

    protected virtual CommandResponseModel<T> CommandResponse<T>(T? content = default, string? errorDetail = default, int? statusCode = default)
    {
        return new CommandResponseModel<T>
        {
            Content = content,
            ErrorDetail = errorDetail
        };
    }

    protected string Description(Type type)
    {
        var descriptionAttr = type.GetCustomAttribute<DescriptionAttribute>();

        return descriptionAttr is not null
            ? descriptionAttr.Description
            : type.Name;
    }
}