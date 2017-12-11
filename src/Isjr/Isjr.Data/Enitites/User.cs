using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Isjr.Data.Enitites
{
    public class User : IdentityUser<int>
	{
	    [Required]
		public string Hash { get; set; }

	    [Required]
		public string Salt { get; set; }
    }
}
