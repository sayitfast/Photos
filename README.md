<h1>#Photos</h1>
<p><strong>"Photos"</strong> is a simple, easy to use, Web application, cource work project, for the Software Technologies Cource at <a href="https://softuni.bg/">Software University</a>.The project was built with <strong>ASP.NET</strong>, <strong>.NET Core</strong> and the <strong>MVC</strong> architecture.</p>

<p>The project includes external <strong>ASP.NET Core</strong> extensions from <a href="https://github.com/ivaylokenov/AspNet.Mvc.TypedRouting">AspNet.Mvc.TypedRouting</a></p>

![home_page](https://cloud.githubusercontent.com/assets/24397315/25558263/be349036-2d2b-11e7-9c49-4195b1e757a1.png)

<h3>What are the basic functionalities?</h3>
<ul>
  <li>
  <h4>Without registration:</h4>
     <ul>
        <li>Searching for content by category</li>
     </ul>
  </li>
    <li>
  <h4>With registration:</h4>
     <ul>
        <li>Having profile page with the ability to manage the uploaded content</li>
        <li>Uploading photos</li>
        <li>Creating alubms</li>
        <li>Commenting albums</li>
        <li>Ability to rate content</li>
        <li>Searching for content by category</li>
     </ul>
  </li>
</ul>

<hr />
<h3>Creating an account</h3>

![register_page](https://cloud.githubusercontent.com/assets/24397315/25558442/7cd01daa-2d2f-11e7-809a-42ae3b559cfc.png)

<p>Creating an account is done by filling the fields with appropriate information. The *Required lable is placed above every field that is needed in order to make the process registring clearer and to insure valid inputs.</p>

<p>You can configure the password requirements in the <a href="https://github.com/StoyanVitanov/Photos/blob/master/src/Code/Startup.cs">Startup.cs</a> file, Configuration action.</p>  

   
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(identity =>
			{
				identity.Password.RequireDigit = true;
				identity.Password.RequireLowercase = false;
				identity.Password.RequireNonAlphanumeric = false;
				identity.Password.RequireUppercase = false;
			})
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

			services.AddMvc().AddTypedRouting();

			services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

<hr />
<h3>Signed in</h3>
<p>Once signed in, on the navigation bar several new items will appear:<strong>the profile picture</strong>, <strong>upload picture icon</strong>, <strong>profile icon</strong>.</p>

![user_navbar](https://cloud.githubusercontent.com/assets/24397315/25558575/2fd60048-2d32-11e7-85e9-e2c1a7cef726.png)

<p>The initial user profile page</p>

![user_myprofile_page](https://cloud.githubusercontent.com/assets/24397315/25558593/8fa3462a-2d32-11e7-9adb-4bb677a8e830.png)

<p>With the shortcuts bellow the profile picture, the user can easily manage the account</p>

<hr />

<h3>Creating content</h3>

<h4>Uploading Image</h4>
      
![upload_photo_page](https://cloud.githubusercontent.com/assets/24397315/25558635/7e079910-2d33-11e7-88b2-f5fc950db894.png)
      
<h4>Uploading Album</h4>
      
![create_album_page](https://cloud.githubusercontent.com/assets/24397315/25558639/8911ec0c-2d33-11e7-9ff7-336a2de78202.png)

<hr />

<h3>Search Functionality</h3>

![search_form](https://cloud.githubusercontent.com/assets/24397315/25563040/dbf8cc0c-2d9b-11e7-8752-67dfbf7c7810.png)

<h3>In the View</h3>

	<div class="row">
		<div class="col-md-8 col-md-offset-2 text-center f_form">
			<div class="text-center">
				@using (Html.BeginForm<SearchController>(c => c.Index(), FormMethod.Post))
			{
					<div class="col-md-8 f_input">
						@{var searchList = new SelectList(new List<SelectListItem>
					{
					new SelectListItem { Text = "Travel", Value = "travel" },
					new SelectListItem { Text = "Holiday", Value = "holiday" },
					new SelectListItem { Text = "Food", Value = "food" },
					new SelectListItem { Text = "Business", Value = "business" },
					new SelectListItem { Text = "Night", Value = "night" },
					new SelectListItem { Text = "Sunset", Value = "sunset" },
					new SelectListItem { Text = "Technology", Value = "technology" },
					new SelectListItem { Text = "Abstract", Value = "abstract" },
					new SelectListItem { Text = "Mountains", Value = "mountains" },
					new SelectListItem { Text = "Music", Value = "music" },
					new SelectListItem { Text = "Black And White", Value = "black and white" },
					new SelectListItem { Text = "Car", Value = "car" },
					new SelectListItem { Text = "City", Value = "city" },
					new SelectListItem { Text = "Flowers", Value = "flowers" },
					new SelectListItem { Text = "Landscape", Value = "lanscape" },
					new SelectListItem { Text = "Ocean", Value = "ocean" },
					new SelectListItem { Text = "Photography", Value = "photography" },
					new SelectListItem { Text = "Animal", Value = "animal" },
					new SelectListItem { Text = "Art", Value = "art" },
					new SelectListItem { Text = "Beach", Value = "beach" },
					new SelectListItem { Text = "Fashion", Value = "fasion" },
					new SelectListItem { Text = "Sport", Value = "sport" },
					new SelectListItem { Text = "People", Value = "people" },
					new SelectListItem { Text = "Vintage", Value = "vintage" },
					new SelectListItem { Text = "Sky", Value = "sky" }
					}, "Value", "Text");
						}
						@Html.DropDownListFor(m => m.Search, searchList, null, new { @class = "form-control list", @style = "width: 100%; border-radius: 0px;" })
						@Html.ValidationMessageFor(m => m.Search, null, new { @class = "text-danger" })
					</div>
					<div class="col-md-3 f_options">
						@{var selectList = new SelectList(new List<SelectListItem>
					{
					new SelectListItem { Text = "Photos", Value = "SingleImages" },
					new SelectListItem { Text = "Albums", Value = "Album" }
					}, "Value", "Text");
						}
						@Html.DropDownListFor(m => m.Option, selectList, null, new { @class = "form-control list", @style = "width: 100%; border-radius: 0px;" })
					</div>
					<div class="col-md-1 f_submit">
						<button type="submit" id="search-button" class="btn btn-primary" style="width: 100%; border-radius: 0px;">
							<span class="glyphicon glyphicon-search"></span>
						</button>
					</div>
							}
			</div>
		</div>
	</div>


<h3>In the Search Controller</h3>

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
		
<p>Depending on the chosen option the <a href="https://github.com/StoyanVitanov/Photos/blob/master/src/Code/Controllers/SearchController.cs">Search Controller</a> redirects to a certain action.</p>


<h4>Searching for images</h4>
   
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
   
<h4>Searching for albums</h4>

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

<h4>Rendering the search results</h4>

<p>The example below shows how the images from the model are listed.</p>


![pagination](https://cloud.githubusercontent.com/assets/24397315/25563147/2aff8028-2d9e-11e7-95e1-ca243eff5378.png)


          @if(Model.Capacity == 0)
          {
          	<h1 class="text-center">There are no images with matching category</h1>
          }
          
           <div class="col-md-6 col-md-offset-3">
          
          
          	@foreach (var image in Model)
          	{
          		<div class="u_image text-center" style="width:100%;">
          			<img src="~/uploads/@image.Path" style="height: initial; width:100%;" />
          			<h4><strong>@image.Name</strong> @image.Description</h4>
          			<div class="row">
          				<div class="col-md-6 col-md-offset-3 text-center">
          					<a href="@(Url.Action("DislikeImage", "Image", new { @imageId = image.Id}))">
          						<span class="glyphicon glyphicon-chevron-left"></span>
          					</a>
          					<span><strong>@image.Rating</strong></span>
          					<a href="@(Url.Action("LikeImage", "Image", new { @imageId = image.Id }))">
          						<span class="glyphicon glyphicon-chevron-right"></span>
          					</a>
          				</div>
          			</div>
          		</div>
          	}
          	<hr />
		
<p>The same idea is used when searching for albums. The change here is in the way albums are showcased. The bootstrap plugin used here is called "Carousel", you can find more information on how to implement it <a href="https://www.w3schools.com/bootstrap/bootstrap_carousel.asp">here</a>.</p>

      @if(Model.Capacity == 0)
      {
      	<h1 class="text-center">There are no albums with matching category!</h1>
      
      }
      
    <div class="row">
      	<div class="col-md-6 col-md-offset-3">
      
      @Html.Partial("_PaginationAlbumsPartial");
      
      @foreach (var album in Model)
      {
        <div class="col-md-6 col-md-offset-3 u_album text-center" style="width: 100%;">
    
    	<h3 class="text-center">@album.Name</h3>
    
    	@if (album.Images.Capacity > 0)
    	{
      	<div id="album-with-pictures" class="carousel slide" data-ride="carousel" data-interval="4000" data-delay="3000">
      
      		<div class="carousel-inner" role="listbox">
      
      			@foreach (var image in album.Images)
      			{
      				var firstImage = album.Images.First();

      				if (image == firstImage)
      				{
      					<div class="item active">
      						<img src="~/uploads/@image.Path" alt="Chania" class="u_album_img" />
      					</div>
      				}
      				else
      				{
      					<div class="item">
      						<img src="~/uploads/@image.Path" alt="Chania" class="u_album_img" />
      					</div>
      				}
      			}
      		</div>
      					</div>
      				}
      	else
      	{
      		<img src="~/images/album_with_no_images.jpg" />
      	}
      
      	    <div class="a_details_link">
      	       <a href="@(Url.Action("Details", "Albums", new { @albumId = album.Id, @userId = album.User.Id }))" title="View" class="pull-right">
    	         <span class="glyphicon glyphicon-option-horizontal"></span>
	      </a>
	   </div>
        </div>
        }
	
<h4>How the pagination is done?</h4> 

![paging](https://cloud.githubusercontent.com/assets/24397315/25563234/386a5966-2da0-11e7-9d86-47a9651915eb.png)

<p>The pagination used in this project is not the best example and it is not recomended for big projects, because it gives errors when the pages are over 100, but for the small projects like the one here it works fine.</p>


<h4>When we call an action, by default we have to set the page to first Here is how is done:</h4>
      
                  public IActionResult ImagesSearch(string category, int page = 1) 
		                        
<p>In the exaple from the Search Controller, we pass to the action the category of the images and set the page to one.
         Next we have to manipulate the code so that every time we call this action it passes to the view the next set of images.
</p>

<h4>By using the <a href="https://msdn.microsoft.com/en-us/library/bb308959.aspx">LINQ</a> library, we can easily write the following query:</h4>

                  .Skip((page - 1) * pageSize).Take(pageSize)
		 		
<p>The purpose of this query is to pick "pageSize" count images after skipping pageSize multiplied by the current page minus one.
   It may sounds complicated, but it is very simple, imagine we are on page one. Lets say that the pageSize = 5 and the page is the first (1), the algorith will skip (1-1) * 5 images and will take 5, so it skips 0 and takes 5. On the second page the algorithm will skip (2 - 1) * 5 and will take 5 again. This time it skips 5 and takes the next set of 5 images, or if there are less than 5 it takes all. 
</p>

<h4>The next step is to pass to the view the current page and the total number of pages</h4>

<p>Taking advantage of the <a href="https://www.w3schools.com/asp/webpages_razor.asp">Razor</a> syntax, this task is fairly easy.All we have to do is to calculate the total page and save them to a ViewBag:</p>

                 ViewBag.TotalPages = Math.Ceiling(this.db.SingleImages
				.Where(img => img.Category == category).Count() / 5.0);
				
<p>Next we have to save the current page:</p>

                  ViewBag.CurrentPage = page;
   
<p>To insure correct results and to avoid inappropriate UX we have to write a simple condition:</p>

                     if (page < 1 || page > ViewBag.TotalPages)
	        	{
	        		if (ViewBag.TotalPages != 0)
	        		{
	        			return NotFound();
	        		}
	        	}
			
<p>What it does is simply not showing error messages to the user, but a blank page.</p>

<h4>The HTML View</h4>

                @model List<Code.Models.SingleImageViewModels.SingleImageDetailsViewModel>

                <div class="text-center">
                	<ul class="pagination">
                		@if (ViewBag.CurrentPage > 1)
                			{
                			<li>
					<a href="ImagesSearch?category=@Model.First().Category&page=@(ViewBag.CurrentPage - 1)" style="background-color: #1f7dd7; color: white;">Previous</a>
					</li>
                			}
                		@for (int i = 1; i <= ViewBag.TotalPages; i++)
                			{
                			@if (i == ViewBag.CurrentPage)
                				{
                				<li>
						<a href="ImagesSearch?category=@Model.First().Category&page=@i" style="background-color: gray; color: white;">@i</a>
						</li>
                				}
                				else
                				{
                				<li>
						<a href="ImagesSearch?category=@Model.First().Category&page=@i" style="background-color: #1f7dd7; color: white;">@i</a>
						</li>
                				}
                			}
                		@if (ViewBag.TotalPages >= 2)
                		{
                			if (Model.Count == 5)
                			{
                				<li>
						<a href="ImagesSearch?category=@Model.First().Category&page=@(ViewBag.CurrentPage + 1)" style="background-color: #1f7dd7; color: white;">Next</a>
						</li>
                			}
                		}
                	</ul>
                </div>                     


<hr />

<h3>Administrator Panel</h3>

<p>This panel gives the admins rights to inspect users and to edit their content if needed</p>

![admin_panel_main_page](https://cloud.githubusercontent.com/assets/24397315/25563694/30e4c4ec-2daa-11e7-8234-51f622280e0e.png)

<h4>Inspect User Page Example:</h4>

![admin_inspect_user_page](https://cloud.githubusercontent.com/assets/24397315/25563701/73526190-2daa-11e7-8aa3-abc088b3c69b.png)

<hr />

<h3>CEO Panel</h3>

<p>This panel gives the site owner rights to make certain user an admin or to take his/her admin rights.</p>

![ceo_panel_main_page](https://cloud.githubusercontent.com/assets/24397315/25563719/d1a81bcc-2daa-11e7-8207-281798cfa601.png)

<hr />

<h3>Profile Page Example</h3>

![my_profile_page](https://cloud.githubusercontent.com/assets/24397315/25657785/43a2b354-3008-11e7-8c99-5296c630449f.png)
<hr />

<h3>Album Examples</h3>

![album_details_page](https://cloud.githubusercontent.com/assets/24397315/25658101/ca0abc38-3009-11e7-9f42-5f3df91b54c7.png)


![album_edit_page](https://cloud.githubusercontent.com/assets/24397315/25658108/d8cd826e-3009-11e7-91cd-f7c91c891386.png)
