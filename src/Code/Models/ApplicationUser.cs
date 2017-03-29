
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Code.Models
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public int Age { get; set; }

		public string Location { get; set; }

		public string Description { get; set; }
	}
}
