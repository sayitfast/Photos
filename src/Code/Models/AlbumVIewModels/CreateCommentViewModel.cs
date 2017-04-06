using Code.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Code.Models.AlbumVIewModels
{
    public class CreateCommentViewModel
    {

		public int Id { get; set; }

		[Required]
		[MinLength(10), MaxLength(160)]
		public string Content { get; set; }

		public ApplicationUser User { get; set; }

		public Album Album { get; set; }
    }
}
