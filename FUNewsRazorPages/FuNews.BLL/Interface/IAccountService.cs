using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.BLL.Interface
{
    public interface IAccountService
    {
        Task<AccountResponse> login(LoginRequest request);
		Task<AccountResponse> createAccount(CreateAccountRequest request);
	}
}
