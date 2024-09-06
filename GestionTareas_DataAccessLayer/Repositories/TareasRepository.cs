using GestionTareas_DataAccessLayer.Data;
using GestionTareas_DataAccessLayer.Interfaces;
using GestionTareas_DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionTareas_DataAccessLayer.Repositories
{
    public class TareasRepository : Repository<Tarea>, ITareasRepository
    {
        public TareasRepository(GestionTareasDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Tarea>> GetAllTareasAsync()
        {
            //return await _context.Set<T>().ToListAsync();
            return await _context.Tarea.Include(t => t.usuario).ToListAsync();
        }

        public async Task<Tarea> FindByIdAsync(long id)
        {
            return await _context.Tarea.Include(t => t.usuario).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> ExistsTareaId(long id)
        {
            return _context.Tarea.Any(t => t.Id == id);
        }
    }
}
