using GestionTareas_DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionTareas_DataAccessLayer.Interfaces
{
    public interface ITareasRepository : IRepository<Tarea>
    {
        Task<IEnumerable<Tarea>> GetAllTareasAsync();
        Task<Tarea>FindByIdAsync(long id);

        Task<bool> ExistsTareaId(long id);
        
    }
}
