namespace Code.Models.AlbumVIewModels
{
	using System;

	public class AlbumDetailsViewModel
    {
		public string Name { get; set; }

		public int Id { get; set; }

		public string Creator { get; set; }

		public string Description { get; set; }

		public DateTime CreatedOn { get; set; }

		public int TotalImages { get; set; }
	}


}
