using GestionTareas_DataAccessLayer.Models;

namespace GestionTareas_BusinessLayer.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(Usuario usuario);
    }
}
