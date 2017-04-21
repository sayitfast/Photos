namespace Code.Controllers
{
	using System.Linq;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Authorization;
	using Models;
	using Data;
	using Models.AlbumVIewModels;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Hosting;
	using System.IO;
	using System.Threading.Tasks;
	using Models.ProfileViewModels;

	[Authorize]
	public class MyProfileController : Controller
	{
		private readonly ApplicationDbContext db;
		private readonly UserManager<ApplicationUser> userManager;
		private IHostingEnvironment environment;

		public MyProfileController(ApplicationDbContext db,
			UserManager<ApplicationUser> userManager,
			IHostingEnvironment environment)
		{
			this.environment = environment;
			this.db = db;
			this.userManager = userManager;
		}


		public IActionResult Index(int albumsPage = 1)
		{
			var usedId = userManager.GetUserId(User);

			int pageSize = 4;

			var user = this.db.Users
				.Where(u => u.Id == usedId)
				.Select(u => new MyProfileViewModel()
				{
					Id = u.Id,
					Age = u.Age,
					FirstName = u.FirstName,
					LastName = u.LastName,
					isAdmin = u.isAdmin,
					Description = u.Description,
					Email = u.Email,
					Location = u.Location,
					ProfilePictureName = u.ProfilePicture,
					LikesCount = this.db.Likes
					.Where(l => l.UserId == u.Id)
					.Count(),
					CommentsCount = this.db.Comments
					.Where(c => c.User.Id == u.Id)
					.Count(),
					MyAlbums = this.db.Album
					.OrderByDescending(al => al.CreatedOn)
					.Skip((albumsPage - 1) * pageSize)
					.Take(pageSize)
					.Where(al => al.UserId == u.Id)
					.Select(al => new MyAlbumViewModel()
					{
						Id = al.Id,
						Name = al.Name,
						Category = al.Category,
						CreatedOn = al.CreatedOn,
						Description = al.Description,
						AlbumImages = this.db.Images
						.Where(img => img.Album.Id == al.Id)
						.Select(img => new MyAlbumImageViewModel()
						{
							Id = img.Id,
							Name = img.Name,
							Path = $"/uploads/{u.Id}/{al.Id}/{img.Name}"

						}).ToList()

					}).ToList()
				})
				.FirstOrDefault();

			ViewBag.CurrentAlbumsPage = albumsPage;

			return View(user);
		}

		// this method will form that user will fill
		[HttpGet]
		public IActionResult Edit()
		{
			var user = userManager.GetUserAsync(User);

			var current = user.Result;

			return View(current);
		}
		// this method will sent the information to the database
		[HttpPost]
		public async Task<IActionResult> Edit(ApplicationUser user, IFormFile ProfilePictureFile)
		{
			if (ModelState.IsValid)
			{
				var currentUser = userManager.GetUserAsync(User).Result;
				currentUser.FirstName = user.FirstName;
				currentUser.LastName = user.LastName;
				currentUser.Location = user.Location;
				currentUser.Age = user.Age;
				currentUser.Description = user.Description;

				if (ProfilePictureFile != null)
				{
					string uploadPath = Path.Combine(environment.WebRootPath, "uploads");
					Directory.CreateDirectory(Path.Combine(uploadPath, currentUser.Id));

					string filename = ProfilePictureFile.FileName.Split('\\').Last();

					using (FileStream fs = new FileStream(Path.Combine(uploadPath, currentUser.Id, filename), FileMode.Create))
					{
						await ProfilePictureFile.CopyToAsync(fs);
					}

					currentUser.ProfilePicture = filename;
				}

				db.Update(currentUser);

				db.SaveChanges();

				return RedirectToAction("Index", "MyProfile");
			}

			return View("Edit");
		}
	}
}
