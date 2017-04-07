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


		// this is the constructor of the AlbumsController
		// it allows us the use:
		// - UserManager => when we want to get the current user using the site
		// - Database => when we want to read ot write information from the Database
		// - System Environment => allows using the FileStream class when we are
		// uploading pictures ( user profile picture and albums pictures )
		public AlbumsController(ApplicationDbContext db,
			UserManager<ApplicationUser> userManager,
			IHostingEnvironment environment)
		{
			this.db = db;
			this.userManager = userManager;
			this.environment = environment;
		}

		//GET: Returns the Create album form that will be displayed to the uses
		// who are logged in
		[Authorize]
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		//POST: Sends the information from the Create form to the Database
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
					UserId = currentUser.Id
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

		// when the user clicks on an actionlink to the details of an album the album's id will
		// be needed to find it in the Database
		public IActionResult Details(int albumId)
		{
			var model = new ParentAlbumViewModel();

			// getting the album itself, fron the Database
			model.Album = db.Album
				.Where(al => al.Id == albumId)
				.FirstOrDefault();

			// creating the AlbumDetails view model will allow us
			// to see the details of the particular album and user that
			// is it's creator
			model.AlbumDetails = new AlbumDetailsViewModel
			{
				Id = model.Album.Id,
				Name = model.Album.Name,
				CreatedOn = model.Album.CreatedOn,
				Description = model.Album.Description,
				Creator = db.Users.Where(u => u.Id == model.Album.UserId).FirstOrDefault()
			};

			// getting the images from the Database that have as albumId the Id of the album
			// the user clicked on 
			model.Images = this.db.Images
				.Where(img => img.Album == model.Album)
				.ToList();


			// gets all the comments related to the album 
			model.Comments = this.db.Comments
				.OrderByDescending(c => c.CreatedOn)
				.Where(c => c.Album.Id == albumId)
				.Select( c => new CommentDetailsViewModel
				{
					Author = c.User,
					Content = c.Content,
					CreatedOn = c.CreatedOn,
					PostedBefore = DateTime.UtcNow - c.CreatedOn				
				})
				.ToList();


			// if there isn't such album in the Database a Not Found page will appear
			if(model.Album == null)
			{
				return NotFound();
			}

			return View(model);

		}

		[Authorize]
		[HttpGet]
		public IActionResult Edit(int albumId)
		{
			var currentUser = userManager.GetUserAsync(User).Result;

			var model = new ParentAlbumViewModel();

			model.Album = this.db.Album
				.Where(al => al.Id == albumId)
				.FirstOrDefault();

			if(model.Album.User != currentUser)
			{
				return NotFound();
			}

			model.Images = this.db.Images
				.Where(img => img.Album.Id == albumId)
				.ToList();

			model.Comments = this.db.Comments
				.Where(c => c.Album.Id == albumId)
				.OrderByDescending(c => c.CreatedOn)
				.Select(c => new CommentDetailsViewModel
				{
					Id = c.Id,
					Author = c.User,
					Content = c.Content,
					CreatedOn = c.CreatedOn
				})
				.ToList();

			return View(model);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Edit(ParentAlbumViewModel model, int albumId, List<IFormFile> pictures)
		{
			if (ModelState.IsValid)
			{
				var currentUser = userManager.GetUserAsync(User).Result;

				if (model.Album != null)
				{
					var album = this.db.Album
						.Where(al => al.Id == albumId)
						.FirstOrDefault();

					album.Name = model.Album.Name;
					album.Description = model.Album.Description;

					db.Album.Add(album);
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
				}

				return RedirectToAction("Details", "Albums", new { @albumId = albumId });
			}

			return RedirectToAction("Details", "Albums", new { @albumId = albumId });
		}

		// returns the comment form in the Details view of an album
		[Authorize]
		[HttpGet]
		public IActionResult Comment(int albumId)
		{
			CreateAlbumViewModel model = new CreateAlbumViewModel();

			return View(model);
		}

		// saves the comments to the database 
		[Authorize]
		[HttpPost]
		public IActionResult Comment(ParentAlbumViewModel model, int albumId)
		{
			var currentUser = userManager.GetUserAsync(User).Result;

			var album = this.db.Album
				.Where(al => al.Id == albumId)
				.FirstOrDefault();

			Comment comment = new Comment()
			{
				Id = model.CreateComment.Id,
				Content = model.CreateComment.Content,
				User = currentUser,
				Album = album,
				CreatedOn = DateTime.UtcNow
			};

			db.Comments.Add(comment);
			db.SaveChanges();

			return RedirectToAction("Details", "Albums", new { @albumId = albumId });
		}

		public IActionResult DeleteComment(int commentId, int albumId)
		{
			var comment = this.db.Comments
				.Where(c => c.Id == commentId)
				.FirstOrDefault();

			db.Comments.Remove(comment);

			db.SaveChanges();

			return RedirectToAction("Edit", "Albums", new { @albumId = albumId });
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
