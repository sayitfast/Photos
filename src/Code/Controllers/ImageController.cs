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
	using Microsoft.AspNetCore.Hosting.Server;

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
		[Authorize]
		public IActionResult Like(int imageId, int albumId, string userId)
		{
			var currentUser = userManager.GetUserAsync(User).Result;

			var image = this.db.Images
				.Where(img => img.Id == imageId &&
				img.Album.Id == albumId)
				.FirstOrDefault();

			image.Rating++;

			var like = new Like()
			{
				Image = image,
				Album = this.db.Album
				.Where(al => al.Id == albumId)
				.FirstOrDefault(),
				UserId = currentUser.Id
			};

			db.Likes.Add(like);

			currentUser.LikesCount++;

			db.Update(currentUser);

			db.SaveChanges();

			return RedirectToAction("Details", "Albums", new { albumId, userId });
		}

		// For images in album
		[Authorize]
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


		// For single images
		[Authorize]
		public IActionResult LikeImage(int imageId)
		{
			var image = this.db.SingleImages
				.Where(img => img.Id == imageId)
				.FirstOrDefault();

			if (image == null)
			{
				return NotFound();
			}

			var currentUser = userManager.GetUserAsync(User).Result;

			var like = new SingleImagesLikes
			{
				User = currentUser,
				Image = image
			};

			db.SingleImagesLikes.Add(like);

			currentUser.LikesCount++;

			image.Rating++;

			db.Update(image);

			db.Update(currentUser);

			db.SaveChanges();

			return RedirectToAction("Details", "Image", new { @imageId = imageId});
		}

		// For single images
		[Authorize]
		public IActionResult DislikeImage(int imageId)
		{
			var image = this.db.SingleImages
				.Where(img => img.Id == imageId)
				.FirstOrDefault();

			if (image == null)
			{
				return NotFound();
			}

			image.Rating--;

			db.Update(image);

			db.SaveChanges();

			return RedirectToAction("Details", "Image", new { @imageId = imageId });
		}


		// For single images
		[Authorize]
		public IActionResult DeleteImage(int imageId, string userId)
		{
			var currentUser = userManager.GetUserAsync(User).Result;

			var user = this.db.Users
				.Where(u => u.Id == userId)
				.FirstOrDefault();

			if(userId != currentUser.Id)
			{
				return BadRequest();
			}

			var image = this.db.SingleImages
				.Where(img => img.Id == imageId)
				.FirstOrDefault();

			if(image == null)
			{
				return NotFound();
			}

			currentUser.ImagesCount--;

			db.Update(currentUser);

			var imageLikes = this.db.SingleImagesLikes
				.Where(l => l.Image.Id == imageId)
				.Select(l => new SingleImagesLikes()
				{
					Id = l.Id,
					Image = l.Image,
					User = l.User
				})
				.ToList();

			if(imageLikes.Count() > 0)
			{
				foreach(var like in imageLikes)
				{
					var likeUser = this.db.Users
						.Where(u => u.Id == like.User.Id)
						.FirstOrDefault();

					likeUser.LikesCount--;

					db.SingleImagesLikes.Remove(like);
				}
			}

			db.SingleImages.Remove(image);

			db.SaveChanges();

			string ownerId = image.Path.Split('/').First();
			string filename = image.Path.Split('/').Last();

			string path = Path.Combine(environment.WebRootPath, "uploads", ownerId, "images", filename);

			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}

			return RedirectToAction("Index", "MyProfile");
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

			if (File != null && ModelState.IsValid)
			{
				var user = userManager.GetUserAsync(User).Result;

				if(image.Description == null)
				{
					image.Description = string.Empty;
				}

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

				img.Path = user.Id + "/images/" + File.FileName;

				user.ImagesCount++;

				db.Update(user);

				db.SingleImages.Add(img);
				db.SaveChanges();

				return RedirectToAction("Index", "MyProfile");
			}

			return View(image);
		}

		// For non-album images
		public IActionResult Details(int imageId)
		{

			var image = this.db.SingleImages
				.Where(img => img.Id == imageId)
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


			return View(image);
		}
	}
}
