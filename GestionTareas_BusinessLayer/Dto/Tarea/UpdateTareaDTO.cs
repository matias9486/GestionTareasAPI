﻿using System.ComponentModel.DataAnnotations;

namespace GestionTareas_BusinessLayer.Dto.Tarea
{
    public class UpdateTareaDTO
    {
        [Required(ErrorMessage = "El Id de la tarea es un campo requerido")]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "{} no puede superar los 50 caracteres")]
        public string Descripcion { get; set; }

        /*
        [Required(ErrorMessage = "Es requerido indicar si la tarea esta terminada")]
        public bool? IsTerminada { get; set; }
        */
        
        [DataType(DataType.DateTime)]
        public DateTime? FechaCreacion { get; set; }

        /*
        [Required(ErrorMessage = "El Id del usuario es un campo requerido")]
        public int UsuarioId { get; set; }
        */
    }
}
