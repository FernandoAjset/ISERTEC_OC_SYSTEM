using System.ComponentModel.DataAnnotations;

namespace ISERTEC_OC_SYSTEM.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string NombreUsuario { get; set; }

        [Required]
        public string Contrasenia { get; set; }
    }
}
