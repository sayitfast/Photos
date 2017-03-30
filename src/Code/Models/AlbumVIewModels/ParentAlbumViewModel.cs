namespace Code.Models.AlbumVIewModels
{
	using System.Collections.Generic;

	public class ParentAlbumViewModel
    {
		public CreateAlbumViewModel Create { get; set; }

		public SearchAlbumsViewModel Search { get; set; }

		public AlbumDetailsViewModel Details { get; set; }

		public List<ListAlbumsViewModel> List { get; set; }
    }
}
