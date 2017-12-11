using System.Threading.Tasks;

namespace Isjr.Data
{
    public interface IDbInitializer
    {
	    Task Seed();
	    Task Migrate();
    }
}
