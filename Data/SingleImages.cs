using Code.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Data
{
    public class SingleImages
    {
		public int Id { get; set; }

		[Required]
		[MaxLength(20)]
		[Display(Name = "Name")]
		public string Name { get; set; }

		public int Rating { get; set; }

		[Required]
		[Display(Name = "Description")]
		public string Description { get; set; }

		public string Location { get; set; }

		[Required]
		public string Category { get; set; }

		public string Path { get; set; }

		public DateTime CreatedOn { get; set; }

		public virtual ApplicationUser User { get; set; }

    }
}
