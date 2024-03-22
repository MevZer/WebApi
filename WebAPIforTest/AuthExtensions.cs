using Microsoft.EntityFrameworkCore;
using Npgsql;



namespace WebAPIforTest
{
    public static class AuthExtensions 
    {
        public static void AuthDefault(WebApplicationBuilder builder)
        {
            if (builder.Configuration["database:Username"] != null)
            {
                dbVault(builder);
            }
            AuthDb(builder);
        }

        public static void AuthDbVault(WebApplicationBuilder builder)
        {
                dbVault(builder);
                AuthDb(builder);

        }

        public static void AuthDb(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        }

        private static void dbVault(WebApplicationBuilder builder)
        {
            var dbBuilder = new NpgsqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));

            dbBuilder.Username = builder.Configuration["database:Username"];
            dbBuilder.Password = builder.Configuration["database:Password"];

            builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"] = dbBuilder.ConnectionString;
        }


    }
}
