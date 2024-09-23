
using GestionTareas_BusinessLayer.Dto.Tarea;
using GestionTareas_BusinessLayer.Exceptions;
using GestionTareas_BusinessLayer.Interfaces;
using GestionTareas_DataAccessLayer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {        
        private readonly ITareasService _tareaService;

        public TareasController(GestionTareasDbContext context, ITareasService service)
        {            
            _tareaService = service;
        }

        [HttpGet]        
        [Route("page/{pageNumber}/size/{pageSize}")]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {            
            var tareas = await _tareaService.GetAll(pageNumber, pageSize);
            return Ok(tareas);            
        }

        [HttpGet]
        [Authorize]
        [Route("byUser/page/{pageNumber}/size/{pageSize}")]
        public async Task<IActionResult> GetAllByUser(int pageNumber = 1, int pageSize = 10)
        {
            var tareas = await _tareaService.GetAllByUserId(pageNumber, pageSize);
            return Ok(tareas);
        }

        [HttpGet("{id:int}", Name = "GetTarea")]
        [Authorize]
        public async Task<IActionResult> GetTarea(int id)
        {
            var tareaDTO = await _tareaService.GetTareaById(id);
            return Ok(tareaDTO);            
        }

        [HttpPost]        
        [Authorize]        
        public async Task<IActionResult> CreateTarea([FromBody] AddTareaDTO tareaDTO)
        {
            if (!ModelState.IsValid)            
                return BadRequest(ModelState);
           
            await _tareaService.CreateTarea(tareaDTO);
            return Created();
        }


        [HttpPut("{id}")]        
        [Authorize]        
        public async Task<IActionResult> UpdateTarea(int id, [FromBody] UpdateTareaDTO tareaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (tareaDTO.Id <= 0)
                throw new NotValidIdException("El Id de la tarea debe ser mayor a 0");
            
            if (id != tareaDTO.Id)
                return BadRequest("Los id no coinciden.");
            
            await _tareaService.UpdateTarea(id, tareaDTO);

            return NoContent();
        }


        [HttpDelete("{id:int}")]        
        [Authorize]        
        public async Task<IActionResult> DeleteTarea(int id)
        {
            await _tareaService.DeleteTarea(id);            
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [Authorize]        
        public async Task<IActionResult>  FinalizeTarea(int id)
        {
            await _tareaService.FinalizeTarea(id);
            return NoContent();
        }

    }
}
