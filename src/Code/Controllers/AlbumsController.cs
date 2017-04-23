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
	using Models.SearchViewModels;
	using Models.AlbumVIewModels.EditAlbumViewModels;

	public class AlbumsController : Controller
	{
		private readonly ApplicationDbContext db;
		private readonly UserManager<ApplicationUser> userManager;
		private IHostingEnvironment environment;

		public AlbumsController(ApplicationDbContext db,
			UserManager<ApplicationUser> userManager,
			IHostingEnvironment environment)
		{
			this.db = db;
			this.userManager = userManager;
			this.environment = environment;
		}

		//GET: Albums/Create
		[Authorize]
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		//POST: Albums/Create
		//Redirect: Albums/Details/{id}
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Create(CreateAlbumViewModel model, List<IFormFile> Files)
		{
			// gets the current logged in user
			var currentUser = userManager.GetUserAsync(User).Result;

			// if the user has successfully filled all the required fields
			if (ModelState.IsValid)
			{
				//creating the album itself
				var album = new Album()
				{
					Name = model.Name,
					Description = model.Description,
					CreatedOn = DateTime.UtcNow,
					User = currentUser,
					Category = model.Category
					
				};

				// saving the album to the Database
				// it is saved at this state of the method in order to user
				// this album's id when we are uploading it's pictures later on
				db.Album.Add(album);
				db.SaveChanges();

				// if the user has uploaded a picture
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

						// creating the path where the images of the album will be stored
						// we saved the album to the Database already so we can user it's Id
						// to create an unique directory
						string path = Path.Combine(environment.WebRootPath, "uploads", currentUser.Id, album.Id.ToString());

						Directory.CreateDirectory(Path.Combine(path));

						// the FileStream class that will save the image to it's directory
						using (FileStream fs = new FileStream(Path.Combine(path, filename), FileMode.Create))
						{
							await image.CopyToAsync(fs);
						}
					}
				}
			
				db.SaveChanges();

				// when everything is saved to the Database the user will be redirected automatically to the
				// details of the album if there are images
				return RedirectToAction("Index", "MyProfile");
			}

			// if the user hasn't uploaded any picture/s he will be redirected to his profile page
			return RedirectToAction("Index", "MyProfile");
		}

		// Albums/Details/{id}
		public IActionResult Details(int albumId, string userId)
		{

			var album = this.db.Album
				.Where(a => a.Id == albumId)
				.Select(a => new AlbumDetailsViewModel()
				{
					Id = a.Id,
					Name = a.Name,
					Category = a.Category,
					CreatedOn = a.CreatedOn,
					Description = a.Description,
					Creator = this.db.Users
					.Where(u => u.Id == userId)
					.FirstOrDefault(),
					Images = this.db.Images
					.Where(img => img.Album.Id == a.Id)
					.Select(img => new AlbumImageDetailsViewModel()
					{
						Id = img.Id,
						Name = img.Name,
						Rating = img.Rating,
						Path = userId + "/" + albumId + "/" + img.Name ,
						Album = a
					}).ToList(),
					Comments = this.db.Comments
					.Where(c => c.Album.Id == a.Id)
					.Select(c => new CommentDetailsViewModel()
					{
						Id = c.Id,
						Author = c.User,
						Content = c.Content,
						CreatedOn = c.CreatedOn

					}).ToList()
				})
				.FirstOrDefault();

			if(album == null)
			{
				return NotFound();
			}

			return View(album);
		}


		//GET: Albums/Edit?albumId={id}
		[Authorize]
		[HttpGet]
		public IActionResult Edit(int albumId, string userId)
		{
			var album = this.db.Album
				.Where(al => al.Id == albumId)
				.Select(al => new AlbumEditViewModel()
				{
					Id = al.Id,
					Name = al.Name,
					Description = al.Description,
					User = this.db.Users
					.Where(u => u.Id == userId)
					.FirstOrDefault(),
					Comments = this.db.Comments
					.Where(c => c.Album.Id == albumId)
					.Select(c => new AlbumCommentsEditViewModel()
					{
						Id = c.Id,
						Content = c.Content,
						CreatedOn = c.CreatedOn,
						Creator = c.User

					}).ToList(),
					Images = this.db.Images
					.Where(img => img.Album.Id == albumId)
					.Select(img => new AlbumImageEditViewModel()
					{
						Id = img.Id,
						Name = img.Name,
						Rating = img.Rating,
						Path = userId + "/" + albumId + "/" + img.Name

					})
					.ToList()
				})
				.FirstOrDefault();

			return View(album);
		}

		//POST: Albums/Edit?albumId={id}
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Edit(AlbumDetailsViewModel model, List<IFormFile> pictures, string userId)
		{
			if (ModelState.IsValid)
			{
				var album = this.db.Album
					.Where(al => al.Id == model.Id)
					.FirstOrDefault();

				album.Name = model.Name;
				album.Description = model.Description;

				db.Update(album);

				if (pictures.Capacity > 0)
				{
					foreach (var image in pictures)
					{
						string filename = image.FileName.Split('\\').Last();

						var img = new Image()
						{
							Name = filename,
							Album = album,
							User = this.db.Users
							.Where(u => u.Id == userId)
							.FirstOrDefault()
						};

						db.Images.Add(img);
					
						string path = Path.Combine(environment.WebRootPath, "uploads", userId, album.Id.ToString());

						Directory.CreateDirectory(Path.Combine(path));

						using (FileStream fs = new FileStream(Path.Combine(path, filename), FileMode.Create))
						{
							await image.CopyToAsync(fs);
						}
					}
				}

				db.SaveChanges();
			}

			return RedirectToAction("Details", "Albums", new { @albumId = model.Id, @userId = userId});
		}

		//GET: Albums/Comment?albumId={id}
		[Authorize]
		[HttpGet]
		public IActionResult Comment()
		{

			return View();
		}

		//POST: Albums/Comment?albumId={id}
		[Authorize]
		[HttpPost]
		public IActionResult Comment(AlbumDetailsViewModel model)
		{
			if(ModelState.IsValid)
			{
				var author = userManager.GetUserAsync(User).Result;

				var comment = new Comment()
				{
					Album = this.db.Album
					.Where(al => al.Id == model.Id)
					.FirstOrDefault(),
					Content = model.PostComment.Content,
					User = author,
					CreatedOn = DateTime.UtcNow.AddHours(3)
				};

				db.Comments.Add(comment);

				db.SaveChanges();

				return RedirectToAction("Details", "Albums", new { @albumId = model.Id, @userId = comment.Album.UserId });

			}

			return BadRequest();
		}

		// Album/DeleteComment?commentId={id}/albumId={id}/userId={id}
		public IActionResult DeleteComment(int commentId, int albumId, string userId)
		{
			var comment = this.db.Comments
				.Where(c => c.Id == commentId)
				.FirstOrDefault();

			db.Comments.Remove(comment);

			db.SaveChanges();

			return RedirectToAction("Edit", "Albums", new { @albumId = albumId, @userId = userId});
		}





		public IActionResult DeleteImage (int imageId, int albumId)
		{
			var image = this.db.Images
				.Where(img => img.Id == imageId)
				.FirstOrDefault();

			db.Images.Remove(image);

			db.SaveChanges();

			return RedirectToAction("Edit", "Albums", new { @albumId = albumId });

		}

		// Shows the confirm deletion page to the user
		public IActionResult Delete(int albumId)
		{

			var model = new ParentAlbumViewModel();

			// gets the album from the Database
			model.Album = this.db.Album
						   .Where(al => al.Id == albumId)
						   .FirstOrDefault();

			// if the album does not exist
			if (model.Album == null)
			{
				return NotFound();
			}

			return View(model);

		}

		// if the user confirms the deletion
		public IActionResult ConfirmedDeletion(int Id)
		{
			// gets the album from the Database
			var album = new Album();

			foreach (var albm in db.Album)
			{
				if (albm.Id == Id)
				{
					album = albm;
					break;
				}
			}
			
			// gets all images that correspond to this album
			var albumImages = this.db.Images
				.Where(img => img.Album == album)
				.ToList();


			if (albumImages.Count > 0)
			{
				// deletes all pictures that have as albumId the id of that album
				foreach (var image in albumImages)
				{
					db.Images.Remove(image);
				}
			}

			// after the deletion of the pictures, the album it self is deleted from the Database
			db.Album.Remove(album);

			db.SaveChanges();

			return RedirectToAction("Index", "MyProfile");
		}

		// admin property that show a list of all users in the Database
		public IActionResult AllUsers()
		{
			var users = db.Users.ToList();

			return View(users);
		}

		// when an admin wants to delete an user
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
				.Where(al => al.User.Id == user.Id).ToList();

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
