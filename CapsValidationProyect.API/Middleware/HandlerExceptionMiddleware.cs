using CapsValidationProyect.Application.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace CapsValidationProyect.API.Middleware
{
    public class HandlerExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HandlerExceptionMiddleware> _logger;
        public HandlerExceptionMiddleware(RequestDelegate next, ILogger<HandlerExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandlerExcepcionAsincrono(context, ex, _logger);
            }
        }

        private async Task HandlerExcepcionAsincrono(HttpContext context, Exception ex, ILogger<HandlerExceptionMiddleware> logger)
        {
            object errores = null;
            switch (ex)
            {
                case HandlerException me:
                    logger.LogError(ex, "Handler Error");
                    errores = me.Errors;
                    context.Response.StatusCode = (int)me.Code;
                    break;
                case Exception e:
                    logger.LogError(ex, "Error de Servidor");
                    errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.ContentType = "application/json";
            if (errores != null)
            {
                var resultados = JsonConvert.SerializeObject(new { errores });
                await context.Response.WriteAsync(resultados);
            }

        }
    }
}
