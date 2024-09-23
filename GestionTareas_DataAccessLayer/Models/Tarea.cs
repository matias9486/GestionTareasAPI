using System.ComponentModel.DataAnnotations;

namespace GestionTareas_DataAccessLayer.Models
{
    public class Tarea
    {
        //Properties
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage="{} es un campo requerido")]
        [StringLength(50, ErrorMessage ="{} no puede superar los 50 caracteres")]
        public string Descripcion { get; set; }
        
        [Required(ErrorMessage = "{} es un campo requerido")]
        public bool IsTerminada { get; set; }
        
        [Required(ErrorMessage = "{} es un campo requerido")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; }

        //Relationships
        public int UsuarioId { get; set; }
        public Usuario usuario { get; set; }
    }
}
