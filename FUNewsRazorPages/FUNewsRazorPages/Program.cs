using FuNews.BLL.Interface;
using FuNews.BLL.Service;
using FuNews.DAL;
using FuNews.DAL.Interface;
using FuNews.DAL.Repository;
using FuNews.Modals.Mapping;
using FUNewsRazorPages.SignalR.NewsArticle;
using FUNewsRazorPages.SignalR.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FuNews.Modals.Entity;
using FuNews.Modals.DTOs;
using FuNews.Modals.DTOs.Request;
namespace FUNewsRazorPages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
			builder.WebHost.UseUrls("http://0.0.0.0:5017");
			
			// Add services to the container.
			builder.Services.AddRazorPages();
            
            // Add CORS for better network connectivity
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddDbContext<FuNewsDbContext>(options =>
             options.UseSqlServer(
                 builder.Configuration.GetConnectionString("DefaultConnection")
             ));
            builder.Services.AddAuthentication("Cookies")
                 .AddCookie("Cookies", options =>
                {
                    options.LoginPath = "/Account/Login";
                });
            // Đăng ký Repository
            builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<INewsTagRepository, NewsTagRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            // Đăng ký Service
            builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<INewsHubService, NewsHubService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            // Đăng ký AutoMapper
            builder.Services.AddAutoMapper(typeof(NewsArticleMapper).Assembly);
            builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);
            
            // Enhanced SignalR configuration for network access
            builder.Services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.KeepAliveInterval = TimeSpan.FromSeconds(10);
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
                options.HandshakeTimeout = TimeSpan.FromSeconds(15);
            });
            
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            var app = builder.Build();

			var adminConfig = builder.Configuration.GetSection("AdminAccount");
			string adminEmail = adminConfig["Email"];
			string adminPassword = adminConfig["Password"]; // lưu ý nên mã hóa

			using (var scope = app.Services.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<FuNewsDbContext>();
				var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

				var exists = db.SystemAccounts.Any(a => a.AccountEmail == adminEmail);
				if (!exists)
				{
					// Tùy bạn có mã hóa password hay không
					var request = new CreateAccountRequest()
					{
						AccountEmail = adminEmail,
						AccountPassword = adminPassword,
						AccountName = "Admin",
						AccountRole = 3 // hoặc Enum nếu có
					};
					accountService.createAccount(request); // hoặc tự db.Accounts.Add()
				}
			}

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Use CORS
            app.UseCors();
            
            // SignalR Hub mappings
            app.MapHub<NewsHub>("/newsHub");
            app.MapHub<UserHub>("/userHub");
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();

            app.Run();
        }
    }
}
