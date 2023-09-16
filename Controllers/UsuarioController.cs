using ISERTEC_OC_SYSTEM.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProyectoPrimerParcial.Servicios;
using System.Security.Claims;

namespace ProyectoPrimerParcial.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio usuarioService;

        public UsuarioController(IUsuarioRepositorio usuarioService)
        {
            this.usuarioService = usuarioService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            try
            {
                bool exito = await usuarioService.AutenticarUsuario(usuario);
                if (exito)
                {
                    await SetSession(usuario);
                    return RedirectToAction("Index", "OrdenDeCompra");
                }
                else
                {
                    ViewData["IsLoggedIn"] = false;
                    TempData["MostrarAlerta"] = true;
                    return View();
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            // Desautenticar al usuario
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuario");
        }


        [HttpPost]
        public async Task<IActionResult> Registro(Usuario usuario)
        {
            try
            {
                // Obtener si existe un usuario con el mismo nombre
                Usuario usuarioPorNombre = await usuarioService.ObtenerUsuarioPorNombre(usuario.NombreUsuario);
                if (usuarioPorNombre is not null)
                {
                    TempData["MostrarAlerta"] = true;
                    return View();
                }
                await usuarioService.CrearUsuario(usuario);
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        public async Task SetSession(Usuario usuario)
        {
            List<Claim> c = new()
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.NombreUsuario),
                new Claim(ClaimTypes.Sid, usuario.Id.ToString())
            };

            ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties p = new()
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
        }
    }
}
