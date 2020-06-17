using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace MicroActive.Security.AuthorizationServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
			// Customize the ASP.NET Identity model and override the defaults if needed.
			// For example, you can rename the ASP.NET Identity table names and more.
			// Add your customizations after calling base.OnModelCreating(builder);

			builder.Entity<ApplicationTenant>()
	.Property(e => e.Id)
	.IsRequired(true)
	.HasMaxLength(450)
	.IsUnicode(false);

			builder.Entity<ApplicationTenant>()
				.Property(e => e.Id)
				.IsUnicode(false);

			builder.Entity<ApplicationTenant>()
				.Property(e => e.Name)
				.IsUnicode(false);

			builder.Entity<ApplicationTenant>()
				.Property(e => e.RegistrationNumber)
				.IsUnicode(false);

			builder.Entity<ApplicationTenant>()
				.Property(e => e.BuildingNameNo)
				.IsUnicode(false);

			builder.Entity<ApplicationTenant>()
				.Property(e => e.StreetName)
				.IsUnicode(false);

			builder.Entity<ApplicationTenant>()
				.Property(e => e.TownCity)
				.IsUnicode(false);

			builder.Entity<ApplicationTenant>()
				.Property(e => e.County)
				.IsUnicode(false);

			builder.Entity<ApplicationTenant>()
				.Property(e => e.PostalCode)
				.IsUnicode(false);

			builder.Entity<ApplicationTenant>()
				.Property(e => e.Telephone)
				.IsUnicode(false);

			builder.Entity<ApplicationTenant>()
				.Property(e => e.Fax)
				.IsUnicode(false);

			builder.Entity<ApplicationTenant>()
				.Property(e => e.Email)
				.IsUnicode(false);

			builder.Entity<ApplicationTenant>()
				.Property(e => e.Logo)
				.IsUnicode(false);
		}
    }
}
