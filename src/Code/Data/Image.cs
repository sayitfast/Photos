using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.ComponentModel.DataAnnotations;

namespace Code.Data
{
    public class Image
    {
		public int Id { get; set; }

		[Required]
		[MinLength(5), MaxLength(20)]
		public string Name { get; set; }

		public int Rating { get; set; }

		public int Description { get; set; }

		public virtual Album Album { get; set; }

    }
}
