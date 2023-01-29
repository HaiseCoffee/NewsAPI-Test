using Microsoft.AspNetCore.Mvc;
using NewsAPI.Models;

namespace NewsAPI_Implementation.Controllers
{
    public class ArticleController : Controller
    { 
        public IActionResult ExpandedArticle(Article art)
        {

            return View(art);
        }
    }
}
