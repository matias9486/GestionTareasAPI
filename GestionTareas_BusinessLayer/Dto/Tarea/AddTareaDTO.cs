using System.ComponentModel.DataAnnotations;

namespace GestionTareas_BusinessLayer.Dto.Tarea
{
    public class AddTareaDTO
    {
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "{} no puede superar los 50 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Es requerido indicar si la tarea esta terminada")]
        public bool? IsTerminada { get; set; }

        [Required(ErrorMessage = "Fecha creación es un campo requerido")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaCreacion { get; set; }

        [Required(ErrorMessage = "El Id del usuario es un campo requerido")]        
        public long UsuarioId { get; set; }
    }
}
