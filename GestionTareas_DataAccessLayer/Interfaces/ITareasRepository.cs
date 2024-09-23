using GestionTareas_DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionTareas_DataAccessLayer.Interfaces
{
    public interface ITareasRepository : IRepository<Tarea>
    {
        Task<IEnumerable<Tarea>> GetAllTareasAsync();

        Task<IEnumerable<Tarea>> GetAllTareasByUserAsync(int userId);
        Task<Tarea>GetTareaByIdWithUserAsync(int id);
        Task<bool> ExistsTareaByUser(int tareaId, int userId);
    }
}
