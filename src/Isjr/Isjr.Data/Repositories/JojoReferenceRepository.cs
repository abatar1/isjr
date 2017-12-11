using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isjr.Data.Enitites;
using Microsoft.EntityFrameworkCore;

namespace Isjr.Data.Repositories
{
    public class JojoReferenceRepository : IJojoReferenceRepository
    {
	    private readonly ApplicationDbContext _dbContext;

	    public JojoReferenceRepository(ApplicationDbContext dbContext)
	    {
		    _dbContext = dbContext;
	    }

	    public IEnumerable<JojoReference> ListAll()
	    {
		    return _dbContext.JojoReferences.AsNoTracking();
	    }

	    public async Task<JojoReference> Get(int id)
	    {
		    return await _dbContext.JojoReferences
				.Where(j => j.Id == id)
				.Include(j => j.Original)
				.Include(j => j.Reference)
			    .FirstOrDefaultAsync();
	    }

	    private async Task<JojoReference> GetWithoutIncludes(int id)
	    {
		    return await _dbContext.JojoReferences
			    .Where(j => j.Id == id)
			    .FirstOrDefaultAsync();
	    }

		public async Task Delete(int id)
	    {
		    var item = await GetWithoutIncludes(id);
		    _dbContext.JojoReferences.Remove(item);
	    }

	    public async Task<JojoReference> Add(JojoReference item)
	    {
			var newItem = await _dbContext.JojoReferences.AddAsync(item);
		    return newItem.Entity;
	    }

	    public async Task<JojoReference> Edit(JojoReference editedItem)
	    {
		    var existedItem = await GetWithoutIncludes(editedItem.Id);
		    existedItem.Accepted = editedItem.Accepted;
		    existedItem.Original = editedItem.Original;
		    existedItem.Reference = editedItem.Reference;
		    existedItem.Header = editedItem.Header;
		    existedItem.Text = editedItem.Text;

		    _dbContext.JojoReferences.Update(existedItem);

		    return existedItem;
	    }

	    public async Task Save()
	    {
		    await _dbContext.SaveChangesAsync();
	    }
    }
}
