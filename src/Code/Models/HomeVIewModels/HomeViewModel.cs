using Code.Models.AlbumVIewModels;
using Code.Models.SingleImageViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models.HomeVIewModels
{
    public class HomeViewModel
    {
		public string Search { get; set; }

		public string Option { get; set; }

		public List<SingleImageDetailsViewModel> Images { get; set; }

		public List<HomeAlbumsDetailsViewModel> Albums { get; set; }
    }
}
