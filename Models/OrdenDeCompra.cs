using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ISERTEC_OC_SYSTEM.Models
{
    public class OrdenDeCompra
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string NitProveedor { get; set; }

        public int UsuarioId { get; set; }

        [Required]
        public DateTime FechaOrden { get; set; }

        [Required]
        public DateTime FechaPago { get; set; }

        [Required]
        public string Terminos { get; set; }

        [Required]
        [MaxLength(50)]
        public string FormaPago { get; set; }

        [Column(TypeName = "decimal(16,2)")]
        public decimal Total { get; set; }

        [ForeignKey("NitProveedor")]
        public Proveedor Proveedor { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
    }
}
