namespace Code.Controllers
{
	using System.Linq;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Authorization;
	using Models;
	using Data;
	using Models.AlbumVIewModels;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Hosting;
	using System.IO;
	using System.Threading.Tasks;

	[Authorize]
	public class MyProfileController : Controller
    {
		private readonly ApplicationDbContext db;
		private readonly UserManager<ApplicationUser> userManager;
		private IHostingEnvironment environment;

		public MyProfileController(ApplicationDbContext db,
			UserManager<ApplicationUser> userManager,
			IHostingEnvironment environment)
		{
			this.environment = environment;
			this.db = db;
			this.userManager = userManager;
		}


		public IActionResult Index()
        {
			var currentUser = userManager.GetUserAsync(User).Result;

			var model = new ParentProfileViewModel();

			model.User = db.Users
				.Where(u => u.Id == currentUser.Id)
				.FirstOrDefault();

			model.Albums = db.Album
				 .Where(al => al.User == currentUser)
				 .ToList();

			model.Images = db.Images
				.Where(img => img.User == currentUser)
				.ToList();

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
		public async Task<IActionResult> Edit(ApplicationUser user, IFormFile ProfilePictureFile)
		{
			if (ModelState.IsValid)
			{
				var currentUser = userManager.GetUserAsync(User).Result;
				currentUser.FirstName = user.FirstName;
				currentUser.LastName = user.LastName;
				currentUser.Location = user.Location;
				currentUser.Age = user.Age;
				currentUser.Description = user.Description;

				if (ProfilePictureFile != null)
				{
					string uploadPath = Path.Combine(environment.WebRootPath, "uploads");
					Directory.CreateDirectory(Path.Combine(uploadPath, currentUser.Id));

					string filename = ProfilePictureFile.FileName.Split('\\').Last();

					using (FileStream fs = new FileStream(Path.Combine(uploadPath, currentUser.Id, filename), FileMode.Create))
					{
						await ProfilePictureFile.CopyToAsync(fs);
					}

					currentUser.ProfilePicture = filename;
				}

				db.Update(currentUser);

				db.SaveChanges();

				return RedirectToAction("Index", "MyProfile");
			}

			return View("Edit");
		}
	}
}
