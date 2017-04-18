namespace Code.Models.AlbumVIewModels
{
	using Data;
	using SearchViewModels;
	using System.Collections.Generic;

	// combining all album classes in one parent class 
	// is used to make displaying several properties
	// of an album in one view 
	// * to use multiple models in a single view page
	public class ParentAlbumViewModel
    {
		// this property is used when the user searches for album
		// in the main page 
		public string Search { get; set; }

		// this class is used when the user is creating an album
		public CreateAlbumViewModel Create { get; set; }

		// this class is used when we want to display a list of albums
		public List<AlbumDetailsViewModel> List { get; set; }

		// this classs is used when the user wants to delete an album
		public Album Album { get; set; }

		// this class is used when the user wants to see the details and images of an album
		public AlbumDetailsViewModel AlbumDetails { get; set; }

		// this class is used to when taking the pictures corresponding to a 
		// particular user or album
		public List<ImageDetailsViewModel> Images { get; set; }
		
		// this class is used when user is creating a comment
		public CreateCommentViewModel CreateComment { get; set; }

		//this class is used when we list the comments of an album
		public CommentDetailsViewModel CommentDetails { get; set; }

		// this class is used when we list all the comments for a specific album
		public List<CommentDetailsViewModel> Comments { get; set; }

		public Like Like { get; set; }
    }
}
