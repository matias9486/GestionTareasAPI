using System.ComponentModel.DataAnnotations;

namespace GestionTareas_BusinessLayer.Dto.Tarea
{
    public class AddTareaDTO
    {
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "{} no puede superar los 50 caracteres")]
        public string Descripcion { get; set; }        
    }
}
