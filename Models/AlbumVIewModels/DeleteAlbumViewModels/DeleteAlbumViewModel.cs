namespace Code.Models.AlbumVIewModels.DeleteAlbumViewModels
{
	using System.Collections.Generic;

    public class DeleteAlbumViewModel
    {
		public int Id { get; set; }

		public List<DeleteAlbumImagesViewModel> Images { get; set; }

		public List<DeleteAlbumCommentsViewModel> Comments { get; set; }
    }
}
