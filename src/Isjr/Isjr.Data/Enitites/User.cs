using System.ComponentModel.DataAnnotations;

namespace Isjr.Data.Enitites
{
    public class User
    {
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

	    [Required]
		public string Hash { get; set; }

	    [Required]
		public string Salt { get; set; }
    }
}
