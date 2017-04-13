namespace Code.Controllers
{
	
	using System.Linq;
	using Microsoft.AspNetCore.Mvc;
	using Data;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class ImageController : Controller
    {
		private readonly ApplicationDbContext db;

		public ImageController(ApplicationDbContext db)
		{
			this.db = db;
		}
		
		public IActionResult Like(int imageId, int albumId)
		{
			var image = this.db.Images
				.Where(img => img.Id == imageId && 
				img.Album.Id == albumId)
				.FirstOrDefault();

			image.Rating++;

			var album = this.db.Album
				.Where(al => al.Id == albumId)
				.FirstOrDefault();

			var like = new Like()
			{
				Image = image,
				Album = album,
				UserId = album.UserId
			};

			db.Likes.Add(like);

			db.SaveChanges();

			return RedirectToAction("Details", "Albums", new { albumId, album.UserId });
		}

		public IActionResult Dislike(int imageId, int albumId)
		{
			var image = this.db.Images
				.Where(img => img.Id == imageId &&
				img.Album.Id == albumId)
				.FirstOrDefault();

			image.Rating--;

			var album = this.db.Album
				.Where(al => al.Id == albumId)
				.FirstOrDefault();
			

			db.SaveChanges();

			return RedirectToAction("Details", "Albums", new { albumId, album.UserId });
		}
        
    }
}
