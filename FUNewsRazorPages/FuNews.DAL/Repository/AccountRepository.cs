using FuNews.DAL.Interface;
using FuNews.Modals.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.DAL.Repository
{
    public class AccountRepository : BaseRepository<SystemAccount, short>, IAccountRepository
    {
        public AccountRepository(FuNewsDbContext _context) : base(_context)
        {
        }

        public async Task<SystemAccount?> login(string email, string password)
        {
            email = email.Trim();
            password = password.Trim();
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(sa => sa.AccountEmail == email && sa.AccountPassword == password);

        }

    }
}
