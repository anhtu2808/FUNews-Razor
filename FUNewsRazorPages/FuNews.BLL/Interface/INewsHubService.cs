using FuNews.Modals.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.BLL.Interface
{
	public interface INewsHubService
	{
		Task NewsCreatedAsync(NewsArticleResponse dto);
		Task NewsUpdatedAsync(NewsArticleResponse dto);
	}
}
