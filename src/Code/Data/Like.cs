namespace Code.Data
{
	using Code;
	using Models;

	public class Like
    {
		public int Id { get; set; }

		public Image Image { get; set; }

		public Album Album { get; set; }

		public string UserId { get; set; }

		public virtual ApplicationUser User { get; set; }
    }
}
