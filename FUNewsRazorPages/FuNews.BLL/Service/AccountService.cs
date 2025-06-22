using FuNews.BLL.Interface;
using FuNews.DAL.Interface;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FuNews.Modals.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.BLL.Service
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRepository;
		private static readonly Random _random = new();

		public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountResponse> login(LoginRequest request)
        {
            var account = await _accountRepository.login(request.Emaill, request.Password);
            return new()
            {
                AccountEmail = account.AccountEmail,
                AccountName = account.AccountName, 
                AccountRole = account.AccountRole,
            };
        }

		public async Task<AccountResponse> createAccount(CreateAccountRequest request)
		{
            var account = new SystemAccount()
            {
                AccountEmail = request.AccountEmail,
                AccountPassword = request.AccountPassword,
                AccountName = request.AccountName,
                AccountRole = request.AccountRole,
            };

            await _accountRepository.AddAsync(account);

            return new()
            {
                AccountRole = account.AccountRole,
                AccountName = account.AccountName,
                AccountEmail = request.AccountEmail,
            };

		}
	}
}
