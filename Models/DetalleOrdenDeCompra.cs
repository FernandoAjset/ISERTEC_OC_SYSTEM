using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ISERTEC_OC_SYSTEM.Models
{
    public class DetalleOrdenDeCompra
    {
        public int Id { get; set; }

        public int OrdenId { get; set; }

        public int ArticuloId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]  
        [Column(TypeName = "decimal(16,2)")]
        public decimal PrecioUnidad { get; set; }

        [ForeignKey("OrdenId")]
        public OrdenDeCompra OrdenDeCompra { get; set; }

        [ForeignKey("ArticuloId")]
        public Articulo Articulo { get; set; }
        public string Descripcion { get; set; }
    }
}
