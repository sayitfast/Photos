namespace Code.Models.ProfileViewModels
{
	using System;
	using System.Collections.Generic;

	public class MyAlbumViewModel
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Category { get; set; }

		public DateTime CreatedOn { get; set; }

		public ApplicationUser User { get; set; }

		public List<MyAlbumImageViewModel> AlbumImages { get; set; }
    }
}
