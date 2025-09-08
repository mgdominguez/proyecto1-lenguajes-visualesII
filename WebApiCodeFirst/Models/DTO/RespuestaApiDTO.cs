using System.Net;

namespace WebApiCodeFirst.Models.DTO
{
    public class RespuestaApiDTO
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }

        public RespuestaApiDTO()
        {
            ErrorMessages = new List<string>();
        }
    }
}