using LanguageExt.Common;
using Melody.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Melody.WebAPI.Extensions;

public static class ControllerExtensions
{
    public static IActionResult ToOk<TResult, TContract>(
        this Result<TResult> result, Func<TResult, TContract> mapper)
    {
        return result.Match(
            obj => new OkObjectResult(mapper(obj)),
            ToActionResult
        );
    }

    public static IActionResult ToStatusCode<TResult>(
        this Result<TResult> result, int statusCode)
    {
        return result.Match(
            obj => new StatusCodeResult(statusCode),
            ToActionResult
        );
    }

    public static IActionResult ToActionResult(this Exception exception)
    {
        return exception switch
        {
            RegistrationErrorException e => new BadRequestObjectResult(e),
            KeyNotFoundException => new NotFoundResult(),
            BannedUserException => new ForbidResult(),
            WrongPasswordException => new ForbidResult(),
            _ => new BadRequestObjectResult(exception.Message)
        };
    }
}