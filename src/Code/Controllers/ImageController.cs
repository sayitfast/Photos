namespace Code.Controllers
{

	using System.Linq;
	using Microsoft.AspNetCore.Mvc;
	using Data;
	using Microsoft.AspNetCore.Authorization;
	using Models.SearchViewModels;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Http;
	using Models.SingleImageViewModels;
	using Microsoft.AspNetCore.Identity;
	using Models;
	using System;
	using System.IO;
	using System.Threading.Tasks;

	[Authorize]
	public class ImageController : Controller
    {
		private readonly ApplicationDbContext db;

		private readonly UserManager<ApplicationUser> userManager;

		private IHostingEnvironment environment;

		public ImageController(ApplicationDbContext db,
			UserManager<ApplicationUser> userManager,
			IHostingEnvironment environment)
		{
			this.db = db;
			this.userManager = userManager;
			this.environment = environment;
		}
		
		// For images in album
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

		// For images in album
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
        
		//GET: Image/Upload
		[Authorize]
		[HttpGet]
		public IActionResult Upload()
		{
			return View();
		}

		//POST: Image/Upload
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Upload(UploadImageViewModel image, IFormFile File)
		{

			if(File != null && ModelState.IsValid)
			{
				var user = userManager.GetUserAsync(User).Result;

				var img = new SingleImages()
				{
					Name = image.Name,
					Category = image.Category,
					Description = image.Description,
					Location = image.Location,
					User = user,
					CreatedOn = DateTime.UtcNow.AddHours(3),
					Rating = 0,
				};

				string path = Path.Combine(environment.WebRootPath, "uploads", user.Id, "images");
				Directory.CreateDirectory(Path.Combine(path));

				// the FileStream class that will save the image to it's directory
				using (FileStream fs = new FileStream(Path.Combine(path, File.FileName), FileMode.Create))
				{
					await File.CopyToAsync(fs);
				}

				img.Path = path + File.FileName;

				db.SingleImages.Add(img);
				db.SaveChanges();

				return RedirectToAction("Details", "Image", new { @imgId = img.Id });
			}

			return View(image);
		}

		// For non-album images
		public IActionResult Details(int imgId)
		{

			var image = this.db.SingleImages
				.Where(img => img.Id == imgId)
				.Select(img => new SingleImageDetailsViewModel()
				{
					Id = img.Id,
					Name = img.Name,
					Category = img.Category,
					Description = img.Description,
					Location = img.Location,
					Path = img.Path,
					Rating = img.Rating,
					UploadedOn = img.CreatedOn,
					User = img.User
				})
				.FirstOrDefault();


			return View();
		}
	}
}
