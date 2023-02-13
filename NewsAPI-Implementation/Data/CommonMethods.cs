using Microsoft.EntityFrameworkCore;
using NewsAPI_Implementation.Models;
using NewsAPI.Constants;

namespace NewsAPI_Implementation.Data
{
    public class CommonMethods
    {

        private MyDbContext _context;
        public CommonMethods(MyDbContext context)
        {
            _context = context;
        }

        public User GetUser(string user) => _context.Users.Where(u => u.Username == user).FirstOrDefault();

        public void UpdateUserPremium(string username)
        {
            var user = GetUser(username);
            user.IsPremium = true;
            UpdateUser(user);
        }

        public void ChangeUserLanguage(string username, Languages lang)
        {
            var user = GetUser(username);
            user.PreferedLanguage = lang;
            UpdateUser(user);
        }

        private void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
