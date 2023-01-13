using MTCG_Web_API.Models.Cards;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MTCG_Web_API.Models.Users
{
    public class User
    {
        [Required (ErrorMessage = "User Name is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password Name is required")]
        public string Password { get; set; }
        public int Coins { get; set; }
        public string Bio { get; set; }
        public List<Card> Stack { get;  private set; }
        public List<Card> Deck { get; private set; }


    }
}
