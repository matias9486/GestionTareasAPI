using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTareas_DataAccessLayer.Models
{
    public class Usuario
    {
        //Properties
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "{} es un campo requerido")]
        [StringLength(50, ErrorMessage = "{} no puede superar los 50 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{} es un campo requerido")]
        [StringLength(50, ErrorMessage = "{} no puede superar los 50 caracteres")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "{} es un campo requerido")]
        [StringLength(50, ErrorMessage = "{} no puede superar los 50 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{} es un campo requerido")]
        [StringLength(50, ErrorMessage = "{} no puede superar los 50 caracteres")]
        public string Password { get; set; }
        //public string Role { get; set; }

        //Relationships
        public List<Tarea> Tareas { get; set; }
    }
}
