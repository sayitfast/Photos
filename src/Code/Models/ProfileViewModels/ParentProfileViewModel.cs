namespace Code.Models
{
	using AlbumVIewModels;

	using System.Collections.Generic;

	public class ParentProfileViewModel
    {
		public ApplicationUser User { get; set; }

		public List<ListAlbumsViewModel> Albums { get; set; }

		public ParentAlbumViewModel Album { get; set; }
    }
}
