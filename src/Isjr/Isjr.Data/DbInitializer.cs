using System.Linq;
using System.Threading.Tasks;
using Isjr.Data.Enitites;
using Isjr.Data.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Isjr.Data
{
    public class DbInitializer : IDbInitializer
    {
	    private readonly RoleManager<IdentityRole<int>> _roleManager;
	    private readonly ApplicationDbContext _context;
	    private readonly IConfiguration _configuration;

	    private static readonly string[] Roles = {"Admin", "Moderator"};

		public DbInitializer(RoleManager<IdentityRole<int>> roleManager, ApplicationDbContext context, IConfiguration config)
	    {
		    _roleManager = roleManager;
		    _context = context;
		    _configuration = config;
	    }

	    private async Task CreateRole(string roleName)
	    {
			var doesRoleExist = await _roleManager.RoleExistsAsync(roleName);
		    if (!doesRoleExist)
		    {
			    var role = new IdentityRole<int> { Name = roleName };
			    await _roleManager.CreateAsync(role);
		    }
		}

	    private async Task CreateRoles()
	    {
		    foreach (var role in Roles)
		    {
			     await CreateRole(role);
		    }
		}

	    private async Task CreateSuperUser()
	    {
		    var userName = _configuration["SuperUserName"];
		    if (string.IsNullOrEmpty(userName))
			{ 
			    throw new System.Exception("Use secrets manager to set SuperUserName \n" +
										   "dotnet user-secrets set SuperUserName <name>");
		    }

			var password = _configuration["SuperUserPassword"];
		    if (string.IsNullOrEmpty(userName))
		    {
			    throw new System.Exception("Use secrets manager to set SuperUserPassword \n" +
										   "dotnet user-secrets set SuperUserPassword <pw>");
		    }

			if (!_context.Users.Any(u => u.Name == userName))
			{
				var salt = Salt.Create();
				var hash = Hash.Create(password, salt);

				await _context.Users.AddAsync(new User
				{
					Name = userName,
					Hash = hash,
					Salt = salt
				});

				_context.SaveChanges();
			}
		}

	    public async Task Seed()
	    {
		    await CreateRoles();
		    await CreateSuperUser();
	    }

	    public async Task Migrate()
	    {
		    await _context.Database.MigrateAsync();
	    }
	}
}
