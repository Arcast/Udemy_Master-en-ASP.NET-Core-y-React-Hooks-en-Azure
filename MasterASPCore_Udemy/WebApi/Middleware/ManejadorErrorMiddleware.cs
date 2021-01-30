using Aplicacion.ManejadorError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Middleware
{
    public class ManejadorErrorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ManejadorErrorMiddleware> logger;
        public ManejadorErrorMiddleware(RequestDelegate _next, ILogger<ManejadorErrorMiddleware> _logger)
        {
            next = _next;
            logger = _logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await ManejadorExceptionAsincrono(context, ex, logger);
            }            
        }

        private async Task ManejadorExceptionAsincrono(HttpContext context, Exception ex, ILogger<ManejadorErrorMiddleware> logger)
        {
            object errores = null;
            switch (ex)
            {
                case ManejadorException me :
                    logger.LogError(ex, "Manejador Error");
                    errores = me.errores;
                    context.Response.StatusCode = (int)me.codigo;
                    break;
                case Exception e:
                    logger.LogError(ex, "Error de servidor");
                    errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    break;
            }
            context.Response.ContentType = "application/json";
            if (errores != null)
            {
               var resultado = JsonConvert.SerializeObject(new { errores });
                await context.Response.WriteAsync(resultado);
            }
            
        }
    }
}
