using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FuNews.Modals.DTOs.Request
{
	public class CreateAccountRequest
	{
		[Required(ErrorMessage = "Account name is required")]
		[StringLength(100, ErrorMessage = "Account name cannot exceed 100 characters")]
		public string AccountName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email format")]
		[StringLength(70, ErrorMessage = "Email cannot exceed 70 characters")]
		public string AccountEmail { get; set; } = string.Empty;

		[Required(ErrorMessage = "Role is required")]
		[Range(1, 3, ErrorMessage = "Please select a valid role")]
		public int AccountRole { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[StringLength(70, ErrorMessage = "Password cannot exceed 70 characters")]
		public string AccountPassword { get; set; } = string.Empty;
	}
}
