namespace Code.Data
{
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;
	using Code.Models;

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

		public DbSet<Image> Images { get; set; }

		public DbSet<Album> Album { get; set; }

		public DbSet<Comment> Comments { get; set; }

		public DbSet<Like> Likes { get; set; }

		public DbSet<SingleImages> SingleImages { get; set; }

		public DbSet<SingleImagesLikes> SingleImagesLikes { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
        }
    }
}
