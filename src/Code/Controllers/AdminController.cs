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
			var model = new ParentAdminViewModel();

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
					TotalAlbums = this.db.Album
					.Where(al => al.UserId == u.Id)
					.Count(),
					TotalImages = this.db.Images
					.Where(img => img.UserId == u.Id)
					.Count(),
					TotalLikes = this.db.Likes
					.Where(l => l.UserId == u.Id)
					.Count()
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
					Creator = al.User,
					CoverImage = this.db.Images
					.Where(img => img.Album.Id == al.Id)
					.FirstOrDefault()
					.Name
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
					TotalAlbums = this.db.Album
				   .Where(al => al.UserId == u.Id)
				   .Count(),
					TotalImages = this.db.Images
				   .Where(img => img.UserId == u.Id)
				   .Count(),
					TotalLikes = this.db.Likes
				   .Where(l => l.UserId == u.Id)
				   .Count(),
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
					.ToList()
				})
				.OrderBy(u => u.FirstName).ToList();

			return View(model);

		}
    }
}
