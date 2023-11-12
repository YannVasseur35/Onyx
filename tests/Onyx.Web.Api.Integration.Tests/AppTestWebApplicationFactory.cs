﻿namespace Onyx.Web.Api.Integration.Tests
{
    public class AppTestWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // remove the existing context configuration
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<OnyxDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                //SQLite Db Context
                services.AddDbContext<OnyxDbContext>(options => options.UseSqlite("DataSource=RubyDatabase.db"));
            });
        }
    }
}