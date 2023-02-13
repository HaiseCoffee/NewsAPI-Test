using NewsAPI.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAPI_Implementation.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public bool IsPremium { get; set; }
        public Languages PreferedLanguage { get; set; }

        public void HashPassword()
        {
            // A higher work factor makes the hash more secure but also slower to generate. The recommended work factor is 12 or higher. 
            Password = BCrypt.Net.BCrypt.HashPassword(Password, 12);
        }

        public User() { }

        public User(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
            IsPremium = false;
            PreferedLanguage = Languages.ES;
        }
    }
}
