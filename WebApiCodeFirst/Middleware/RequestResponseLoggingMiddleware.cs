using System.Text;
using WebApiCodeFirst.Models;
using WebApiCodeFirst.Repositorios.IRepositorios;

namespace WebApiCodeFirst.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogApiRepositorio logApiRepositorio)
        {
            var request = context.Request;
            request.EnableBuffering();

            string requestBody = string.Empty;
            if (request.ContentLength > 0 && request.Body.CanSeek)
            {
                request.Body.Position = 0;
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            string responseText = string.Empty;
            string tipoLog = "Info";
            string? detalleError = null;
            string? userId = null;

            try
            {
                await _next(context);

                responseBody.Position = 0;
                responseText = await new StreamReader(responseBody).ReadToEndAsync();
                responseBody.Position = 0;

                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                tipoLog = "Error";
                detalleError = ex.ToString();
                throw;
            }
            finally
            {
                if (context.User?.Identity?.IsAuthenticated == true)
                {
                    userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                }

                var log = new LogApi
                {
                    Fecha = DateTime.UtcNow,
                    TipoLog = tipoLog,
                    RequestBody = requestBody,
                    ResponseBody = responseText,
                    UrlEndpoint = $"{request.Method} {request.Path}",
                    MetodoHttp = request.Method,
                    DireccionIp = context.Connection.RemoteIpAddress?.ToString(),
                    Detalle = detalleError,
                    Usuario = userId
                };

                await logApiRepositorio.RegistrarLogAsync(log);
            }
        }
    }
}