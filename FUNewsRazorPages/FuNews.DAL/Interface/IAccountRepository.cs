using FuNews.Modals.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.DAL.Interface
{
    public interface IAccountRepository : IBaseRepository<SystemAccount, short>
    {

        Task<SystemAccount> login(string email, string password);  
    }
}
