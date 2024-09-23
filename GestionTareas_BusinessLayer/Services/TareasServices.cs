using GestionTareas_BusinessLayer.Dto;
using GestionTareas_BusinessLayer.Dto.Tarea;
using GestionTareas_BusinessLayer.Exceptions;
using GestionTareas_BusinessLayer.Interfaces;
using GestionTareas_DataAccessLayer.Interfaces;
using GestionTareas_DataAccessLayer.Models;

namespace GestionTareas_BusinessLayer.Services
{
    public class TareasServices : ITareasService
    {        
        private readonly ITareasRepository _repository;
        private readonly ICurrentUserService _currentUserService;


        public TareasServices(ITareasRepository repository, ICurrentUserService currentUserService)
        {            
            _repository = repository;
            _currentUserService = currentUserService;
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
                    Username = t.usuario?.UserName,
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

        public async Task<PaginationResponseDTO<TareaDTO>> GetAllByUserId(int pageNumber, int pageSize)
        {
            var currentUserId = (int)_currentUserService.UserId;
            var tareas = await _repository.GetAllTareasByUserAsync(currentUserId);

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
                    Username = t.usuario?.UserName,
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

        public async Task<TareaDTO> GetTareaById(int tareaId) {
            var currentUserId = (int)_currentUserService.UserId;
            var tareaExist = await ExistsTareaByUser(tareaId, currentUserId);
            if (!tareaExist)
                throw new NotFoundException($"No se encontró tarea con el id: {tareaId}");

            var tarea = await _repository.GetTareaByIdWithUserAsync(tareaId);

            var tareaDTO = new TareaDTO   //convertir a DTO
            {
                Id = tarea.Id,
                Descripcion = tarea.Descripcion,
                FechaCreacion = tarea.FechaCreacion,
                IsTerminada = tarea.IsTerminada,
                Username = tarea.usuario.UserName,
                UsuarioId = tarea.UsuarioId
            };

            return tareaDTO;
        }
              
        public async Task CreateTarea(AddTareaDTO tarea)
        {
            var currentUserId = (int)_currentUserService.UserId;

            var newTarea = new Tarea
            {
                Descripcion = tarea.Descripcion,
                IsTerminada = false,
                UsuarioId = currentUserId,
                FechaCreacion = DateTime.Now
            };
                                    
            await _repository.AddAsync(newTarea);
            await _repository.SaveChangesAsync();            
        }

        public async Task UpdateTarea(int id, UpdateTareaDTO tareaDTO)
        {
            var currentUserId = (int)_currentUserService.UserId;
            var tareaExist = await ExistsTareaByUser(id, currentUserId);
            if (!tareaExist)
                throw new NotFoundException($"No se encontró tarea con el id: {id}");

            var tarea = await _repository.GetByIdAsync(id);
            tarea.Descripcion = tareaDTO.Descripcion;
            tarea.FechaCreacion = tareaDTO.FechaCreacion ?? tarea.FechaCreacion; // Valor predeterminado si es null                            

            await _repository.UpdateAsync(tarea);
            await _repository.SaveChangesAsync();                   
        }

        public async Task DeleteTarea(int id)
        {
            var currentUserId = (int)_currentUserService.UserId;
            var tareaExist = await ExistsTareaByUser(id, currentUserId);
            if (!tareaExist)
                throw new NotFoundException($"No se encontró tarea con el id: {id}");

            var tareaBuscada = await _repository.GetByIdAsync(id);            
            
            await _repository.DeleteAsync(tareaBuscada);            
            await _repository.SaveChangesAsync();            
        }

        public async Task FinalizeTarea(int id)
        {
            var currentUserId = (int)_currentUserService.UserId;
            var tareaExist = await ExistsTareaByUser(id,currentUserId);
            if (!tareaExist)
                throw new NotFoundException($"No se encontró tarea con el id: {id}");

            var tarea = await _repository.GetByIdAsync(id);            
            tarea.IsTerminada = true;                

            await _repository.UpdateAsync(tarea);
            await _repository.SaveChangesAsync();            
        }
       
        public async Task<bool> ExistsTareaByUser(int tareaId, int userId)
        {
            return await _repository.ExistsTareaByUser(tareaId, userId);
        }
        
    }
}
