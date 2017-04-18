using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Code.Data;
using Code.Models.AlbumVIewModels;
using System.Collections.Generic;
using Code.Models.SearchViewModels;

namespace Code.Controllers
{
	public class HomeController : Controller
    {
		private readonly ApplicationDbContext db;

		public HomeController(ApplicationDbContext db)
		{
			this.db = db;
		}

        public IActionResult Index(ParentAlbumViewModel model)
        {
			model.List = this.db.Album
				.OrderByDescending(al => al.CreatedOn)
				.Select(al => new AlbumDetailsViewModel
				{
					Id = al.Id,
					Name = al.Name,
					Creator = al.User,
				})
				.Take(12)
				.ToList();

			var albums = this.db.Album
				.OrderByDescending(al => al.CreatedOn)
				.Take(12)
				.ToList();

			model.Images = new List<ImageDetailsViewModel>();

			foreach(var album in albums)
			{
				var albumImages = this.db.Images
					.Where(img => img.Album.Id.ToString() == album.Id.ToString())
					.Select(img => new ImageDetailsViewModel()
					{
						Id = img.Id,
						Album = img.Album,
						Name = img.Name,
						Rating = img.Rating,
						User = img.User
					})
					.ToList();
				model.Images.AddRange(albumImages);
			}


			return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
