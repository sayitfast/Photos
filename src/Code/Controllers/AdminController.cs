namespace Code.Controllers
{
	using Data;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Models;
	using Models.AdminViewModels;
	using Models.AlbumVIewModels;
	using Models.SearchViewModels;
	using Models.SingleImageViewModels;
	using System;
	using System.Linq;

	[Authorize(Roles ="admin")]
	public class AdminController : Controller
    {
		public readonly UserManager<ApplicationUser> userManager;

		public readonly ApplicationDbContext db;

		public AdminController (UserManager<ApplicationUser> userManager,
			ApplicationDbContext db)
		{
			this.userManager = userManager;
			this.db = db;
		}

        public IActionResult Panel()
        {
			var model = new AdminViewModel();

			model.Users = this.db.Users
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
					TotalLikes = u.LikesCount
				})
				.OrderBy(u => u.FirstName)
				.ToList();

			model.Albums = this.db.Album
				.Select(al => new AlbumDetailsViewModel()
				{
					Id = al.Id,
					Description = al.Description,
					Name = al.Name,
					CreatedOn = al.CreatedOn,
					Category = al.Category,
					Creator = al.User
				})
				.ToList();

            return View(model);
        }

		public IActionResult Users()
		{
			var model = this.db.Users
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
						Album = c.Album

					}).ToList()
				})
				.OrderBy(u => u.FirstName).ToList();

			return View(model);

		}

		public IActionResult AlbumDashboard(int albumId)
		{
			var model = new ParentAlbumViewModel();

			model.AlbumDetails= this.db.Album
				.Where(al => al.Id == albumId)
				.Select(al => new AlbumDetailsViewModel()
				{
					Id = al.Id,
					Category = al.Category,
					Creator = al.User,
					Description = al.Description,
					CreatedOn = al.CreatedOn,
					Name = al.Name
				})
				.FirstOrDefault();

			model.Images = this.db.Images
				.Where(img => img.Album.Id == albumId)
				.Select(img => new ImageDetailsViewModel()
				{
					Id = img.Id,
					Name = img.Name,
					Album = img.Album,
					Rating = img.Rating,
					User = img.User
				})
				.ToList();

			model.Comments = this.db.Comments
				.Where(c => c.Album.Id == albumId)
				.Select(c => new CommentDetailsViewModel()
				{
					Id = c.Id,
					Author = c.User,
					Content = c.Content,
					CreatedOn = c.CreatedOn
				})
				.ToList();

			return View(model);
		}
    }
}
