using Isjr.Data.Enitites;
using Microsoft.EntityFrameworkCore;

namespace Isjr.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<JojoReference> JojoReferences { get; set; }
		public DbSet<MultimediaType> MultimediaTypes { get; set; }
		public DbSet<MultimediaItem> MultimediaItems { get; set; }
	}
}