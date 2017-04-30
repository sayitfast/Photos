<h1>#Photos</h1>
<p><strong>"Photos"</strong> is a simple, easy to use, Web application, cource work project, for the Software Technologies Cource at <a href="https://softuni.bg/">Software University</a>.The project was built with <strong>ASP.NET</strong>, <strong>.NET Core</strong> and the <strong>MVC</strong> architecture.</p>

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
<h3>Signing in</h3>

![login_page](https://cloud.githubusercontent.com/assets/24397315/25558494/9c20548a-2d30-11e7-8ff4-6c7b7aa3e702.png)

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

<h3>Account Example</h3>

![my_profile_page](https://cloud.githubusercontent.com/assets/24397315/25558661/e86febc2-2d33-11e7-8c9e-0ceec2f16a6c.png)

<hr />

<h3>Search Functionality</h3>

![search_form](https://cloud.githubusercontent.com/assets/24397315/25563040/dbf8cc0c-2d9b-11e7-8752-67dfbf7c7810.png)
