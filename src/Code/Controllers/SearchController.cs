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
	using Models.SingleImageViewModels;
	using System;

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

		// GET: Search/Index
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		// POST: Search/Index
		[HttpPost]
		public IActionResult Index(HomeViewModel model)
		{
			if (model.Search == null)
			{
				return RedirectToAction("Index", "Home");
			}

			if (model.Option == "SingleImages")
			{
				return RedirectToAction("ImagesSearch", "Search", new { @category = model.Search });
			}

			else if (model.Option == "Album")
			{
				return RedirectToAction("AlbumsSearch", "Search", new { @category = model.Search });
			}
			else
			{
				return NotFound();
			}

		}

		// Search/ImagesSearch?category={category}&page={1}
		public IActionResult ImagesSearch(string category, int page = 1)
		{
			ViewBag.TotalPages = Math.Ceiling(
				this.db.SingleImages
				.Where(img => img.Category == category).Count() / 5.0);

			ViewBag.CurrentPage = page;


			if (page < 1 || page > ViewBag.TotalPages)
			{
				if (ViewBag.TotalPages != 0)
				{
					return NotFound();
				}
			}

			int pageSize = 5;

			var result = this.db.SingleImages
				.Where(img => img.Category == category)
				.OrderByDescending(img => img.CreatedOn)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(img => new SingleImageDetailsViewModel()
				{
					Id = img.Id,
					Description = img.Description,
					Location = img.Location,
					Name = img.Name,
					Path = img.Path,
					Rating = img.Rating,
					UploadedOn = img.CreatedOn,
					Category = img.Category,
					User = img.User
				})
				.ToList();

			return View(result);
		}

		// Search/AlbumsSearch?category={category}&page={1}
		public IActionResult AlbumsSearch(string category, int page = 1)
		{
			ViewBag.TotalPages = Math.Ceiling(
				this.db.Album
				.Where(al => al.Category == category).Count() / 5.0);

			ViewBag.CurrentPage = page;


			if (page < 1 || page > ViewBag.TotalPages)
			{
				if (ViewBag.TotalPages != 0)
				{
					return NotFound();
				}
			}

			int pageSize = 5;

			var result = this.db.Album
				.Where(al => al.Category == category)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
			      .OrderByDescending(al => al.CreatedOn)
			      .Select(al => new HomeAlbumsDetailsViewModel()
			      {
			        Id = al.Id,
			        Name = al.Name,
			        User = al.User,
					Category = al.Category,
			        Images = this.db.Images
			     	.Where(img => img.Album.Id == al.Id)
			     	.Select(img => new AlbumImageDetailsViewModel()
			     	{
			     		Rating = img.Rating,
			     		Album = al,
			     		Path = al.UserId + "/" + al.Id.ToString() + "/" + img.Name
			     	})
			     	.ToList()
			      }).ToList();

			return View(result);

		}
	}
}
