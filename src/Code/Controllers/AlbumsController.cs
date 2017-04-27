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
	using Models.AlbumVIewModels.DeleteAlbumViewModels;

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
			
			var currentUser = userManager.GetUserAsync(User).Result;

			if (ModelState.IsValid)
			{
				var album = new Album()
				{
					Name = model.Name,
					Description = model.Description,
					CreatedOn = DateTime.UtcNow,
					User = currentUser,
					Category = model.Category
					
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

						currentUser.ImagesCount++;
					}

					currentUser.AlbumsCount++;
				}

				db.Update(currentUser);

				db.SaveChanges();
				
				return RedirectToAction("Index", "MyProfile");
			}

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

			var currentUser = userManager.GetUserAsync(User).Result;

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

			if(album.User != currentUser)
			{
				return BadRequest();
			}

			return View(album);
		}

		//POST: Albums/Edit?albumId={id}
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Edit(AlbumDetailsViewModel model, List<IFormFile> pictures, string userId)
		{

			var user = this.db.Users
				.Where(u => u.Id == userId)
				.FirstOrDefault();
			
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

						user.ImagesCount++;
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

				author.CommentsCount++;

				db.SaveChanges();

				return RedirectToAction("Details", "Albums", new { @albumId = model.Id, @userId = comment.Album.UserId });

			}

			return BadRequest();
		}

		// in Albums/Edit?albumId{id}/userId={id}
		// Albums/DeleteComment?commentId={id}/albumId={id}/userId={id}
		public IActionResult DeleteComment(int commentId, int albumId, string userId)
		{

			var currentUser = userManager.GetUserAsync(User).Result;

			// finds the author of the comment
			var user = this.db.Comments
				.Where(c => c.Id == commentId)
				.Select(c => new DeleteAlbumCommentsViewModel()
				{
					User = c.User
				})
				.FirstOrDefault();

			if(user.User.Id != currentUser.Id)
			{
				return BadRequest();
			}
		

			var comment = this.db.Comments
				.Where(c => c.Id == commentId)
				.FirstOrDefault();

			db.Comments.Remove(comment);

			user.User.CommentsCount--;

			db.Users.Update(user.User);

			db.SaveChanges();

			return RedirectToAction("Edit", "Albums", new { @albumId = albumId, @userId = userId});
		}

		// in Albums/Edit?albumId{id}/userId={id}
		// Albums/DeleteImage?imageId={id}/albumId={id}/userId={id}
		public IActionResult DeleteImage (int imageId, int albumId, string userId)
		{

			var currnetUser = userManager.GetUserAsync(User).Result;

			var image = this.db.Images
				.Where(img => img.Id == imageId)
				.FirstOrDefault();

			if(image == null)
			{
				return NotFound();
			}

			var user = this.db.Users
				.Where(u => u.Id == userId)
				.FirstOrDefault();

			if(user.Id != currnetUser.Id)
			{
				return BadRequest();
			}

			var likes = this.db.Likes
				.Where(l => l.Image.Id == imageId)
				.ToList();

			if(likes.Count > 0)
			{
				foreach(var like in likes)
				{
					if(like.UserId == userId)
					{
						user.LikesCount--;
					}
				}
				db.Likes.RemoveRange(likes);
			}

			db.Images.Remove(image);

			user.ImagesCount--;

			db.Update(user);

			db.SaveChanges();

			return RedirectToAction("Edit", "Albums", new { @albumId = albumId, @userId = userId });

		}

		// in Albums/Edit?albumId{id}/userId={id}
		// Albums/Delete?albumId={id}
		public IActionResult Delete(int albumId, string userId)
		{

			var currentUser = userManager.GetUserAsync(User).Result;

			var album = this.db.Album
				.Where(al => al.Id == albumId)
				.FirstOrDefault();

			var user = this.db.Users
				.Where(u => u.Id == userId)
				.FirstOrDefault();

			if(user.Id != currentUser.Id)
			{
				return BadRequest();
			}

			var images = this.db.Images
				.Where(img => img.Album.Id == albumId)
				.ToList();

			if(images.Count > 0)
			{
				foreach(var image in images)
				{
					var likes = this.db.Likes
						.Where(l => l.Image.Id == image.Id)
						.Select(l => new Like()
						{
							Id = l.Id,
							Image = l.Image,
							UserId = l.UserId
						})
						.ToList();

					if(likes.Count > 0)
					{
						foreach (var like in likes)
						{
							if (like.UserId == userId)
							{
								user.LikesCount--;
							}
						}
						db.Likes.RemoveRange(likes);
					}
					user.ImagesCount--;
				}
				db.Images.RemoveRange(images);
			}

			var comments = this.db.Comments
				.Where(c => c.Album.Id == albumId)
				.ToList();

			var commentsUsers = this.db.Comments
				.Where(c => c.Album.Id == albumId)
				.Select(c => new CommentDetailsViewModel()
				{
					Author = c.User

				}).ToList();

			if(comments.Count > 0)
			{

				foreach(var comment in commentsUsers)
				{
					var author = this.db.Users
						.Where(u => u.Id == comment.Author.Id)
						.FirstOrDefault();

					author.CommentsCount--;

					db.Update(author);
				}

				db.Comments.RemoveRange(comments);
			}

			db.SaveChanges();

			db.Album.Remove(album);

			user.AlbumsCount--;

			db.Update(user);

			db.SaveChanges();


			string path = Path.Combine(environment.WebRootPath, "uploads", userId, albumId.ToString());

			if (System.IO.Directory.Exists(path))
			{
				System.IO.Directory.Delete(path, true);
			}

			return RedirectToAction("Index", "MyProfile");
		}
		
	}
}
