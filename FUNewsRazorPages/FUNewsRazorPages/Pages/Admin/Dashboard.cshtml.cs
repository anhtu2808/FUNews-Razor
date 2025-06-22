using FuNews.BLL.Interface;
using FuNews.Modals.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace FUNewsRazorPages.Pages.Dashboard
{
    public class DashboardModel : PageModel
    {
        public int TotalPublic { get; set; }
        public int TotalPending { get; set; }
        public List<string> CategoryNames { get; set; } = new();
        public List<int> CategoryCounts { get; set; } = new();

        public List<string> TagNames { get; set; } = new();
        public List<int> TagCounts { get; set; } = new();
        private readonly IDashboardService _dashboardService;
        public DashboardModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [BindProperty(SupportsGet = true)]
        public DateTime? startDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? endDate { get; set; }
        public async Task OnGetAsync()
        {
            var request = new NewsDashboardRequest
            {
                startDate = startDate,
                endDate = endDate
            };

            var dashboardData = await _dashboardService.GetNewsDashboard(request);

            // Binding sang biến dùng cho Razor view
            TotalPublic = dashboardData.TotalPublic;
            TotalPending = dashboardData.TotalPending;

            CategoryNames = dashboardData.CategoryDashboard.Select(c => c.CategoryNames).ToList();
            CategoryCounts = dashboardData.CategoryDashboard.Select(c => c.CategoryCounts).ToList();

            TagNames = dashboardData.TagDashboard.Select(t => t.TagNames).ToList();
            TagCounts = dashboardData.TagDashboard.Select(t => t.TagCounts).ToList();

        }

    }
}
