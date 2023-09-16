using Dapper;
using ISERTEC_OC_SYSTEM.Models;
using Microsoft.Data.SqlClient;

namespace ISERTEC_OC_SYSTEM.Repositorios
{
    public class RepositorioProveedores : IRepositorioProveedores
    {
        private readonly IConfiguration configuration;
        private string ConnectionString;

        public RepositorioProveedores(IConfiguration configuration)
        {
            this.configuration = configuration;
            ConnectionString = configuration.GetConnectionString("DefaultConnection");

        }
        public IEnumerable<Proveedor> ObtenerTodos()
        {
            using var conexion = new SqlConnection(ConnectionString);
            var proveedores = conexion.Query<Proveedor>(@"EXEC Proveedor_CRUD @Accion, @NIT, @Nombre, @Direccion, @Telefono",
                                                        new
                                                        {
                                                            Accion = "T",
                                                            NIT = 0,
                                                            Nombre = "",
                                                            Direccion = "",
                                                            Telefono = ""
                                                        }
                                                        );
            return proveedores;
        }
    }
}
