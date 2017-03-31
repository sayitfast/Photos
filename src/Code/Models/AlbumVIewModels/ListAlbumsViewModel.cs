namespace Code.Models.AlbumVIewModels
{
    public class ListAlbumsViewModel
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public virtual ApplicationUser Creator { get; set; }

		public int TotalImages { get; set; }
    }
}
