using Isjr.Data.Enitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Isjr.Data
{
	public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
	{
		public ApplicationDbContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<JojoReference> JojoReferences { get; set; }
		public DbSet<MultimediaType> MultimediaTypes { get; set; }
		public DbSet<MultimediaItem> MultimediaItems { get; set; }
	}
}