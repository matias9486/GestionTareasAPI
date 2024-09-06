using GestionTareas_BusinessLayer.Dto;
using GestionTareas_BusinessLayer.Dto.Tarea;
using GestionTareas_DataAccessLayer.Models;

namespace GestionTareas_BusinessLayer.Interfaces
{
    public interface ITareasService
    {
        Task<PaginationResponseDTO<TareaDTO>> GetAll(int pageNumber, int pageSize);
        //Task<List<TareaDTO>> GetAll(int pageNumber, int pageSize);        

        //Para no traer el objeto 
        Task<bool> ExistsTareaId(long id);

        Task<TareaDTO> FindById(int id);
        Task<bool> CreateTarea(Tarea tarea);

        Task<bool> UpdateTarea(int id, UpdateTareaDTO tareaDTO);

        Task<bool> DeleteTarea(long id);

        Task<bool> FinalizeTarea(long id);
    }
}
