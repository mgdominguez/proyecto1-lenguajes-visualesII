using System.ComponentModel.DataAnnotations;

namespace WebApiCodeFirst.Models
{
    public class LogApi
    {
        [Key]
        public long IdLog { get; set; }

        public DateTime Fecha { get; set; }

        public string TipoLog { get; set; } = null!;

        public string? RequestBody { get; set; }

        public string? ResponseBody { get; set; }

        public string? UrlEndpoint { get; set; }

        public string? MetodoHttp { get; set; }

        public string? DireccionIp { get; set; }

        public string? Usuario { get; set; }

        public string? Detalle { get; set; }
    }
}