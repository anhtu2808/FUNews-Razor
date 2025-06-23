using FuNews.BLL.Interface;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FUNewsRazorPages.SignalR.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsRazorPages.Pages.Account
{
    public class ManageAccountModel : PageModel
    {
        private readonly IAccountService _accountService;

        public ManageAccountModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public List<AccountResponse> Accounts { get; set; } = new();

        [BindProperty]
        public CreateAccountRequest CreateAccount { get; set; } = new();

        [BindProperty]
        public UpdateAccountRequest UpdateAccount { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadAccountsAsync();
		}

        private async Task LoadAccountsAsync()
        {
            Accounts = await _accountService.GetAllAccount();
			foreach (var acc in Accounts)
			{
				if (!string.IsNullOrEmpty(acc.AccountEmail))
				{
					acc.IsOnline = UserHub.OnlineUsers.Values.Contains(acc.AccountEmail);
					// hoặc nếu bạn tách Dictionary riêng thì dùng: UserConnectionStore.OnlineUsers
				}
			}
		}

        // Handle Create
        public async Task<IActionResult> OnPostCreateAsync()
        {
            // if (!ModelState.IsValid)
            // {
            //     await LoadAccountsAsync();
            //     return Page();
            // }

            try
            {
                await _accountService.createAccount(CreateAccount);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadAccountsAsync();
                return Page();
            }
        }

        // Handle Edit
        public async Task<IActionResult> OnPostEditAsync(short id)
        {
            // if (!ModelState.IsValid)
            // {
            //     await LoadAccountsAsync();
            //     return Page();
            // }

            try
            {
                await _accountService.updateAccount(id, UpdateAccount);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadAccountsAsync();
                return Page();
            }
        }

        // Handle Delete
        public async Task<IActionResult> OnPostDeleteAsync(short id)
        {
            try
            {
                await _accountService.deleteAccount(id);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadAccountsAsync();
                return Page();
            }
        }

        public string GetRoleName(int? role)
        {
            return role switch
            {
                1 => "Staff",
                2 => "Lecturer",
                3 => "Admin",
                _ => "Unknown"
            };
        }
    }
}
