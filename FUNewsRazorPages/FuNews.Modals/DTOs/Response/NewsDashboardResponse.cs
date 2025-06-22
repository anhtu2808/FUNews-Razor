using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.Modals.DTOs.Response
{
    public class NewsDashboardResponse
    {
        public int TotalPublic { get; set; }
        public int TotalPending { get; set; }
        public List<CategoryDashboardResponse> CategoryDashboard { get; set; }
        public List<TagDashboardResponse> TagDashboard { get; set; }
    }
}
