using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MTCG_Web_API.Authentication;
using MTCG_Web_API.Models.Users;
using Npgsql;
using System.Data;
using System;
using System.Xml.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;

namespace MTCG_Web_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        // To read the connection settings we need to 
        // dependency injection
        private readonly IConfiguration _configuration;
        // Dependency injection to get the application path to photos' folder
        private readonly IWebHostEnvironment _environment;

        private readonly UserManager<ApplicationUser> userManager;

        //, UserManager<ApplicationUser> userManager
        public SessionsController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        [HttpPost]
        public JsonResult Post(User user)
        {
            string query = @"
                SELECT username, password FROM users
                WHERE username = @username AND password = @password
             ";

            string hashedpassword = HashPassword(user.Password);

            // Getting data into the data table object
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ConnString");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("username", user.UserName);
                    myCommand.Parameters.AddWithValue("password", hashedpassword);
                    myReader = myCommand.ExecuteReader();
                    //table.Load(myReader);
                    if (myReader.Read())
                    {
                        // The username and password were found in the table, so the login is successful
                        myReader.Close();
                        myCon.Close();
                        return new JsonResult("login successful");
                    }
                    else
                    {
                        // The username and password were not found in the table, so the login has failed
                        myReader.Close();
                        myCon.Close();
                        return new JsonResult("login faild");
                    }
                }
            }
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
