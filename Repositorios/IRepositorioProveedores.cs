using ISERTEC_OC_SYSTEM.Models;

namespace ISERTEC_OC_SYSTEM.Repositorios
{
    public interface IRepositorioProveedores
    {
        IEnumerable<Proveedor> ObtenerTodos();
    }
}
