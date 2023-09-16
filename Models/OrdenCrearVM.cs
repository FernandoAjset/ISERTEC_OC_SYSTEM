namespace ISERTEC_OC_SYSTEM.Models
{
    public class OrdenCrearVM
    {
        public OrdenDeCompra Orden { get; set; }
        public IEnumerable<DetalleOrdenDeCompra> Detalles { get; set; }
    }
}
