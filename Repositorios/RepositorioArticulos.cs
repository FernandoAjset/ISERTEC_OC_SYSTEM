using Dapper;
using ISERTEC_OC_SYSTEM.Models;
using Microsoft.Data.SqlClient;

namespace ISERTEC_OC_SYSTEM.Repositorios
{
    public class RepositorioArticulos : IRepositorioArticulos
    {
        private readonly IConfiguration configuration;
        private string cadenaConexion;

        public RepositorioArticulos(IConfiguration configuration)
        {
            this.configuration = configuration;
            cadenaConexion = configuration.GetConnectionString("DefaultConnection");
        }
        public IEnumerable<Articulo> ObtenerTodos()
        {
            using var conexion = new SqlConnection(cadenaConexion);
            var articulos = conexion.Query<Articulo>("EXEC Articulo_CRUD @Accion, @Id, @Descripcion",
                new
                {
                    Accion = "T",
                    Id = 0,
                    Descripcion = ""
                }
                );
            return articulos;
        }
    }
}
