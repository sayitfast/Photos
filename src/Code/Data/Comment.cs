using Code.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.ComponentModel.DataAnnotations;

namespace Code.Data
{
	[Authorize]
    public class Comment
    {
		public int Id { get; set; }

		[MinLength(5), MaxLength(160)]
		public string Content { get; set; }

		public DateTime CreatedOn { get; set; }

		public ApplicationUser User { get; set; }

		public Album Album { get; set; }
    }
}
