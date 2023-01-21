using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;



namespace MTCG.Authentication
{
    internal class PasswordHandler
    {
        public string HashPassword(string password)
        {
            // SHA256 : Secure Hash Algorithem to 256
            SHA256 hash = SHA256.Create();                              // create an Instance of this class
            var passwordBytes = Encoding.Default.GetBytes(password);    // Change password string to an arry of bytes to use in computeHash() method
            var hashedpassword = hash.ComputeHash(passwordBytes);       // this method only take an array of beytes and hash it and it returns an array of bytes
            string hashedPasswordString = BitConverter.ToString(hashedpassword).Replace("-", "");
            return hashedPasswordString;
        }
    }
}
