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
            return await _context.Tarea.Include(t => t.usuario).ToListAsync();
        }

        //Agregado para buscar tareas por usuario
        public async Task<IEnumerable<Tarea>> GetAllTareasByUserAsync(int userId)
        {            
            return await _context.Tarea.Where(u => u.UsuarioId == userId).Include(t => t.usuario).ToListAsync();
        }
        

        //Incluye al usuario también. Sino uso algo del usuario, uso el GetById del repositorio generico
        public async Task<Tarea> GetTareaByIdWithUserAsync(int id)
        {
            return await _context.Tarea.Include(t => t.usuario).FirstOrDefaultAsync(t => t.Id == id);
        }
        
        public async Task<bool> ExistsTareaByUser(int tareaId, int userId)
        {
            return _context.Tarea.Any(t => t.Id == tareaId && t.UsuarioId == userId);
        }
    }
}
