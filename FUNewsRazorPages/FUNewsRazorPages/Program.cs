using FuNews.BLL.Interface;
using FuNews.BLL.Service;
using FuNews.DAL;
using FuNews.DAL.Interface;
using FuNews.DAL.Repository;
using FuNews.Modals.Mapping;
using FUNewsRazorPages.SignalR.NewsArticle;
using Microsoft.EntityFrameworkCore;

namespace FUNewsRazorPages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<FuNewsDbContext>(options =>
             options.UseSqlServer(
                 builder.Configuration.GetConnectionString("DefaultConnection")
             ));

            // Đăng ký Repository
            builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<INewsTagRepository, NewsTagRepository>();
            // Đăng ký Service
            builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<INewsHubService, NewsHubService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();

            // Đăng ký AutoMapper
            builder.Services.AddAutoMapper(typeof(NewsArticleMapper).Assembly);
            builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);
            builder.Services.AddSignalR();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.MapHub<NewsHub>("/newsHub");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
