namespace ISERTEC_OC_SYSTEM.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Articulo
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Descripcion { get; set; }
    }

}
