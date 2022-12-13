using LanguageExt.Common;
using Melody.WebAPI.DTO.Song;
using Microsoft.AspNetCore.Mvc;

namespace Melody.WebAPI.Extensions;

public static class ControllerExtensions
{
    public static IActionResult ToOk<TResult, TContract>(
        this Result<TResult> result, Func<TResult, TContract> mapper)
    {
        return result.Match<IActionResult>(
            obj => new OkObjectResult(mapper(obj)),
            exception => new BadRequestObjectResult(exception.Message));
    }
}