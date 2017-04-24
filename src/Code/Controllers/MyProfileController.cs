namespace Code.Controllers
{
	using System.Linq;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Authorization;
	using Models;
	using Data;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Hosting;
	using System.IO;
	using System.Threading.Tasks;
	using Models.ProfileViewModels;
	using Models.SingleImageViewModels;

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


		public IActionResult Index()
		{
			var usedId = userManager.GetUserId(User);

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
					MyImages = this.db.SingleImages
					.Where(img => img.User.Id == u.Id)
					.Take(6)
					.Select(img => new SingleImageDetailsViewModel()
					{
						Id = img.Id,
						Name = img.Name,
						Description = img.Description,
						Location = img.Location,
						Category = img.Category,
						Path = img.Path,
						Rating = img.Rating,
						UploadedOn = img.CreatedOn,
						User = u
					})
					.ToList(),
					MyAlbums = this.db.Album
					.OrderByDescending(al => al.CreatedOn)
					.Take(3)
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

					}).ToList(),
					AlbumCount = u.AlbumsCount,
					ImagesCount = u.ImagesCount,
					CommentsCount = u.CommentsCount,
					LikesCount = u.LikesCount
				})
				.FirstOrDefault();

			return View(user);
		}

		// GET: MyProfile/Edit
		[HttpGet]
		public IActionResult Edit()
		{
			var user = userManager.GetUserAsync(User);

			var current = user.Result;

			return View(current);
		}
		// POST: MyProfile/Edit
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

		// MyProfile/MyAlbumsAll?page=1
		public IActionResult MyAlbumsAll(int page = 1)
		{
			var user = userManager.GetUserAsync(User).Result;

			var pageSize = 6;

			if(user == null)
			{
				return NotFound();
			}

			var albums = this.db.Album
				.OrderByDescending( al => al.CreatedOn)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Where(al => al.UserId == user.Id)
				.Select(al => new MyAlbumViewModel()
				{
					Id = al.Id,
					Category = al.Category,
					CreatedOn = al.CreatedOn,
					Description = al.Description,
					Name = al.Name,
					AlbumImages = this.db.Images
					.Where(i => i.Album.Id == al.Id)
					.Select(i => new MyAlbumImageViewModel()
					{
						Id = i.Id,
						Name = i.Name,
						Path = $"/uploads/{al.UserId}/{al.Id}/{i.Name}"
					})
					.ToList()
				})
				.ToList();

			ViewBag.CurrentPage = page;

			return View(albums);
		}

		// MyProfile/MyImagesAll?page=1
		public IActionResult MyImagesAll(int page = 1)
		{
			var user = userManager.GetUserAsync(User).Result;

			if(user == null)
			{
				return NotFound();
			}

			var pageSize = 16;

			var images = this.db.SingleImages
				.Where(i => i.User.Id == user.Id)
				.OrderByDescending(i => i.CreatedOn)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(i => new SingleImageDetailsViewModel()
				{
					Id = i.Id,
					Name = i.Name,
					Description = i.Description,
					Category = i.Category,
					Location = i.Location,
					Path = i.Path,
					Rating = i.Rating,
					UploadedOn = i.CreatedOn,
					
				}).ToList();

			ViewBag.CurrentPage = page;

			return View(images);
		}
	}
}
