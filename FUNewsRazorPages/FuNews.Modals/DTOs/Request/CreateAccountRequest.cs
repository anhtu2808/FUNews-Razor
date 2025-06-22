using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.Modals.DTOs.Request
{
	public class CreateAccountRequest
	{
		public string? AccountName { get; set; }
		public string? AccountEmail { get; set; }
		public int? AccountRole { get; set; }
		public string? AccountPassword { get; set; }
	}
}
