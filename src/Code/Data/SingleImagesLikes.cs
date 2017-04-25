namespace Code.Data
{
	using Code.Models;

	public class SingleImagesLikes
    {
		public int Id { get; set; }

		public SingleImages Image { get; set; }

		public ApplicationUser User { get; set; }
    }
}
