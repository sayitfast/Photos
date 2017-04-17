namespace Code.Models.AlbumVIewModels
{
	using System;

	public class AlbumDetailsViewModel
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public ApplicationUser Creator { get; set; }

		public string Description { get; set; }

		public DateTime CreatedOn { get; set; }

		public int TotalImages { get; set; }

		public string Category { get; set; }

		public string CoverImage { get; set; }
	}


}
