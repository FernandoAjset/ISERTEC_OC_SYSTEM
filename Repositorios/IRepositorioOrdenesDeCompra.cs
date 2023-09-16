using ISERTEC_OC_SYSTEM.Models;

namespace ISERTEC_OC_SYSTEM.Repositorios
{
    public interface IRepositorioOrdenesDeCompra
    {
        IEnumerable<OrdenDeCompra> ObtenerTodasLasOrdenesDeCompra();
        bool AgregarOrdenDeCompra(OrdenCrearVM ordenDeCompra);
        OrdenCrearVM ObtenerPorId(int Id);
    }
}
