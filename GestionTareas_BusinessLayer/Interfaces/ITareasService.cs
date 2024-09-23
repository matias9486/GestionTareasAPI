using GestionTareas_BusinessLayer.Dto;
using GestionTareas_BusinessLayer.Dto.Tarea;
using GestionTareas_DataAccessLayer.Models;

namespace GestionTareas_BusinessLayer.Interfaces
{
    public interface ITareasService
    {
        Task<PaginationResponseDTO<TareaDTO>> GetAll(int pageNumber, int pageSize);

        Task<PaginationResponseDTO<TareaDTO>> GetAllByUserId(int pageNumber, int pageSize);

        Task<TareaDTO> GetTareaById(int tareaId);

        //Para no traer el objeto         
        Task<bool> ExistsTareaByUser(int tareaId, int userId);
        
        Task CreateTarea(AddTareaDTO tarea);

        Task UpdateTarea(int id, UpdateTareaDTO tareaDTO);

        Task DeleteTarea(int id);

        Task FinalizeTarea(int id);
    }
}
