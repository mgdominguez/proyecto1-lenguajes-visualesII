using WebApiCodeFirst.Models;
using WebApiCodeFirst.Models.DTO;

namespace WebApiCodeFirst.Repositorios.IRepositorios
{
    public interface IUsuarioRepositorio
    {
        ICollection<Usuario> GetUsuarios();

        Usuario GetUsuario(int usuarioId);

        bool IsUniqueUser(string usuario);

        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);

        Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);
    }
}