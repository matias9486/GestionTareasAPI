namespace GestionTareas_BusinessLayer.Exceptions
{
    public class NotFoundException : Exception
    {        
        // Constructor con un mensaje
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
