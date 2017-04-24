namespace Code.Models.AlbumVIewModels.DeleteAlbumViewModels
{
	using System.Collections.Generic;
	using Code.Data;

	public class DeleteAlbumImagesViewModel
    {
		public int Id { get; set; }

		public Album Album { get; set; }

		public List<DeleteAlbumImagesLikesViewModel> Likes { get; set; }
    }
}
