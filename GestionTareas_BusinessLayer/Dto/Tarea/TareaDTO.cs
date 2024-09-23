namespace GestionTareas_BusinessLayer.Dto.Tarea
{
    public class TareaDTO
    {
        public int Id { get; set; }        
        public string Descripcion { get; set; }       
        public bool IsTerminada { get; set; }
        public DateTime FechaCreacion { get; set; }
        
        public int UsuarioId { get; set; }
        public string Username { get; set; }
    }
}
