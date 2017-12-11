using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Isjr.Data
{
	internal class TemporaryDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		private const string DbContextName = "ApplicationDbContext";

		private ApplicationDbContext Create(string basePath)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables();

			var config = builder.Build();

			var connectionString = config.GetConnectionString(DbContextName);

			if (string.IsNullOrWhiteSpace(connectionString))
			{
				throw new InvalidOperationException(
					$"Could not find a connection string named '{DbContextName}'.");
			}

			if (string.IsNullOrEmpty(connectionString))
				throw new ArgumentException(
					$"{nameof(connectionString)} is null or empty.",
					nameof(connectionString));

			var optionsBuilder =
				new DbContextOptionsBuilder<ApplicationDbContext>();

			optionsBuilder.UseNpgsql(connectionString);

			return new ApplicationDbContext(optionsBuilder.Options);
		}

		public ApplicationDbContext CreateDbContext(string[] args)
		{
			var projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new[] { @"bin\" }, StringSplitOptions.None)[0];
			return Create(projectPath);
		}
	}
}
