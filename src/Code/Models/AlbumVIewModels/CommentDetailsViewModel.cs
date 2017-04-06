namespace Code.Models.AlbumVIewModels
{
	using System;

    public class CommentDetailsViewModel
    {
		public ApplicationUser Author { get; set; }

		public string Content { get; set; }

		public DateTime CreatedOn { get; set; }
    }
}
