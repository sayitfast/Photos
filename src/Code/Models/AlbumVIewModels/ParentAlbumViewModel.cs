namespace Code.Models.AlbumVIewModels
{
	using Data;
	using System.Collections.Generic;

	// combining all album classes in one parent class 
	// is used to make displaying several properties
	// of an album in one view 
	// * to use multiple models in a single view page
	public class ParentAlbumViewModel
    {
		// this class is used when the user is creating an album
		public CreateAlbumViewModel Create { get; set; }

		// this class is used when the user is searching through the albums
		public SearchAlbumsViewModel Search { get; set; }

		// this class is used when we want to display a list of albums
		public List<AlbumDetailsViewModel> List { get; set; }

		// this classs is used when the user wants to delete an album
		public Album Album { get; set; }

		// this class is used when the user wants to see the details and images of an album
		public AlbumDetailsViewModel AlbumDetails { get; set; }

		// this class is used to when taking the pictures corresponding to a 
		// particular user or album
		public List<Image> Images { get; set; }
    }
}
