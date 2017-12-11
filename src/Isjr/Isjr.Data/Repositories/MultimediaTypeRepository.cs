using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Isjr.Data.Enitites;
using Microsoft.EntityFrameworkCore;

namespace Isjr.Data.Repositories
{
    public class MultimediaTypeRepository : IMultimediaTypeRepository
    {
	    private readonly ApplicationDbContext _dbContext;

	    public MultimediaTypeRepository(ApplicationDbContext dbContext)
	    {
		    _dbContext = dbContext;
	    }

	    public IEnumerable<MultimediaType> ListAll()
	    {
		    return _dbContext.MultimediaTypes.AsNoTracking();
	    }

	    public async Task<MultimediaType> Get(int id)
	    {
		    return await _dbContext.MultimediaTypes.FirstOrDefaultAsync();
	    }

	    public async Task<MultimediaType> Add(MultimediaType item)
	    {
		    var newItem = await _dbContext.MultimediaTypes.AddAsync(item);
		    return newItem.Entity;
	    }

	    public async Task Save()
	    {
			await _dbContext.SaveChangesAsync();
		}	    
	}
}
