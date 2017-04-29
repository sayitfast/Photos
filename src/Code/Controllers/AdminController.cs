namespace Code.Controllers
{
	using Data;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Models;
	using Models.AdminViewModels;
	using Models.AlbumVIewModels;
	using Models.SearchViewModels;
	using Models.SingleImageViewModels;
	using System;
	using System.IO;
	using System.Linq;

	[Authorize(Roles ="admin")]
	public class AdminController : Controller
    {
		private readonly ApplicationDbContext db;
		private readonly UserManager<ApplicationUser> userManager;
		private IHostingEnvironment environment;

		public AdminController(ApplicationDbContext db,
			UserManager<ApplicationUser> userManager,
			IHostingEnvironment environment)
		{
			this.db = db;
			this.userManager = userManager;
			this.environment = environment;
		}

		// Admin/Panel?page={page}
		public IActionResult Panel(int page = 1)
        {
			ViewBag.TotalPages = Math.Ceiling(this.db.Users.Count() / 10.0);

			ViewBag.CurrentPage = page;

			if(page < 1 || page > ViewBag.CurrentPage)
			{
				if (ViewBag.TotalPages != 0)
				{
					return NotFound();
				}
			}

			int pageSize = 10;

			var model = new AdminViewModel();

			model.Users = this.db.Users
				.OrderBy(u => u.FirstName)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(u => new UserDetailsViewModel()
				{
					Id = u.Id,
					FirstName = u.FirstName,
					LastName = u.LastName,
					Location = u.Location,
					ProfilePictureName = u.ProfilePicture,
					Email = u.Email,
					isAdmin = u.isAdmin,
					TotalAlbums = u.AlbumsCount,
					TotalImages = u.ImagesCount,
					TotalLikes = u.LikesCount,
					TotalComments = u.CommentsCount
					
				})
				.OrderBy(u => u.FirstName)
				.ToList();

            return View(model);
        }

		// Admin/UserDetails?userId={id}
		public IActionResult UserDetails(string userId)
		{
			var model = this.db.Users
				.Where(u => u.Id == userId)
				.Select(u => new UserDetailsViewModel()
				{
					Id = u.Id,
					FirstName = u.FirstName,
					LastName = u.LastName,
					Location = u.Location,
					ProfilePictureName = u.ProfilePicture,
					Email = u.Email,
					isAdmin = u.isAdmin,
					TotalAlbums = u.AlbumsCount,
					TotalImages = u.ImagesCount,
					TotalLikes = u.LikesCount,
					Albums = this.db.Album
					.Where(al => al.UserId == u.Id)
					.Select(al => new AlbumDetailsViewModel()
					{
						Id = al.Id,
						Description = al.Description,
						Name = al.Name,
						CreatedOn = al.CreatedOn,
						Category = al.Category,
						Creator = al.User
					})
					.ToList(),
					Images = this.db.SingleImages
					.Where(img => img.User.Id == u.Id)
					.Select(img => new SingleImageDetailsViewModel()
					{
						Id = img.Id,
						Path = img.Path,
						Category = img.Category,
						Description = img.Description,
						User = img.User,
						Location = img.Location,
						Name = img.Name,
						Rating = img.Rating,
						UploadedOn = img.CreatedOn

					}).ToList(),
					Comments = this.db.Comments
					.Where(c => c.User.Id == u.Id)
					.Select(c => new CommentDetailsViewModel()
					{
						Id = c.Id,
						Content = c.Content,
						CreatedOn = c.CreatedOn,
						Album = c.Album,
						Author = c.User

					}).ToList()
				})
				.FirstOrDefault();

			return View(model);

		}

		// Admin/DeleteAlbum?albumId={id}&userId={id}
		// Redirects to: Admin/UserDetails?userId={id}
		// this action deletes the whole album (images/comments/likes)
		public IActionResult DeleteAlbum(int albumId, string userId)
		{
			var album = this.db.Album
				.Where(al => al.Id == albumId)
				.Select(al => new Album()
				{
					Id = al.Id,
					User = al.User,
				})
				.FirstOrDefault();

			if(album == null)
			{
				return NoContent();
			}

			var creator = this.db.Users
				.Where(u => u.Id == userId)
				.FirstOrDefault();

			creator.AlbumsCount--;

			db.Update(creator);

			var images = this.db.Images
				.Where(img => img.Album.Id == albumId)
				.Select(img => new Image()
				{
					Id = img.Id,
					Album = img.Album,
					User = img.User
				})
				.ToList();

			if(images.Count > 0)
			{
				foreach (var image in images)
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

					if (likes.Count > 0)
					{
						foreach (var like in likes)
						{
							var author = this.db.Users
								.Where(u => u.Id == like.UserId)
								.FirstOrDefault();

							author.LikesCount--;

							db.Update(author);
						}
						db.Likes.RemoveRange(likes);
					}
					creator.ImagesCount--;
				}

				db.Images.RemoveRange(this.db.Images.Where(img => img.Album.Id == albumId).ToList());
			}
			var albumComments = this.db.Comments
				.Where(c => c.Album.Id == albumId)
				.Select(c => new Comment()
				{
					Id = c.Id,
					Album = c.Album,
					User = c.User
				})
				.ToList();
			if(albumComments.Count > 0)
			{
				foreach(var comment in albumComments)
				{
					var author = this.db.Users
						.Where(u => u.Id == comment.User.Id)
						.FirstOrDefault();
					author.CommentsCount--;
					db.Update(author);
				}

				db.Comments.RemoveRange(albumComments);
			}

			db.Album.Remove(this.db.Album.Where(al => al.Id == albumId).FirstOrDefault());

			db.SaveChanges();

			string path = Path.Combine(environment.WebRootPath, "uploads", userId, albumId.ToString());

			if (System.IO.Directory.Exists(path))
			{
				System.IO.Directory.Delete(path, true);
			}

			return RedirectToAction("UserDetails", "Admin", new { @userId = userId });
		}

		// Admin/DeleteImage?albumId={id}&userId={id}
		// Redirects to: Admin/UserDetails?userId={id}
		// this action deletes an image and the likes related to it
		public IActionResult DeleteImage(int imageId, string userId)
		{
			var user = this.db.Users
				.Where(u => u.Id == userId)
				.FirstOrDefault();

			var image = this.db.SingleImages
				.Where(img => img.Id == imageId)
				.FirstOrDefault();

			if(image == null)
			{
				return NoContent();
			}

			var likes = this.db.SingleImagesLikes
				.Where(l => l.Image.Id == imageId)
				.Select(l => new SingleImagesLikes()
				{
					Id = l.Id,
					User = l.User,
					Image = l.Image
				})
				.ToList();

			if(likes.Count > 0)
			{
				foreach(var like in likes)
				{
					var author = this.db.Users
						.Where(u => u.Id == like.User.Id)
						.FirstOrDefault();
					author.LikesCount--;
					db.Update(author);
				}
				db.SingleImagesLikes.RemoveRange(likes);
			}

			user.ImagesCount--;

			db.Update(user);

			db.SingleImages.Remove(image);

			db.SaveChanges();

			return RedirectToAction("UserDetails", "Admin", new { @userId = userId });
		}

		// Admin/DeleteComment?albumId={id}&userId={id}
		// Redirects to: Admin/UserDetails?userId={id}
		// this aciton deletes a comment form particular user
		public IActionResult DeleteComment(int commentId, string userId)
		{
			var comment = this.db.Comments
				.Where(c => c.Id == commentId)
				.FirstOrDefault();

			if(comment == null)
			{
				return NoContent();
			}

			var user = this.db.Users
				.Where(u => u.Id == userId)
				.FirstOrDefault();

			user.CommentsCount--;

			db.Update(user);

			db.Comments.Remove(comment);

			db.SaveChanges();

			return RedirectToAction("UserDetails", "Admin", new { @userId = userId });
		}
    }
}
