namespace Code.Models.SingleImageViewModels
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class UploadImageViewModel
    {
		[Required]
		[MaxLength(20)]
		public string Name { get; set; }

		[Required]
		[MaxLength(160)]
		public string Description { get; set; }

		public string Location { get; set; }

		[Required]
		public string Category { get; set; }
	}
}
