using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models.SearchViewModels
{
    public class UserDetailsViewModel
    {
	    public string Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Location { get; set; }

		public int TotalAlbums { get; set; }

		public int TotalImages { get; set; }

		public int TotalLikes { get; set; }


    }
}
