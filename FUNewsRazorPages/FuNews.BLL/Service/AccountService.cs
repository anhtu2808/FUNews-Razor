using AutoMapper;
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
        private IMapper _mapper;

		public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<AccountResponse> login(LoginRequest request)
        {
            var account = await _accountRepository.login(request.Emaill, request.Password);
            return new()
            {
                AccountEmail = account.AccountEmail,
                AccountName = account.AccountName, 
                AccountRole = account.AccountRole,
                AccountId = account.AccountId,
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

        public async Task<List<AccountResponse>> GetAllAccount()
        {
            var accounts = await _accountRepository.GetAllAsync();
            List< AccountResponse > responses = new List< AccountResponse >();
            foreach (var account in accounts) 
            {
                AccountResponse response = new()
                {
                    AccountId = account.AccountId,
                    AccountEmail = account.AccountEmail,
                    AccountName = account.AccountName,
                    AccountRole = account.AccountRole
                };
                responses.Add(response);

			}
            return responses;


		}
	}
}
