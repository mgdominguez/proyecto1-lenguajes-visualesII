using WebApiCodeFirst.Data;
using WebApiCodeFirst.Models;
using WebApiCodeFirst.Repositorios.IRepositorios;

namespace WebApiCodeFirst.Repositorios
{
    public class LogApiRepositorio : ILogApiRepositorio
    {
        private readonly ApplicationDbContext _db;

        public LogApiRepositorio(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task RegistrarLogAsync(LogApi log)
        {
            log.Fecha = DateTime.UtcNow;
            await _db.logApi.AddAsync(log);
            await _db.SaveChangesAsync();
        }
    }
}