using System.Collections.Generic;
using System.Threading.Tasks;
using Isjr.Data.Enitites;

namespace Isjr.Data.Repositories
{
    public interface IMultimediaTypeRepository
    {
	    IEnumerable<MultimediaType> ListAll();
	    Task<MultimediaType> Get(int id);
	    Task<MultimediaType> Add(MultimediaType item);
	    Task Save();
    }
}
