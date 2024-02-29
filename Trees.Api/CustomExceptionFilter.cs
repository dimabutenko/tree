using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tree.Services.Exceptions;
using Trees.Exceptions;

namespace Trees;

public class CustomExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var result = context.Exception switch
        {
            TreeNodeException or DatabaseUpdateException => new ObjectResult(new ApiException(context.Exception)),
            not null => new ObjectResult(new ApiException(context.Exception)),
            _ => null
        };

        if (result == null) return;

        result.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = result;
    }
}

// {"type": "name of exception", "id": "id of event", "data": {"message": "message of exception"}}`
