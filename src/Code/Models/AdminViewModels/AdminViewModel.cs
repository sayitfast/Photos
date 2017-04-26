namespace Code.Models.AdminViewModels
{
	using Code.Models.AlbumVIewModels;
	using System.Collections.Generic;
	using Code.Models.SearchViewModels;

	public class AdminViewModel
    {
		public List<AlbumDetailsViewModel> Albums { get; set; }

		public List<UserDetailsViewModel> Users { get; set; }
    }
}
