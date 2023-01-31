using Microsoft.AspNetCore.Mvc;
using NewsAPI_Implementation.Data;
using NewsAPI_Implementation.Models;

namespace NewsAPI_Implementation.Controllers
{
    public class UserController : Controller
    {

        //TODO: ADD CARD PAYMENT OR MERCADOPAGO TO CHANGE ISPREMIUM VALUE
        private MyDbContext _context;
        public UserController (MyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult RegisterUser(string user, string email, string pass)
        {
            User newUser = new User(user, pass, email);
            newUser.HashPassword();

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(string user, string pass)
        {
            User searchUser = GetUser(user);
            if (searchUser != null)
            {
                if(BCrypt.Net.BCrypt.Verify(pass, searchUser.Password))
                {
                    HttpContext.Session.SetString("Username", searchUser.Username);
                    HttpContext.Session.Set("IsPremium", BitConverter.GetBytes(searchUser.IsPremium));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("Login");
                }
            }
            else
            {
                return View("Login");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("IsPremium");
            return View("Login");
        }
        private User GetUser(string user)
        {
            return _context.Users.Where(u => u.Username == user).FirstOrDefault();
        }
    }
}
