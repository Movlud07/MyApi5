using MyApi5.Business.Errors;
using System.Text.Json;

namespace MyApi5.API.Middlewares
{
    public class MiddleWareForException
    {
        public readonly RequestDelegate _next;
        public MiddleWareForException(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //before
            //await Console.Out.WriteLineAsync("Request accepted.");

            //--------------------------------------------

            //now
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                //switch (ex)
                //{
                //    case DublicateException:
                //        context.Response.StatusCode = StatusCodes.Status409Conflict;
                //        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
                //        break;
                //    case ElementNotExistException:
                //        context.Response.StatusCode = StatusCodes.Status404NotFound;
                //        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
                //        break;
                //    case PasswordStateException:
                //        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                //        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
                //        break;
                //    default:
                //        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                //        await context.Response.WriteAsJsonAsync(new { error = "Bilinmedik xeta bas verdi" });
                //        break;
                //}

                string message = ex.Message;
                var errors = new List<RestExceptionError>();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                if (ex is RestException rest)
                {
                    
                    message = rest.Message;
                    errors = rest.Errors;
                    context.Response.StatusCode = rest.Code;
                }
                await context.Response.WriteAsJsonAsync(new { message, errors});
            }


            //--------------------------------------------

            //after
            //await Console.Out.WriteLineAsync("Response returned.");

        }

    }
}
