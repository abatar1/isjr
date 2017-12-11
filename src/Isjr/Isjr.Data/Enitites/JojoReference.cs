using System.ComponentModel.DataAnnotations;

namespace Isjr.Data.Enitites
{
    public class JojoReference
    {
		public int Id { get; set; }
		
		[MaxLength(50)]
		[Required]
		public string Header { get; set; }

		[MaxLength(300)]
		[Required]
		public string Text { get; set; }

		public bool Accepted { get; set; }

		public MultimediaItem Original { get; set; }

		public MultimediaItem Reference { get; set; }
    }
}
