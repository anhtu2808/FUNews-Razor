using FuNews.BLL.Interface;
using FuNews.Modals.DTOs.Request;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FUNewsRazorPages.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;
        public LoginModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public LoginRequest Input { get; set; }
        public string ErrorMessage { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Input = new()
            {
                Emaill = Input.Emaill,
                Password = Input.Password
            };

            var user = await _accountService.login(Input);
            if (user == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, user.AccountName),
            new Claim(ClaimTypes.Role, user.AccountRole.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);

            HttpContext.Session.SetString("AccountEmail", user.AccountEmail);
            return RedirectToPage("/Index");
        }
    }
}
