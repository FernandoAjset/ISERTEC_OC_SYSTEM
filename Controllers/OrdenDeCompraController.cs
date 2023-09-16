using ISERTEC_OC_SYSTEM.Models;
using ISERTEC_OC_SYSTEM.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ISERTEC_OC_SYSTEM.Controllers
{
    [Authorize]
    public class OrdenDeCompraController : Controller
    {
        private readonly IRepositorioOrdenesDeCompra repositorioOrdenesDeCompra;
        private readonly IRepositorioProveedores repositorioProveedores;
        private readonly IRepositorioArticulos repositorioArticulos;

        public OrdenDeCompraController(IRepositorioOrdenesDeCompra repositorioOrdenesDeCompra,
            IRepositorioProveedores repositorioProveedores,
            IRepositorioArticulos repositorioArticulos
            )
        {
            this.repositorioOrdenesDeCompra = repositorioOrdenesDeCompra;
            this.repositorioProveedores = repositorioProveedores;
            this.repositorioArticulos = repositorioArticulos;
        }
        // GET: OrdenDeCompraController
        [HttpGet]
        public ActionResult Index()
        {
            var ordenes = repositorioOrdenesDeCompra.ObtenerTodasLasOrdenesDeCompra();
            return View(ordenes);
        }

        // GET: OrdenDeCompraController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrdenDeCompraController/Create
        [HttpGet]
        public ActionResult Create()
        {
            var proveedores = repositorioProveedores.ObtenerTodos();
            var articulos = repositorioArticulos.ObtenerTodos();

            ViewBag.Proveedores = new SelectList(proveedores, "NIT", "Nombre");
            ViewBag.articulos = new SelectList(articulos, "Id", "Descripcion");

            var modelo = new OrdenCrearVM();
            modelo.Orden = new OrdenDeCompra();
            modelo.Detalles = new List<DetalleOrdenDeCompra>();
            return View(modelo);
        }

        // POST: OrdenDeCompraController/Create
        [HttpPost]
        public ActionResult Create([FromBody] OrdenCrearVM orden)
        {
            try
            {
                // Acceder al ClaimsPrincipal actual
                var user = HttpContext.User;

                // Obtener un claim específico por su tipo
                var sidClaim = user.FindFirst(ClaimTypes.Sid);

                if (sidClaim != null)
                {
                    var userId = int.Parse(sidClaim.Value);
                    orden.Orden.UsuarioId = userId != 0 ? userId : 4;
                }
                else
                {
                    orden.Orden.UsuarioId = 4;
                }


                var operacion = repositorioOrdenesDeCompra.AgregarOrdenDeCompra(orden);
                if (!operacion)
                {
                    return RedirectToAction("Error", "Home", new { error = "No se pudo crear la factura" });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });

            }
        }

        // GET: OrdenDeCompraController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrdenDeCompraController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrdenDeCompraController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var orden = repositorioOrdenesDeCompra.ObtenerPorId(id);
                if (orden == null || orden.Orden == null || orden.Detalles == null)
                {
                    return RedirectToAction("Error", "Home", new { error = "La factura no existe" });
                }
                return View(orden);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        // POST: OrdenDeCompraController/Delete/5
        [HttpPost]
        public ActionResult Delete(OrdenCrearVM orden )
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public async Task<IActionResult> Imprimir(int id)
        {
            try
            {
                var orden = repositorioOrdenesDeCompra.ObtenerPorId(id);
                if (orden == null || orden.Orden == null || orden.Detalles == null)
                {
                    return RedirectToAction("Error", "Home", new { error = "La factura no existe" });
                }

                return View("Imprimir", orden);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }
    }
}
