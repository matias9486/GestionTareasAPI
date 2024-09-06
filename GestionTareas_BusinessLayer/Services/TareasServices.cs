using GestionTareas_BusinessLayer.Dto;
using GestionTareas_BusinessLayer.Dto.Tarea;
using GestionTareas_BusinessLayer.Exceptions;
using GestionTareas_BusinessLayer.Interfaces;
using GestionTareas_DataAccessLayer.Interfaces;
using GestionTareas_DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionTareas_BusinessLayer.Services
{
    public class TareasServices : ITareasService
    {        
        private readonly ITareasRepository _repository;

        public TareasServices(ITareasRepository repository)
        {            
            _repository = repository;
        }
        public async Task<PaginationResponseDTO<TareaDTO>> GetAll(int pageNumber, int pageSize)
        {            
            var tareas = await _repository.GetAllTareasAsync();
            
            // Validaciones básicas
            if (tareas == null)
            {
                throw new InvalidOperationException("No se obtuvieron tareas del repositorio.");
            }

            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("El número de página y el tamaño de página deben ser mayores que 0.");
            }

            //Paginación
            var totalItems = tareas.Count();

            var paginatedTareas = tareas
                .Skip(pageSize * (pageNumber - 1)) //Saltear la x cantidad de productos de la lista
                .Take(pageSize) //Tomar la x cantidad de productos de la lista
                .Select(t => new TareaDTO   //convertir a DTO
                {
                    Id = t.Id,
                    Descripcion = t.Descripcion,
                    FechaCreacion = t.FechaCreacion,
                    IsTerminada = t.IsTerminada,
                    Username = t.usuario?.Username,
                    UsuarioId = t.UsuarioId

                })
                .ToList();            
            
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var paginationMetadata = new PaginationMetadata
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                HasNext = pageNumber < totalPages,
                NextPageUrl = pageNumber < totalPages ? $"/api/tareas/page/{pageNumber + 1}/size/{pageSize}" : null,
                HasPrevious = pageNumber > 1,
                PreviousPageUrl = pageNumber > 1 ? $"/api/tareas/page/{pageNumber - 1}/size/{pageSize}" : null
            };

            var response = new PaginationResponseDTO<TareaDTO>
            {
                Data = paginatedTareas,
                Pagination = paginationMetadata
            };

            return response;
        }        

        public async Task<TareaDTO> FindById(int id)
        {
            var tarea = await _repository.FindByIdAsync(id);

            if (tarea is null)
                throw new NotFoundException("No se encontró tarea con ese Id");

            var tareaDTO = new TareaDTO   //convertir a DTO
            {
                Id = tarea.Id,
                Descripcion = tarea.Descripcion,
                FechaCreacion = tarea.FechaCreacion,
                IsTerminada = tarea.IsTerminada,
                Username = tarea.usuario.Username,
                UsuarioId = tarea.UsuarioId
            };

            return tareaDTO;
        }

        public async Task<bool> CreateTarea(Tarea tarea)
        {
            try
            {                
                await _repository.AddAsync(tarea);
                await _repository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;                
            }                        
        }

        public async Task<bool> UpdateTarea(int id, UpdateTareaDTO tareaDTO)
        {
            var tareaExist = await _repository.ExistsTareaId(id);
            if (!tareaExist)
                throw new NotFoundException($"No se encontró tarea con el id: {id}");

            Tarea tarea = new Tarea {      
                Id =tareaDTO.Id,
                Descripcion = tareaDTO.Descripcion,
                IsTerminada = tareaDTO.IsTerminada ?? false, // Valor predeterminado si es null
                FechaCreacion = tareaDTO.FechaCreacion ?? DateTime.Now, // Valor predeterminado si es null
                UsuarioId = tareaDTO.UsuarioId
            };
            await _repository.UpdateAsync(tarea);
            await _repository.SaveChangesAsync();
            return true;            
        }

        public async Task<bool> DeleteTarea(long id)
        {
            var tareaBuscada = await _repository.GetByIdAsync(id);
            if (tareaBuscada is null)
                throw new NotFoundException($"No se encontró tarea con el id {id}");
            
            await _repository.DeleteAsync(tareaBuscada);            
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> FinalizeTarea(long id)
        {
            var tarea = await _repository.FindByIdAsync(id);
            if (tarea is null)
                throw new NotFoundException($"No se encontró tarea con el id: {id}");

            tarea.IsTerminada = true;                
            await _repository.UpdateAsync(tarea);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsTareaId(long id) {
            return await _repository.ExistsTareaId(id);
        }
    }
}
