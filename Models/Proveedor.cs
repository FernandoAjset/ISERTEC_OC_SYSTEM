namespace ISERTEC_OC_SYSTEM.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Proveedor
    {
        [Key]
        [MaxLength(10)]
        public string NIT { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string Direccion { get; set; }

        [MaxLength(20)]
        public string Telefono { get; set; }
    }

}
