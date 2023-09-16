using Dapper;
using ISERTEC_OC_SYSTEM.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ISERTEC_OC_SYSTEM.Repositorios
{
    public class RepositorioOrdenesDeCompra : IRepositorioOrdenesDeCompra
    {
        private readonly IConfiguration configuration;
        private string StringConnection;

        public RepositorioOrdenesDeCompra(IConfiguration configuration)
        {
            this.configuration = configuration;
            StringConnection = configuration.GetConnectionString("DefaultConnection");
        }

        public bool AgregarOrdenDeCompra(OrdenCrearVM ordenDeCompra)
        {
            using var conexion = new SqlConnection(StringConnection);
            conexion.Open();

            using var transaction = conexion.BeginTransaction(); // Iniciar la transacción

            try
            {
                var idOrden = conexion.QueryFirstOrDefault<int>(@"EXEC OrdenCompra_CRUD @Accion,@Id,@NitProveedor,
                                                        @UsuarioId,@FechaOrden,@FechaPago,
                                                        @Terminos,@FormaPago,@Total",
                                                            new
                                                            {
                                                                Accion = "C",
                                                                Id = 0,
                                                                ordenDeCompra.Orden.NitProveedor,
                                                                ordenDeCompra.Orden.UsuarioId,
                                                                ordenDeCompra.Orden.FechaOrden,
                                                                ordenDeCompra.Orden.FechaPago,
                                                                ordenDeCompra.Orden.Terminos,
                                                                ordenDeCompra.Orden.FormaPago,
                                                                Total = 0
                                                            }, transaction: transaction);
                if (idOrden != 0)
                {
                    foreach (var detalle in ordenDeCompra.Detalles)
                    {
                        var parametersDetalle = new
                        {
                            Accion = "C",
                            OrdenId = idOrden,
                            detalle.ArticuloId,
                            detalle.Cantidad,
                            detalle.PrecioUnidad
                        };

                        // Ejecutar el SP para crear el detalle.
                        conexion.Execute(
                            "DetalleOrdenCompra_CRUD",
                            parametersDetalle,
                        commandType: CommandType.StoredProcedure,
                            transaction: transaction // Asociar la transacción con la ejecución del SP
                        );
                    }

                    transaction.Commit(); // Confirmar la transacción si todo ha ido bien
                }
                else
                {
                    transaction.Rollback(); // Deshacer la transacción si no se pudo crear la factura
                }

                return idOrden != 0;
            }
            catch (Exception ex)
            {
                // En caso de error, deshacer la transacción
                transaction.Rollback();
                throw new Exception(ex.Message, ex); // Lanzar la excepción para que se maneje en la capa superior
            }
        }

        public IEnumerable<OrdenDeCompra> ObtenerTodasLasOrdenesDeCompra()
        {
            using var conexion = new SqlConnection(StringConnection);
            var ordenes = conexion.Query<OrdenDeCompra>(@"EXEC OrdenCompra_CRUD @Accion,@Id,@NitProveedor,
                                                        @UsuarioId,@FechaOrden,@FechaPago,
                                                        @Terminos,@FormaPago,@Total",
                                                        new
                                                        {
                                                            Accion = "T",
                                                            Id = 0,
                                                            NitProveedor = "",
                                                            UsuarioId = 0,
                                                            FechaOrden = "",
                                                            FechaPago = "",
                                                            Terminos = "",
                                                            FormaPago = "",
                                                            Total = 0
                                                        }
                                                        );
            return ordenes;
        }

        public OrdenCrearVM ObtenerPorId(int Id)
        {
            using var conexion = new SqlConnection(StringConnection);
            var orden = conexion.QueryFirstOrDefault<OrdenDeCompra>(@"EXEC OrdenCompra_CRUD @Accion,@Id,@NitProveedor,
                                                        @UsuarioId,@FechaOrden,@FechaPago,
                                                        @Terminos,@FormaPago,@Total",
                                                        new
                                                        {
                                                            Accion = "R",
                                                            Id,
                                                            NitProveedor = "",
                                                            UsuarioId = 0,
                                                            FechaOrden = "",
                                                            FechaPago = "",
                                                            Terminos = "",
                                                            FormaPago = "",
                                                            Total = 0
                                                        }
                                                        );
            var detalles = conexion.Query<DetalleOrdenDeCompra>(@"EXEC DetalleOrdenCompra_CRUD @Accion,@OrdenId,@ArticuloId,
                                                        @Cantidad,@PrecioUnidad",
                                            new
                                            {
                                                Accion = "R",
                                                OrdenId = Id,
                                                ArticuloId = 0,
                                                Cantidad = 0,
                                                PrecioUnidad = 0
                                            }
                                            );
            return new OrdenCrearVM { Orden = orden, Detalles = detalles };
        }
    }
}
