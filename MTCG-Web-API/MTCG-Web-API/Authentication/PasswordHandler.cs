using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Security.Cryptography;
using System.Text;
using static System.Console;  // with this namespace, no need to write Console each time to read or write
namespace MTCG_Web_API.Authentication
{
    public class PasswordHandler
    {
        public void Login()
        {

        }

        public void Register()
        {

        }

        public string HashPassword(string password)
        {
            // SHA256 : Secure Hash Algorithem to 256
            SHA256 hash = SHA256.Create();  // create an Instance of this class
            var passwordBytes = Encoding.Default.GetBytes(password); // Change password string to an arry of bytes to use in computeHash() method
            var hashedpassword = hash.ComputeHash(passwordBytes);  // this method only take an array of beytes and hash it and it returns an array of bytes
            string hashedPasswordString = BitConverter.ToString(hashedpassword).Replace("-", "");
            return hashedPasswordString;
        }

    }
}
