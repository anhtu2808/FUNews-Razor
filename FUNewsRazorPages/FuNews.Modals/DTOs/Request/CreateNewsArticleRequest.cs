namespace FuNews.Modals.DTOs.Request;

public class CreateNewsArticleRequest
{
	public string? NewsTitle { get; set; }
	public string Headline { get; set; }

	public string? NewsContent { get; set; }
	public string? NewsSource { get; set; }
	public string? UrlThumbnails { get; set; }
	public short? CategoryId { get; set; }
}