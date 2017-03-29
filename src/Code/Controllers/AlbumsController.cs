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

	public class AlbumsController : Controller
	{
		private readonly ApplicationDbContext db;
		private readonly UserManager<ApplicationUser> userManager;

		public AlbumsController(ApplicationDbContext db,
			UserManager<ApplicationUser> userManager)
		{
			this.db = db;
			this.userManager = userManager;
		}

		[Authorize]
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[Authorize]
		[HttpPost]
		public IActionResult Create(ParentAlbumViewModel model)
		{
			if (ModelState.IsValid)
			{
				var userId = userManager.GetUserId(User);
				var album = new Album
				{
					Name = model.Create.Name,
					Description = model.Create.Description,
					CreatedOn = DateTime.UtcNow,
					UserId = userId
				};
				db.Album.Add(album);
				db.SaveChanges();
				return this.RedirectToAction(c => c.Details(album.Id));
			}
			return View(model);
		}

		public IActionResult Details(int Id)
		{
			var model = new ParentAlbumViewModel();

			var album = this.db
				.Album
				.Where(al => al.Id == Id)
				.Select(al => new AlbumDetailsViewModel
				{
					Name = al.Name,
					Id = al.Id,
					Creator = al.User.UserName,
					CreatedOn = al.CreatedOn,
					Description = al.Description,
					TotalImages = 0
				})
				.FirstOrDefault();

			if (album == null)
			{
				return NotFound();
			}

			model.Details = album;

			return View(model);
		}

		public IActionResult MyAlbums()
		{
			var model = new ParentAlbumViewModel();

			var albums = this.db.Album
				.OrderByDescending(al => al.CreatedOn)
				.Where(al => al.User.UserName == User.Identity.Name)
				.Select(al => new ListAlbumsViewModel
				{
					Id = al.Id,
					Name = al.Name,
					Creator = User.Identity.Name,
				})
				.ToList();

			model.List = albums;

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
			 model.Details = this.db
							.Album
							.Where(al => al.Id == Id)
							.Select(al => new AlbumDetailsViewModel
							{
								Name = al.Name,
								Id = al.Id,
								Creator = al.User.UserName,
								CreatedOn = al.CreatedOn,
								Description = al.Description,
								TotalImages = 0
							})
							.FirstOrDefault();

			if(userManager.GetUserName(User) != model.Details.Creator)
			{
				return RedirectToAction("Index", "Home");
			}

			if(model.Details == null)
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
				if(albm.Id == Id)
				{
					album = albm;
					break;
				}
			}
			db.Album.Remove(album);
			db.SaveChanges();

			return RedirectToAction("Index", "MyProfile", new { userId = userManager.GetUserId(User)});
		}

		public IActionResult AllUsers()
		{
			var users = this.db.Users.ToList();

			return View(users);
		}

		public IActionResult DeleteUser(string Id)
		{
			var user = new ApplicationUser();

			foreach(var profile in db.Users)
			{
				if(profile.Id == Id)
				{
					user = profile;
					break;
				}
			}
			db.Users.Remove(user);

			var UserAlbums = this.db.Album
				.Where(al => al.UserId == user.Id).ToList();

			if(UserAlbums.Count > 0)
			{
				for(int index = 0; index < UserAlbums.Count; index++)
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
