using FuNews.BLL.Interface;
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

        public async Task OnGetAsync()
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

        public string GetRoleName(int? role)
        {
            return role switch
            {
                1 => "Lecture",
                2 => "Staff",
                3 => "Admin",
                _ => "Không rõ"
            };
        }
    }
}
