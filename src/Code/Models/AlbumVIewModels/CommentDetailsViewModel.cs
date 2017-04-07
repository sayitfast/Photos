namespace Code.Models.AlbumVIewModels
{
	using System;

    public class CommentDetailsViewModel
    {
		public int Id { get; set; }

		public ApplicationUser Author { get; set; }

		public string Content { get; set; }

		public DateTime CreatedOn { get; set; }

		public TimeSpan PostedBefore { get; set; }
    }
}
