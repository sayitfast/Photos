namespace Code.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Authorization;
	using Models.AlbumVIewModels;
	using Data;
	using System;
	using Models;
	using System.Linq;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Http;
	using System.IO;
	using Microsoft.AspNetCore.Hosting;
	using System.Threading.Tasks;
	using System.Collections.Generic;

	public class AlbumsController : Controller
	{
		private readonly ApplicationDbContext db;
		private readonly UserManager<ApplicationUser> userManager;
		private IHostingEnvironment environment;

		public Task ProfilePictureFile { get; private set; }

		public AlbumsController(ApplicationDbContext db,
			UserManager<ApplicationUser> userManager,
			IHostingEnvironment environment)
		{
			this.db = db;
			this.userManager = userManager;
			this.environment = environment;
		}

		//GET: Create method
		[Authorize]
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		//POST: Create method
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Create(CreateAlbumViewModel model, List<IFormFile> Files)
		{
			var currentUser = userManager.GetUserAsync(User).Result;

			if (ModelState.IsValid)
			{
				var album = new Album()
				{
					Name = model.Name,
					Description = model.Description,
					CreatedOn = DateTime.UtcNow,
					UserId = currentUser.Id
				};

				db.Album.Add(album);
				db.SaveChanges();

				if (Files.Capacity > 0)
				{
					foreach (var image in Files)
					{
						string filename = image.FileName.Split('\\').Last();
						var img = new Image()
						{
							Name = filename,
							Album = album,
							User = currentUser
						};

						db.Images.Add(img);

						string path = Path.Combine(environment.WebRootPath, "uploads", currentUser.Id, album.Id.ToString());
						Directory.CreateDirectory(Path.Combine(path));

						using (FileStream fs = new FileStream(Path.Combine(path, filename), FileMode.Create))
						{
							await image.CopyToAsync(fs);
						}
					}
				}
			
				db.SaveChanges();

				return RedirectToAction("Details", new { album.Id });
			}

			return RedirectToAction("Index", "MyProfile");
		}

		public IActionResult Details(int albumId, string userId)
		{
			var model = new ParentAlbumViewModel();

			model.Album = this.db.Album
				.Where(al => al.Id == albumId && al.UserId == userId)
				.FirstOrDefault();

			model.Images = this.db.Images
				.Where(img => img.Album == model.Album)
				.ToList();


			if(model.Album == null)
			{
				return NotFound();
			}

			return View(model);

		}

		public IActionResult Search(ParentAlbumViewModel model)
		{
			var input = model.Search.Name;

			var resultAlbumList = this.db.Album
				.Where(al => al.Name == input)
				.Select(al => new ListAlbumsViewModel
				{
					Id = al.Id,
					Name = al.Name,
				})
				.ToList();

			model.List = resultAlbumList;

			return View(model);
		}

		public IActionResult Delete(int Id)
		{
			var model = new ParentAlbumViewModel();

			model.Album = this.db.Album
						   .Where(al => al.Id == Id)
						   .FirstOrDefault();


			if (userManager.GetUserId(User) != model.Album.User.Id)
			{
				return RedirectToAction("Index", "Home");
			}

			if (model.Album == null)
			{
				return NotFound();
			}

			return View(model);

		}

		public IActionResult ConfirmedDeletion(int Id)
		{
			var album = new Album();

			foreach (var albm in db.Album)
			{
				if (albm.Id == Id)
				{
					album = albm;
					break;
				}
			}

			var albumImages = this.db.Images
				.Where(img => img.Album == album)
				.ToList();


			if (albumImages.Count > 0)
			{
				foreach (var image in albumImages)
				{
					db.Images.Remove(image);
				}
			}

			db.Album.Remove(album);

			db.SaveChanges();

			return RedirectToAction("Index", "MyProfile", new { userId = userManager.GetUserId(User) });
		}

		public IActionResult AllUsers()
		{
			var users = db.Users.ToList();

			return View(users);
		}

		public IActionResult DeleteUser(string Id)
		{

		var user = new ApplicationUser();

			foreach (var profile in db.Users)
			{
				if (profile.Id == Id)
				{
					user = profile;
					break;
				}
			}
			db.Users.Remove(user);

			var UserAlbums = this.db.Album
				.Where(al => al.UserId == user.Id).ToList();

			if (UserAlbums.Count > 0)
			{
				for (int index = 0; index < UserAlbums.Count; index++)
				{
					db.Album.Remove(UserAlbums[index]);
					db.SaveChanges();
				}
			}

			db.SaveChanges();

			return RedirectToAction("AllUsers");
		}
	}
}
