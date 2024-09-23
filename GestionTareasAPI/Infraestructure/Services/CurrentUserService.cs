using GestionTareas_BusinessLayer.Interfaces;
using System.Security.Claims;

namespace GestionTareasAPI.Infraestructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public int? UserId { get; }

        public string UserName { get; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = Convert.ToInt32(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));
            UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            Console.WriteLine("Name: " + UserName + " nameIdentifier:" + UserId);
        }
    }
}
