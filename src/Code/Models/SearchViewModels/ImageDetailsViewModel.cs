using Code.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models.SearchViewModels
{
    public class ImageDetailsViewModel
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public Album Album { get; set; }

		public ApplicationUser User { get; set; }

		public int Rating { get; set; }

    }
}
