using System.Collections.Generic;
using System.Threading.Tasks;

namespace Isjr.Data.Repositories
{
    public interface IRepository<T>
    {
	    IEnumerable<T> ListAll();
	    Task<T> Get(int id);
		Task Delete(int id);
		Task<T> Add(T item);
		Task<T> Edit(T item);		
	    Task Save();
    }
}
