using WebApiCodeFirst.Models;

namespace WebApiCodeFirst.Repositorios.IRepositorios
{
    public interface ILogApiRepositorio
    {
        Task RegistrarLogAsync(LogApi log);
    }
}