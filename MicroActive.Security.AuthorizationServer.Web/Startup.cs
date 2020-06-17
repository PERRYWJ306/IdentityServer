using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.AccessTokenValidation;

using MicroActive.Security.AuthorizationServer.Data;
using MicroActive.Security.AuthorizationServer.Data.Models;
using MicroActive.Security.AuthorizationServer.Web.Identity;
using MicroActive.Security.AuthorizationServer.Web.Services;
using AutoMapper;

namespace MicroActive.Security.AuthorizationServer.Web
{
	public class Startup
    {
		private IHostingEnvironment _environment = null;

		public Startup(IHostingEnvironment env)
        {
			_environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			var connectionString = Configuration.GetConnectionString("DefaultConnection");
			var migrationsAssembly = typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name;

			//Identity Context
			services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connectionString));

			// Add framework services.
			services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
				//.AddIdentityServerUserClaimsPrincipalFactory()
				.AddClaimsPrincipalFactory<Identity.CustomUserClaimsFactory>()
                .AddDefaultTokenProviders();

			//services.AddSingleton(Mapper.Configuration);
			//services.AddScoped<IMapper>(sp =>
			//  new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

			services.AddAutoMapper();

			services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

			// Adds IdentityServer
			services.AddIdentityServer()
				.AddSigningCredential(LoadCertificate())
				.AddConfigurationStore(builder =>
					builder.UseSqlServer(connectionString, options =>
						options.MigrationsAssembly(migrationsAssembly)))
				.AddOperationalStore(builder =>
					builder.UseSqlServer(connectionString, options =>
						options.MigrationsAssembly(migrationsAssembly)))
//				.AddProfileService<CustomProfileService>()
				.AddAspNetIdentity<ApplicationUser>()
				
				.AddProfileService<CustomProfileService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

			InitializeDatabase(app);

			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

			// Adds IdentityServer
			app.UseIdentityServer();

			var authority = Configuration.GetSection("AppSettings")["IdentityAuthority"];

			app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
			{
				Authority = authority,
				RequireHttpsMetadata = false,
				RoleClaimType = "roleaction",
				ApiName = "api1"
			});

			app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

		X509Certificate2 LoadCertificate()
		{			
			return new X509Certificate2(
			  $"{_environment.ContentRootPath}\\bin\\idsrv3test.pfx", "idsrv3test");
		}

		private void InitializeDatabase(IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
				serviceScope.ServiceProvider.GetRequiredService<IdentityContext>().Database.Migrate();

				serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

				var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
				context.Database.Migrate();

				#region Initial DB Items
				if (!context.Clients.Any())
				{
					foreach (var client in Config.GetClients())
					{
						context.Clients.Add(client.ToEntity());
					}
					context.SaveChanges();
				}
				
				if (!context.IdentityResources.Any())
				{
					foreach (var resource in Config.GetIdentityResources())
					{
						context.IdentityResources.Add(resource.ToEntity());
					}
					context.SaveChanges();
				}

				if (!context.ApiResources.Any())
				{
					foreach (var resource in Config.GetApiResources())
					{
						context.ApiResources.Add(resource.ToEntity());
					}
					context.SaveChanges();
				}
				#endregion
			}
		}
	}
}
