using Code.Models.AlbumVIewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models.HomeVIewModels
{
    public class HomeAlbumsDetailsViewModel
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public string Category { get; set; }

		public ApplicationUser User { get; set; }

		public List<AlbumImageDetailsViewModel> Images { get; set; }
    }
}
