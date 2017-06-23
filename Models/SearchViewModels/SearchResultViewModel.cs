namespace Code.Models.SearchViewModels
{
	using AlbumVIewModels;
	using Data;
	using SingleImageViewModels;
	using System.Collections.Generic;

	public class SearchResultViewModel
    {
		public List<AlbumDetailsViewModel> Albums { get; set; }

		public List<UserDetailsViewModel> Users { get; set; }

		public List<SingleImageDetailsViewModel> Images { get; set; }
    }
}
