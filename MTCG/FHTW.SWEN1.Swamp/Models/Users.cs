using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MTCG.Models
{
    internal class Users
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; private set; }
        [Required(ErrorMessage = "Password Name is required")]
        public string Password { get; set; }
        public int Coins { get; set; }
        public int Wins { get; set; }
        public int Defeats { get; set; }
        public int Draws { get; set; }
        public int Played { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
    }
}
