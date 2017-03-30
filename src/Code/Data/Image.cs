namespace Code.Data
{
	using Code.Models;

	public class Image
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public Album Album { get; set; }

		public virtual ApplicationUser User { get; set; }

		public string Desctiprion { get; set; }

		public int Rating { get; set; }


    }
}
