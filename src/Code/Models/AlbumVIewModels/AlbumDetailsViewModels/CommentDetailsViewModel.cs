namespace Code.Models.AlbumVIewModels
{
	using Data;
	using System;

	public class CommentDetailsViewModel
    {
		public int Id { get; set; }

		public Album Album { get; set; }

		public ApplicationUser Author { get; set; }

		public string Content { get; set; }

		public DateTime CreatedOn { get; set; }

		public TimeSpan PostedBefore { get; set; }
    }
}
