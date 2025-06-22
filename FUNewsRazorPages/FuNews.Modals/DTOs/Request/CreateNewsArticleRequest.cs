using Microsoft.AspNetCore.Http;

namespace FuNews.Modals.DTOs.Request;

public class CreateNewsArticleRequest
{
	public string? NewsTitle { get; set; }
	public string? Headline { get; set; }

	public string? NewsContent { get; set; }
	public string? NewsSource { get; set; }
    public IFormFile? UrlThumbnailsFile { get; set; } 
    public string? UrlThumbnailsPath { get; set; } 
    public short? CategoryId { get; set; }
	public List<int>? TagIds { get; set; }
}