namespace Code.Data
{
	using Code.Models;

	public class Image
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int Rating { get; set; }

		public virtual Album Album { get; set; }

		public string UserId { get; set; }

		public virtual ApplicationUser User { get; set; }
    }
}
