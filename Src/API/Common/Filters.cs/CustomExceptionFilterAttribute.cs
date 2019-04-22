using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace API.Common.Filters.cs
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException exception)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                var result = Result.Fail(exception.Errors, "Error validating content");
                context.Result = new JsonResult(result);
                return;
            }

            var code = HttpStatusCode.InternalServerError;

            if (context.Exception is NotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = new JsonResult(Result.Fail(context.Exception.Message));
        }
    }
}
