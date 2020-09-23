using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Solo.Common.Attributes;
using Solo.Domain;
using Solo.Domain.Entities;
using Solo.Domain.Map;

namespace Solo.Data.Infrastructure
{
    public class SoloDbContext : DbContext
    {
        public const string ConnectionStringName = "SoloSqlServerConnectionString";

        public DbSet<User> Users { get; set; }
        public DbSet<Park> Parks { get; set; }
        public DbSet<ParkObject> ParkObjects { get; set; }

        public SoloDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeForeignKeys = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var foreignKey in cascadeForeignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<ParkObject>().OwnsOne(o => o.Location);

            var decimalPropertiesWithPrecisionAttribute =
                modelBuilder.Model.GetEntityTypes()
                    .SelectMany(t => t.GetProperties())
                    .Where(p => p.ClrType == typeof(decimal) && p.PropertyInfo.GetCustomAttribute<DecimalPrecisionAttribute>() != null).ToArray();

            foreach (var decimalProperty in decimalPropertiesWithPrecisionAttribute)
            {
                var decimalPrecisionAttribute = decimalProperty.PropertyInfo.GetCustomAttribute<DecimalPrecisionAttribute>();
                decimalProperty.SetColumnType($"decimal({decimalPrecisionAttribute.Precision},{decimalPrecisionAttribute.Scale})");
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}