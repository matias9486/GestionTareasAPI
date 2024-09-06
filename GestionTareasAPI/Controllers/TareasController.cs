
using GestionTareas_BusinessLayer.Dto.Tarea;
using GestionTareas_BusinessLayer.Exceptions;
using GestionTareas_BusinessLayer.Interfaces;
using GestionTareas_DataAccessLayer.Data;
using GestionTareas_DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionTareasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        //private readonly GestionTareasDbContext _context;
        private readonly ITareasService _tareaService;

        public TareasController(GestionTareasDbContext context, ITareasService service)
        {
            //_context = context;
            _tareaService = service;
        }

        [HttpGet]
        [Route("/page/{pageNumber}/size/{pageSize}")]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {            
            var tareas = await _tareaService.GetAll(pageNumber, pageSize);
            return Ok(tareas);            
        }

        [HttpGet("{id:int}", Name = "GetTarea")]
        public async Task<IActionResult> GetTarea(int id)
        {
            var tareaDTO = await _tareaService.FindById(id);
            return Ok(tareaDTO);            
        }

        [HttpPost]
        public async Task<IActionResult> CreateTarea([FromBody] AddTareaDTO tareaDTO)
        {
            if (!ModelState.IsValid)            
                return BadRequest(ModelState);

            if (tareaDTO.UsuarioId <= 0)
                throw new NotValidIdException("El Id del usuario debe ser mayor a 0");
                //return BadRequest("El Id del usuario debe ser mayor a 0");

            var tarea = new Tarea
            {
                Descripcion = tareaDTO.Descripcion,
                IsTerminada = false,
                UsuarioId = tareaDTO.UsuarioId,
                FechaCreacion = DateTime.Now
            };            
            var succeeded = await _tareaService.CreateTarea(tarea);
            if (succeeded)
                return CreatedAtAction("GetTarea", new { id = tarea.Id }, tarea); //post redirect get
            else
                return BadRequest("No se pudo agregar tarea");
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTarea(int id, [FromBody] UpdateTareaDTO tareaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (tareaDTO.Id <= 0)
                throw new NotValidIdException("El Id de la tarea debe ser mayor a 0");

            //TODO: el usuario una vez logueado podria levantar su id sin pedirlo
            if (tareaDTO.UsuarioId <= 0)
                throw new NotValidIdException("El Id del usuario debe ser mayor a 0");

            //TODO: comprobar que exista usuario. Deberia agregar el repository de usuario en el service de Tarea para buscarlo?

            if (id != tareaDTO.Id)
                return BadRequest("Los id no coinciden.");

            //llamar a servicio y pasarle el id y el dto
            await _tareaService.UpdateTarea(id, tareaDTO);

            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTarea(long id)
        {
            await _tareaService.DeleteTarea(id);            
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult>  FinalizeTarea(long id)
        {
            await _tareaService.FinalizeTarea(id);
            return NoContent();
        }

    }
}
