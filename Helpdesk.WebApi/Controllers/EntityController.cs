using Microsoft.AspNetCore.Mvc;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Commands.Entities;
using Helpdesk.WebApi.Models.Abstracts;

namespace Helpdesk.WebApi.Controllers;

[ApiController]
[Route("/entities")]
public class EntityController : ControllerBase
{
    [HttpGet]
    public IActionResult GetEntity([FromServices] GetEntityCommand command, [FromQuery] EntityGetRequestModel entityGetRequest)
    {
        var entityResponse = command.Get(entityGetRequest);

        return entityResponse.Content is not null
            ? Ok(entityResponse.Content)
            : Problem(entityResponse.ErrorDetail);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetEntityList([FromServices] GetEntitiesListCommand command, [FromQuery] EntityGetRequestModel entityGetRequest)
    {
        var listEntitiesResponse = await command.GetListAsync(entityGetRequest);

        return listEntitiesResponse.Content is not null
            ? Ok(listEntitiesResponse.Content)
            : Problem(listEntitiesResponse.ErrorDetail);
    }

    [HttpPut]
    public async Task<IActionResult> PutEntity([FromServices] PutEntityCommand command, EntityPutRequestModel entityPutRequest)
    {
        var entityResponse = await command.PutAsync(entityPutRequest);

        return entityResponse.Content is not null
            ? Ok(entityResponse.Content)
            : Problem(entityResponse.ErrorDetail);
    }

    [HttpGet("count")]
    public IActionResult GetCountEntities([FromServices] GetCountEntitiesCommand command, [FromQuery] EntityGetRequestModel entityGetRequest)
    {
        var countEntitiesResponse = command.GetCount(entityGetRequest);

        return countEntitiesResponse.Content is not null
            ? Ok(countEntitiesResponse.Content)
            : Problem(countEntitiesResponse.ErrorDetail);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteEntity([FromServices] DeleteEntitiesCommand command, EntityGetRequestModel entityGetRequest)
    {
        var deletedEntitiesResponse = await command.DeleteAsync(entityGetRequest);

        return deletedEntitiesResponse.Content is not null
            ? Ok(deletedEntitiesResponse.Content)
            : Problem(deletedEntitiesResponse.ErrorDetail);
    }

    [HttpPost]
    public async Task<IActionResult> PostEntity([FromServices] PostEntityCommand command, EntityPostRequestModel entityPostRequest)
    {
        var entityResponse = await command.PostAsync(entityPostRequest);

        return entityResponse.Content is not null
            ? Ok(entityResponse.Content)
            : Problem(entityResponse.ErrorDetail);
    }
}