using Microsoft.AspNetCore.Mvc;
using NewsAPI.Constants;
using NewsAPI.Models;
using NewsAPI;
using NewsAPI_Implementation.Models;
using System.Diagnostics;

//TODO: ADD FILTER TO SEARCH BY LANGUAGE
namespace NewsAPI_Implementation.Controllers
{
    public class HomeController : Controller
    {
        private NewsApiClient newsApiClient = new NewsApiClient("f9594db5a39d425b94ed8b7ebf6a7d9f");

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
            {
                Q = "Apple",
                SortBy = SortBys.Popularity,
                Language = Languages.EN
            });
        
            return View(articlesResponse);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}