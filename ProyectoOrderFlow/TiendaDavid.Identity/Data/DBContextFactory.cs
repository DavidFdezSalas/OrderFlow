using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TiendaDavid.Identity.Data;

namespace TiendaDavid.Identity.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // La cadena de conexión es correcta (aunque la contraseña es muy compleja para mostrarla aquí).
            const string connectionString = "Host=localhost;Port=62094;Username=postgres;Password=qWfebxck}GDH(j!!j3u8Z+;Database=identitydb";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // ¡¡¡LÍNEA CLAVE FALTANTE!!!
            // Debes decirle a EF Core que use el proveedor Npgsql y le pasas la cadena de conexión.
            optionsBuilder.UseNpgsql(connectionString);

            // Ahora estás devolviendo las opciones correctamente configuradas.
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}