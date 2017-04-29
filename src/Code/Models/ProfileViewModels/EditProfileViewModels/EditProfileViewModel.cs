using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models.ProfileViewModels.EditProfileViewModels
{
    public class EditProfileViewModel
    {
		public string FirstName { get; set; }

		public string LastName { get; set; }

		[MaxLength(100)]
		public string Location { get; set; }

		[Range(0, 120)]
		public string Age { get; set; }

		[MaxLength(160)]
		public string Description { get; set; }
    }
}
