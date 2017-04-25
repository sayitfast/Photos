namespace Code.Controllers
{
	using Data;
	using System.Linq;
	using System.Collections.Generic;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Models;
	using Models.AlbumVIewModels;
	using Models.SearchViewModels;
	using Models.HomeVIewModels;

	public class SearchController : Controller
	{

		public readonly ApplicationDbContext db;

		public readonly UserManager<ApplicationUser> userManager;

		public SearchController(ApplicationDbContext db,
			UserManager<ApplicationUser> userManager)
		{
			this.db = db;
			this.userManager = userManager;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(HomeViewModel model)
		{
			if (model.Search == null)
			{
				return RedirectToAction("Index", "Home");
			}

			var result = new SearchResultViewModel();

			switch (model.Option)
			{
				case "SingleImages":
					
					break;
				case "Album":

					break;
				case "Users":

					break;
			}

			return View();
		}
	}
}
