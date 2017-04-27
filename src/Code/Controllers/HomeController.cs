using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Code.Data;
using Code.Models.AlbumVIewModels;
using System.Collections.Generic;
using Code.Models.SearchViewModels;
using Code.Models.HomeVIewModels;
using Code.Models.SingleImageViewModels;

namespace Code.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext db;

		public HomeController(ApplicationDbContext db)
		{
			this.db = db;
		}

		// Home/Index
		public IActionResult Index()
		{
			var model = new HomeViewModel();

				model.Images = this.db.SingleImages
				.Take(12)
				.OrderByDescending(img => img.CreatedOn)
				.Select(img => new SingleImageDetailsViewModel()
				{
					Id = img.Id,
					Name = img.Name,
					Path = img.Path,
					Rating = img.Rating
				})
				.ToList();

				model.Albums = this.db.Album
					   .Take(6)
					   .OrderByDescending(al => al.Id)
					   .Select(al => new HomeAlbumsDetailsViewModel()
					   {
						     Id = al.Id,
						     Name = al.Name,
							 User = al.User,
						     Images = this.db.Images
							.Where(img => img.Album.Id == al.Id)
							.Select(img => new AlbumImageDetailsViewModel()
							{
								Id = img.Id,
								Name = img.Name,
								Rating = img.Rating,
								Album = al,
								Path = al.UserId + "/" + al.Id.ToString() + "/" + img.Name
							})
							.ToList()
					   }).ToList();


			return View(model);
		}
	}
}
