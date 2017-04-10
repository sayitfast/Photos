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
		public IActionResult Index(ParentAlbumViewModel model)
		{
			string input = model.Search
				.ToLower()
				.Trim();

			var result = new SearchResultViewModel();

			result.Albums = this.db.Album
				.Where(al => al.Category == input)
				.OrderByDescending(al => al.CreatedOn)
				.Select(al => new AlbumDetailsViewModel()
				{
					Id = al.Id,
					Creator = al.User,
					CreatedOn = al.CreatedOn,
					Name = al.Name,
					Description = al.Description,
				})
				.ToList();

			result.Users = new List<UserDetailsViewModel>()
				.OrderByDescending(u => u.TotalImages)
				.ToList();

			result.Images = new List<ImageDetailsViewModel>();

			foreach (var album in result.Albums)
			{
				result.Images
					.AddRange(this.db.Images
					.Where(img => img.Album.Id == album.Id)
					.OrderByDescending(img => img.Rating)
					.Select(img => new ImageDetailsViewModel()
					{
						Id = img.Id,
						Name = img.Name,
						Album = img.Album,
						User = img.User,
						Rating = img.Rating
					})
					.ToList());

				var user = this.db.Users
					.Where(u => u.Id == album.Creator.Id)
					.FirstOrDefault();

				var details = new UserDetailsViewModel()
				{
					Id = user.Id,
					FirstName = user.FirstName,
					LastName = user.LastName,
					Location = user.Location,

					TotalAlbums = this.db.Album
					.Where(al => al.UserId == user.Id)
					.Count(),

					TotalImages = this.db.Images
					.Where(img => img.UserId == user.Id)
					.Count(),

					TotalLikes = this.db.Likes
					.Where(l => l.UserId == user.Id)
					.Count()
				};

				if(result.Users.Count > 0) 
				{
					if(result.Users.All(c => c.Id != details.Id))
					{
						result.Users.Add(details);
					}
				}
				else
				{
					result.Users.Add(details);
				}
			}
			return View(result);
		}
	}
}
