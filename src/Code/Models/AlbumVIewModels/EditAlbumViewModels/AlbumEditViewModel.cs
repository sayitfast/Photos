namespace Code.Models.AlbumVIewModels.EditAlbumViewModels
{
	using System.Collections.Generic;

    public class AlbumEditViewModel
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public ApplicationUser User { get; set; }

		public List<AlbumImageEditViewModel> Images { get; set; }

		public List<AlbumCommentsEditViewModel> Comments { get; set; }

    }
}
