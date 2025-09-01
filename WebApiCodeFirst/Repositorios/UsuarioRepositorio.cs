using WebApiCodeFirst.Data;
using WebApiCodeFirst.Models;
using WebApiCodeFirst.Models.DTO;
using WebApiCodeFirst.Repositorios.IRepositorios;

namespace WebApiCodeFirst.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private string claveSecreta;

        public UsuarioRepositorio(ApplicationDbContext bd, IConfiguration config)
        {
            _bd = bd;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
        }

        public Usuario GetUsuario(int usuarioId)
        {
            return _bd.usuario.FirstOrDefault(c => c.Id == usuarioId);
        }

        public ICollection<Usuario> GetUsuarios()
        {
            throw new NotImplementedException();
        }

        public bool IsUniqueUser(string usuario)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            throw new NotImplementedException();
        }
    }
}