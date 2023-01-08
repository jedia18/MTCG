using MTCG_Web_API.Models.Cards;
using System.Collections.Generic;

namespace MTCG_Web_API.Models.Users
{
    public class User
    {
        //public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Coins { get; set; }
        public string Bio { get; set; }
        public List<Card> Stack { get;  private set; }
        public List<Card> Deck { get; private set; }


    }
}
