namespace Code.Models
{
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public int Age { get; set; }

		public string Location { get; set; }

		public string Description { get; set; }

		public string ProfilePicture { get; set; }

		public int ImagesCount { get; set; }

		public int AlbumsCount { get; set; }

		public int LikesCount { get; set; }

		public int CommentsCount { get; set; }

		public bool isAdmin { get; set; }
	}
}
