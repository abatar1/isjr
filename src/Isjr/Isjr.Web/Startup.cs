using System.Threading.Tasks;
using Isjr.Data;
using Isjr.Data.Enitites;
using Isjr.Web.ErrorHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Isjr.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
			var builder = new ConfigurationBuilder()
		        .SetBasePath(env.ContentRootPath)
		        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
		        .AddEnvironmentVariables();
	        Configuration = builder.Build();
		}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
	        services.AddMvc().AddMvcOptions(o => { o.Filters.Add(new GlobalExceptionFilter()); });

			var sqlConnectionString = Configuration.GetConnectionString("ApplicationDbContext");
			services.AddDbContext<ApplicationDbContext>(options =>
		        options.UseNpgsql(
			        sqlConnectionString,
			        b => b.MigrationsAssembly("Isjr.Data")
		        )
	        );

	        services.AddIdentity<User, IdentityRole<int>>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
		        .AddDefaultTokenProviders();

			services.AddScoped<RoleManager<IdentityRole<int>>>();
	        services.AddScoped<UserManager<User>>();

	        services.AddScoped<IDbInitializer, DbInitializer>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
	  
			await InitializeDatabase(app);
		}

	    private static async Task InitializeDatabase(IApplicationBuilder app)
	    {
		    using (var serviceScope = app.ApplicationServices.CreateScope())
		    {
			    var dbInitializer = serviceScope.ServiceProvider.GetService<IDbInitializer>();		   
			    await dbInitializer.Migrate();
			    await dbInitializer.Seed();
		    }
	    }
	}
}
