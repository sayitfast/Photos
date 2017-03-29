using Code.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace Code.Data
{

    public class Album
    {
		public int Id { get; set; }

		[Required]
		[MinLength(5), MaxLength(100)]
		public string Name { get; set; }

		[MaxLength(160)]
		public string Description { get; set; }

		public string UserId { get; set; }

		public virtual ApplicationUser User { get; set; }

		public DateTime CreatedOn { get; internal set; }
	}
}
