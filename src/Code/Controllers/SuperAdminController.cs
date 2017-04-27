namespace Code.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Identity;
	using Code.Models;
	using Code.Data;
	using Microsoft.AspNetCore.Authorization;
	using Models.AdminViewModels;
	using System;
	using System.Linq;
	using Models.SearchViewModels;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


	// FOR THE SITE OWNER
	[Authorize(Roles = "super_admin")]
	public class SuperAdminController : Controller
    {
		public readonly UserManager<ApplicationUser> userManager;
		public readonly ApplicationDbContext db;

		public SuperAdminController(UserManager<ApplicationUser> userManager,
			ApplicationDbContext db)
		{
			this.userManager = userManager;
			this.db = db;
		}

		// SuperAdmin/Index?page={1}
		public IActionResult Index(int page = 1)
        {
			ViewBag.TotalPages = Math.Ceiling(this.db.Users.Count() / 10.0);

			ViewBag.CurrentPage = page;

			if (page < 1 || page > ViewBag.CurrentPage)
			{
				if (ViewBag.TotalPages != 0)
				{
					return NotFound();
				}
			}

			int pageSize = 10;

			var model = new AdminViewModel();

			model.Admins = this.db.Users
				.Where(u => u.isAdmin == true)
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
				.ToList();

			model.Users = this.db.Users
				.Where(u => u.isAdmin == false)
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

		// Redirects:  SuperAdmin/Index?page={page}
		public IActionResult GiveRights(string userId, int page)
		{
			var user = this.db.Users
				.Where(u => u.Id == userId)
				.FirstOrDefault();

			user.isAdmin = true;

			db.Update(user);

			var role = new IdentityUserRole<string>();
			role.RoleId = "1";
			role.UserId = userId;

			db.UserRoles.Add(role);

			db.SaveChanges();

			return RedirectToAction("Index", "SuperAdmin", new { @page = page });
		}

		// Redirects:  SuperAdmin/Index?page={page}
		public IActionResult TakeRights(string userId, int page)
		{
			var user = this.db.Users
				   .Where(u => u.Id == userId)
				   .FirstOrDefault();

			user.isAdmin = false;

			var role = new IdentityUserRole<string>();
			role.RoleId = "1";
			role.UserId = userId;

			db.UserRoles.Remove(role);

			db.Update(user);

			db.SaveChanges();

			return RedirectToAction("Index", "SuperAdmin", new { @page = page });
		}
	}
}
