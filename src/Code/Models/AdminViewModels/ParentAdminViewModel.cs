namespace Code.Models.AdminViewModels
{
	using Code.Models.AlbumVIewModels;
	using System.Collections.Generic;
	using Code.Models.SearchViewModels;

	public class ParentAdminViewModel
    {
		public List<AlbumDetailsViewModel> Albums { get; set; }

		public List<UserDetailsViewModel> Users { get; set; }

		public List<ImageDetailsViewModel> Images { get; set; }
    }
}
