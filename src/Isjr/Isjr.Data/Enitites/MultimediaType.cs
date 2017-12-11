using System.ComponentModel.DataAnnotations;

namespace Isjr.Data.Enitites
{
    public class MultimediaType
    {
		public int Id { get; set; }

	    [Required]
		public string Name { get; set; }
    }
}
