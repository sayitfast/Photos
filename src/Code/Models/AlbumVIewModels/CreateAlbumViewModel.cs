namespace Code.Models.AlbumVIewModels
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class CreateAlbumViewModel
    {
		[Required]
		public string Name { get; set; }

		[Required]
		public string Category { get; set; }

		public DateTime CreatedOn { get; set; }

		[MaxLength(160)]
		public string Description { get; set; }

    }
}
