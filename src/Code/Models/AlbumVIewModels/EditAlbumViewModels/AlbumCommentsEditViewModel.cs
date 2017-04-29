namespace Code.Models.AlbumVIewModels.EditAlbumViewModels
{
	using System;

    public class AlbumCommentsEditViewModel
    {
		public int Id { get; set; }

		public string Content { get; set; }

		public DateTime CreatedOn { get; set; }

		public ApplicationUser Creator { get; set; }
    }
}
