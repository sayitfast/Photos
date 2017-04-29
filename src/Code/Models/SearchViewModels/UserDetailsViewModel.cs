using Code.Data;
using Code.Models.AlbumVIewModels;
using Code.Models.SingleImageViewModels;
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

		public string Email { get; set; }

		public bool isAdmin { get; set; }

		public string ProfilePictureName { get; set; }

		public int TotalAlbums { get; set; }

		public int TotalImages { get; set; }

		public int TotalLikes { get; set; }

		public int TotalComments { get; set; }

		public List<AlbumDetailsViewModel> Albums { get; set; }

		public List<SingleImageDetailsViewModel> Images { get; set; }

		public List<CommentDetailsViewModel> Comments { get; set; }
    }
}
