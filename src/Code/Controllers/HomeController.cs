﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Code.Data;
using Code.Models.AlbumVIewModels;

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
			model.List = db.Album
				.OrderByDescending(al => al.CreatedOn)
				.Select(al => new ListAlbumsViewModel
				{
					Id = al.Id,
					Name = al.Name,
					Creator = al.User,
					TotalImages = 0
				})
				.Take(12)
				.ToList();


			return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
