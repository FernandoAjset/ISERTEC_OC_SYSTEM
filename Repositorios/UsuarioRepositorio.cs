using Dapper;
using ISERTEC_OC_SYSTEM.Models;
using Microsoft.Data.SqlClient;
using ProyectoPrimerParcial.Servicios;
using System.Security.Cryptography;
using System.Text;

namespace ISERTEC_OC_SYSTEM.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly string connectionString;

        public UsuarioRepositorio(
            IConfiguration configuration
            )
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> CrearUsuario(Usuario usuario)
        {
            // Cifrar la contraseña utilizando el algoritmo SHA256
            byte[] passwordHash = GenerateHash(usuario.Contrasenia);
            string passwordBase64 = Convert.ToBase64String(passwordHash);
            // Guardar el usuario
            using var connection = new SqlConnection(connectionString);
            var parameters = new { Accion = "C", usuario.Nombre, Usuario = usuario.NombreUsuario, Contrasenia = passwordBase64 };

            int usrId = await connection.ExecuteAsync("Usuario_CRUD", parameters, commandType: System.Data.CommandType.StoredProcedure);

            return usrId > 0;
        }

        public async Task<bool> AutenticarUsuario(Usuario usuario)
        {
            using var connection = new SqlConnection(connectionString);
            var parameters = new { Accion = "R", Nombre="", Usuario = usuario.NombreUsuario, usuario.Contrasenia };

            var usuarios = await connection.QueryAsync<Usuario>("Usuario_CRUD", parameters, commandType: System.Data.CommandType.StoredProcedure);

            var usuarioEncontrado = usuarios.FirstOrDefault();

            if (usuarioEncontrado != null)
            {
                // Cifrar la contraseña enviada desde el frontend utilizando.
                byte[] passwordHashToCheck = GenerateHash(usuario.Contrasenia);
                // Comparar los dos hashes cifrados
                bool autenticado = CompareHashes(passwordHashToCheck, Convert.FromBase64String(usuarioEncontrado.Contrasenia));
                return autenticado;
            }
            return false;
        }

        public async Task<Usuario> ObtenerUsuarioPorNombre(string nombreUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            var parameters = new
            {
                Accion = "A",
                Nombre = "",
                Usuario = nombreUsuario,
                Contrasenia = ""

            };

            return await connection.QueryFirstOrDefaultAsync<Usuario>("Usuario_CRUD", parameters, commandType: System.Data.CommandType.StoredProcedure);
        }

        // MÉTODOS ADICIONALES DEL SERVICIO PARA MANEJO DE CIFRADO DE CONTRASEÑA
        // Método para generar el hash de la contraseña usando el algoritmo SHA256 y el salt proporcionado
        private byte[] GenerateHash(string contraseña)
        {
            using var sha256 = SHA256.Create();
            // Concatenar la contraseña y el salt
            byte[] passwordBytes = Encoding.UTF8.GetBytes(contraseña);
            byte[] passwordWithSaltBytes = new byte[passwordBytes.Length];
            passwordBytes.CopyTo(passwordWithSaltBytes, 0);
            // Generar el hash utilizando SHA256
            byte[] hash = sha256.ComputeHash(passwordWithSaltBytes);

            return hash;
        }

        // Método para comparar dos hashes
        private static bool CompareHashes(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
                return false;

            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                    return false;
            }

            return true;
        }
    }

}
