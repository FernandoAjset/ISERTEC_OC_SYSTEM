using ISERTEC_OC_SYSTEM.Models;

namespace ISERTEC_OC_SYSTEM.Repositorios
{
    public interface IRepositorioArticulos
    {
        IEnumerable<Articulo> ObtenerTodos();
    }
}
