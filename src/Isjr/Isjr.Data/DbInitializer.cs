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
	    private readonly IConfiguration _configuration;
	    private readonly UserManager<User> _userManager;
	    private readonly ApplicationDbContext _dbContext;

	    private static readonly string[] Roles = {"Admin", "Moderator"};

		public DbInitializer(RoleManager<IdentityRole<int>> roleManager, UserManager<User> userManager, IConfiguration config, ApplicationDbContext dbContext)
	    {
		    _roleManager = roleManager;
		    _userManager = userManager;
			_configuration = config;
		    _dbContext = dbContext;
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

			if (await _userManager.FindByNameAsync(userName) == null)
			{
				var salt = Salt.Create();
				var hash = Hash.Create(password, salt);

				var superUser = new User
				{
					UserName = userName,
					Hash = hash,
					Salt = salt
				};

				var userResult = await _userManager.CreateAsync(superUser);

				if (userResult.Succeeded)
				{
					await _userManager.AddToRoleAsync(superUser, "Admin");
				}
			}
		}

	    public async Task Seed()
	    {
		    await CreateRoles();
		    await CreateSuperUser();
	    }

	    public async Task Migrate()
	    {
		    await _dbContext.Database.MigrateAsync();
	    }
	}
}
