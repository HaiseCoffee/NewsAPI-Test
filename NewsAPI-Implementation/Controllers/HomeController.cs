using Microsoft.AspNetCore.Mvc;
using NewsAPI.Constants;
using NewsAPI.Models;
using NewsAPI;
using NewsAPI_Implementation.Controllers;
using NewsAPI_Implementation.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using NewsAPI_Implementation.Services;
using Braintree;
using NewsAPI_Implementation.Data;

//TODO: ADD FILTER TO SEARCH BY LANGUAGE
namespace NewsAPI_Implementation.Controllers
{
    public class HomeController : Controller
    {
        private NewsApiClient newsApiClient = new NewsApiClient("f9594db5a39d425b94ed8b7ebf6a7d9f");

        private readonly ILogger<HomeController> _logger;

        private readonly IBraintreeService _braintreeService;

        private MyDbContext _context;

        private CommonMethods _commonMethods;


        public HomeController(ILogger<HomeController> logger, IBraintreeService braintreeService, MyDbContext context)
        {
            _logger = logger;
            _braintreeService = braintreeService;
            _context = context;
            _commonMethods = new CommonMethods(context);
        }
        
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
            {
                Q = "World",
                SortBy = SortBys.Popularity,
                Language = GetLanguage(),
                Page = !IsLoggedIn() ? 1 : page,
                PageSize = pageSize
            });

            int i = 1;
            foreach (var article in articlesResponse.Articles) {
                article.Source.Id = i.ToString();
                i++;
            }
            
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(articlesResponse);
        }

        [HttpPost]
        public JsonResult ChangeLanguage(int language)
        {
            var lang = (Languages)language;
            _commonMethods.ChangeUserLanguage(HttpContext.Session.GetString("Username"), lang);
            HttpContext.Session.SetInt32("PreferedLanguage", language);
            var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
            {
                Q = "World",
                SortBy = SortBys.Popularity,
                Language = lang
            });

            return Json(articlesResponse);
        }

        [HttpGet]
        public ActionResult SearchTopic (string searchValue)
        {
            var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
            {
                Q = searchValue,
                SortBy = SortBys.Popularity,
                Language = GetLanguage()
            });

            return View("Index", articlesResponse);
        }

        public IActionResult GetPremium()
        {
            if (IsLoggedIn())
            {
                var gateway = _braintreeService.GetGateway();
                var clientToken = gateway.ClientToken.Generate();  //Genarate a token
                ViewBag.ClientToken = clientToken;
                var data = new UserPurchaseVM
                {
                    Nonce = ""
                };

                return View(data);
            } else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Payment(UserPurchaseVM us)
        {
            var gateway = _braintreeService.GetGateway();
            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal("10"),
                PaymentMethodNonce = us.Nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);

            if (result.IsSuccess())
            {
                _commonMethods.UpdateUserPremium(HttpContext.Session.GetString("Username"));
                return View("Success");
            }
            else
            {
                return View("Error");
            }
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

        private Languages GetLanguage()
        {
            int? preferedLanguage = HttpContext.Session.GetInt32("PreferedLanguage");
            var languages = preferedLanguage.HasValue ? (Languages)preferedLanguage.Value : Languages.ES;

            return languages;
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetString("Username") != null;
        }
    }
}