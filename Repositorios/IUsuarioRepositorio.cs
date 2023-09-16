using ISERTEC_OC_SYSTEM.Models;

namespace ProyectoPrimerParcial.Servicios
{
    public interface IUsuarioRepositorio
    {
        public Task<bool> AutenticarUsuario(Usuario usuario);
        public Task<bool> CrearUsuario(Usuario usuario);
        Task<Usuario> ObtenerUsuarioPorNombre(string nombreUsuario);
    }

}
