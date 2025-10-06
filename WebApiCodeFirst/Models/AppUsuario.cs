using Microsoft.AspNetCore.Identity;

namespace WebApiCodeFirst.Models
{
    public class AppUsuario : IdentityUser
    {
        public string Nombre { get; set; }
    }
}