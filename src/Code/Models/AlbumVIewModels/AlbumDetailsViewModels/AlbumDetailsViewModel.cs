namespace Code.Models.AlbumVIewModels
{
	using System;
	using System.Collections.Generic;

	public class AlbumDetailsViewModel
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime CreatedOn { get; set; }

		public string Category { get; set; }

		public List<AlbumImageDetailsViewModel> Images { get; set; }

		public List<CommentDetailsViewModel> Comments { get; set; }

		public ApplicationUser Creator { get; set; }

		public CreateCommentViewModel PostComment { get; set; }
	}
}
