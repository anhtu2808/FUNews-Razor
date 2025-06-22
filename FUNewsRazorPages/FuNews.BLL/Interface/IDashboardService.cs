using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.BLL.Interface
{
    public interface IDashboardService
    {
        Task<NewsDashboardResponse> GetNewsDashboard(NewsDashboardRequest request);
    }
}
