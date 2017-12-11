using System.Linq;
using System.Threading.Tasks;
using Isjr.Data.Enitites;
using Isjr.Data.Repositories;
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
	    private readonly IMultimediaTypeRepository _multimediaTypeRepository;

	    private static readonly string[] RoleNames = {"Admin", "Moderator"};
	    private static readonly string[] MultimediaTypeNames = {"Image"};

		public DbInitializer(RoleManager<IdentityRole<int>> roleManager, UserManager<User> userManager, IConfiguration config, ApplicationDbContext dbContext, IMultimediaTypeRepository multimediaTypeRepository)
	    {
		    _roleManager = roleManager;
		    _userManager = userManager;
			_configuration = config;
		    _dbContext = dbContext;
		    _multimediaTypeRepository = multimediaTypeRepository;
	    }

	    private async Task CreateRoles()
	    {
		    foreach (var roleName in RoleNames)
		    {
				var doesRoleExist = await _roleManager.RoleExistsAsync(roleName);
			    if (!doesRoleExist)
			    {
				    var role = new IdentityRole<int> { Name = roleName };
				    await _roleManager.CreateAsync(role);
			    }
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

	    private async Task CreateMultimediaTypes()
	    {
		    var existingTypes = _multimediaTypeRepository.ListAll().ToList();

			foreach (var mediaType in MultimediaTypeNames)
		    {
			    if (existingTypes.All(x => x.Name != mediaType))
			    {
				    await _multimediaTypeRepository.Add(new MultimediaType {Name = mediaType});
			    }
		    }

		    await _multimediaTypeRepository.Save();
	    }

	    public async Task Seed()
	    {
		    await CreateRoles();
		    await CreateSuperUser();
		    await CreateMultimediaTypes();
	    }

	    public async Task Migrate()
	    {
		    await _dbContext.Database.MigrateAsync();
	    }
	}
}
