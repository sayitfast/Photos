namespace Code.Models.AlbumVIewModels
{
	using Data;
	using System.Collections.Generic;

	public class ParentAlbumViewModel
    {
		public CreateAlbumViewModel Create { get; set; }

		public SearchAlbumsViewModel Search { get; set; }

		public List<ListAlbumsViewModel> List { get; set; }

		public Album Album { get; set; }

		public List<Image> Images { get; set; }
    }
}
