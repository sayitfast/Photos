namespace Code.Models
{
	using AlbumVIewModels;
	using Data;
	using System.Collections.Generic;

	public class ParentProfileViewModel
    {
		public ApplicationUser User { get; set; }

		public List<Album> Albums { get; set; }

		public List<Image> Images { get; set; }

		public ParentAlbumViewModel Album { get; set; }
    }
}
