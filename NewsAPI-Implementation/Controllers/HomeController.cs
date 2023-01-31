using Microsoft.AspNetCore.Mvc;
using NewsAPI.Constants;
using NewsAPI.Models;
using NewsAPI;
using NewsAPI_Implementation.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

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
                Q = "World",
                SortBy = SortBys.Popularity,
                Language = Languages.ES
            });

            int i = 1;
            foreach (var article in articlesResponse.Articles) {
                article.Source.Id = i.ToString();
                i++;
            }
        
            return View(articlesResponse);
        }

        [HttpPost]
        public JsonResult ChangeLanguage(int language)
        {
            var lang = (Languages)language;
            // Do something with the selected language, such as saving it to the user's session or profile
            var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
            {
                Q = "World",
                SortBy = SortBys.Popularity,
                Language = lang
            });

            HttpContext.Session.SetInt32("Language", language);
            return Json(articlesResponse);
        }

        [HttpGet]
        public ActionResult SearchTopic (string searchValue)
        {
            var languages = (Languages)HttpContext.Session.GetInt32("Language");

            var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
            {
                Q = searchValue,
                SortBy = SortBys.Popularity,
                Language = languages != null ? languages : Languages.ES
            });

            return View("Index", articlesResponse);
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