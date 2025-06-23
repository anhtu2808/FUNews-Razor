using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsRazorPages.Pages.Account
{
    public class LogoutModel : PageModel
    {
		public async Task<IActionResult> OnPostAsync()
		{
			// Clear session data
			HttpContext.Session.Clear();
			
			await HttpContext.SignOutAsync("Cookies");
			return RedirectToPage("/Index");
		}
	}
}
