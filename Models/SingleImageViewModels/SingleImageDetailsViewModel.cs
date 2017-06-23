using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models.SingleImageViewModels
{
    public class SingleImageDetailsViewModel
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public int Rating { get; set; }

		public string Description { get; set; }

		public string Location { get; set; }

		public string Category { get; set; }

		public string Path { get; set; }

		public DateTime UploadedOn { get; set; }

		public virtual ApplicationUser User { get; set; }
	}
}
