namespace Code.Models.AlbumVIewModels
{
	using Code.Data;

	public class AlbumImageDetailsViewModel
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public int Rating { get; set; }

		public  Album Album { get; set; }

		public string Path { get; set; }

	}
}
