using ISERTEC_OC_SYSTEM.Repositorios;
using Microsoft.AspNetCore.Authentication.Cookies;
using ProyectoPrimerParcial.Servicios;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Usuario/Login";
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepositorioOrdenesDeCompra, RepositorioOrdenesDeCompra>();
builder.Services.AddScoped<IRepositorioProveedores, RepositorioProveedores>();
builder.Services.AddScoped<IRepositorioArticulos, RepositorioArticulos>();


builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}");
app.Run();
