namespace Code.Models.ProfileViewModels
{
	using System.Collections.Generic;

	public class MyProfileViewModel
    {
		public string Id { get; set; }

		public string ProfilePictureName { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public bool isAdmin { get; set; }

		public string Email { get; set; }

		public string Location { get; set; }

		public int Age { get; set; }

		public string Description { get; set; }

		public int LikesCount { get; set; }

		public int CommentsCount { get; set; }

		public List<MyAlbumViewModel> MyAlbums { get; set; }

    }
}
