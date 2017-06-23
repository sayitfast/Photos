namespace Code.Data
{
	using Code.Models;
	using System.ComponentModel.DataAnnotations;
	using System;
	using System.Collections.Generic;
	using Microsoft.AspNetCore.Http;

	public class Album
    {
		public int Id { get; set; }

		[Required]
		[MinLength(5), MaxLength(100)]
		public string Name { get; set; }

		[MaxLength(160)]
		public string Description { get; set; }

		[Required]
		public string Category { get; set; }

		public string UserId { get; set; }

		public virtual ApplicationUser User { get; set; }

		public DateTime CreatedOn { get; internal set; }
		
	}
}
