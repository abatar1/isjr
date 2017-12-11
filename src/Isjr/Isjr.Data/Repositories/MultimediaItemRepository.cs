using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isjr.Data.Enitites;
using Microsoft.EntityFrameworkCore;

namespace Isjr.Data.Repositories
{
    public class MultimediaItemRepository : IMultimediaItemRepository
    {
	    private readonly ApplicationDbContext _dbContext;

	    public MultimediaItemRepository(ApplicationDbContext dbContext)
	    {
		    _dbContext = dbContext;
	    }

	    public IEnumerable<MultimediaItem> ListAll()
	    {
		    return _dbContext.MultimediaItems.AsNoTracking();

	    }

	    public async Task<MultimediaItem> Get(int id)
	    {
		    return await _dbContext.MultimediaItems
			    .Where(j => j.Id == id)
			    .Include(j => j.Type)
			    .FirstOrDefaultAsync();
		}

	    private async Task<MultimediaItem> GetWithoutIncludes(int id)
	    {
		    return await _dbContext.MultimediaItems
				.Where(j => j.Id == id)
			    .FirstOrDefaultAsync();
	    }

		public async Task Delete(int id)
	    {
			var item = await GetWithoutIncludes(id);
		    _dbContext.MultimediaItems.Remove(item);
		}

	    public async Task<MultimediaItem> Add(MultimediaItem item)
	    {
			var newItem = await _dbContext.MultimediaItems.AddAsync(item);
		    return newItem.Entity;
		}

	    public async Task<MultimediaItem> Edit(MultimediaItem editedItem)
	    {
			var existedItem = await GetWithoutIncludes(editedItem.Id);
		    existedItem.Type = editedItem.Type;
		    existedItem.Name = editedItem.Name;
		    existedItem.Url = editedItem.Url;

		    _dbContext.MultimediaItems.Update(existedItem);

		    return existedItem;
		}

	    public async Task Save()
	    {
			await _dbContext.SaveChangesAsync();
		}
    }
}
