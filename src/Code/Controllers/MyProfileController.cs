namespace Code.Controllers
{
	using System.Linq;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Authorization;
	using Models;
	using Data;
	using Models.AlbumVIewModels;

	[Authorize]
	public class MyProfileController : Controller
    {
		private readonly ApplicationDbContext db;
		private readonly UserManager<ApplicationUser> userManager;

		public MyProfileController(ApplicationDbContext db,
			UserManager<ApplicationUser> userManager)
		{
			this.db = db;
			this.userManager = userManager;
		}


		public IActionResult Index(string userId)
        {
			var model = new ParentProfileViewModel();

			model.User = db.Users
				.Where(u => u.Id == userId)
				.FirstOrDefault();

			 model.Albums = db.Album
				.Where(al => al.User.Id == userId)
				.Select(al => new ListAlbumsViewModel
				{
					Creator = User.Identity.Name,
					Id = al.Id,
					Name = al.Name
				}).ToList();

			return View(model);
        }

		// this method will form that user will fill
		[HttpGet]
		public IActionResult Edit()
		{
			var user = userManager.GetUserAsync(User);

			var current = user.Result;

			return View(current);
		}
		// this method will sent the information to the database
		[HttpPost]
		public IActionResult Edit(ApplicationUser user)
		{
			if(ModelState.IsValid)
			{
				var currentUser = userManager.GetUserAsync(User);

				currentUser.Result.FirstName = user.FirstName;
				currentUser.Result.LastName = user.LastName;
				currentUser.Result.Location = user.Location;
				currentUser.Result.Age = user.Age;
				currentUser.Result.Description = user.Description;

				db.Update(currentUser.Result);

				db.SaveChanges();

				return RedirectToAction("Index", "Home");
			}

			return View("Edit");
		}
    }
}
