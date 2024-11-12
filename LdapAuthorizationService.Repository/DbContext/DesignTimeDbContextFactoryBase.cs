using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LdapAuthorizationService.Repository.DbContext
{
    public abstract class DesignTimeDbContextFactoryBase<TContext> :
        IDesignTimeDbContextFactory<TContext> where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);
        
        public TContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            var connectionString = config.GetConnectionString("Postgre");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Could not find a connection string named 'Postgre'.");
            }
            return Create(connectionString);

        }
        
        private TContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException($"{nameof(connectionString)} is null or empty.",
                    nameof(connectionString));

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            Console.WriteLine("Connection string: {0}", connectionString);

            optionsBuilder.UseNpgsql(connectionString);

            var options = optionsBuilder.Options;
            return CreateNewInstance(options);
        }
    }
}