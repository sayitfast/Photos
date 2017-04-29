namespace Code.Models.AlbumVIewModels
{
	using Code.Data;
	using System.ComponentModel.DataAnnotations;

	public class CreateCommentViewModel
    {

		public int Id { get; set; }

		[Required]
		[MinLength(1), MaxLength(160)]
		public string Content { get; set; }

		public ApplicationUser User { get; set; }

		public Album Album { get; set; }
    }
}
