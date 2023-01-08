using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.IO;
using System.Numerics;
using System;
using MTCG_Web_API.Models.Users;
using MTCG_Web_API.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace MTCG_Web_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // To read the connection settings we need to 
        // dependency injection
        private readonly IConfiguration _configuration;
        // Dependency injection to get the application path to photos' folder
        private readonly IWebHostEnvironment _environment;

        private readonly UserManager<ApplicationUser> userManager;

        //, UserManager<ApplicationUser> userManager
        public UsersController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select UserName as ""UserName"",
                       Password as ""Password"",
                       Coins as ""Coins"",
                       Bio as ""Bio""
                from users
             ";

            // Getting data into the data table object
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ConnString");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(User user)
        {
            string query = @"
                insert into users(UserName,Password,Coins,Bio)
                values              (@UserName,@Password,@Coins,@Bio)
            ";

            // Getting data into the data table object 
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ConnString");
            NpgsqlDataReader myReader;
            if (user.Bio == null)
            {
                user.Bio = "null";
            }

            // make a hasched password from the password which user gives
            string hashedpassword = HashPassword(user.Password);

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@UserName", user.UserName);
                    myCommand.Parameters.AddWithValue("@Password", hashedpassword);
                    //myCommand.Parameters.AddWithValue("@Password", user.Password);
                    myCommand.Parameters.AddWithValue("@Coins", Convert.ToInt32(user.Coins));
                    myCommand.Parameters.AddWithValue("@Bio", user.Bio);
                    try
                    {
                        myReader = myCommand.ExecuteReader();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "23505: doppelter Schlüsselwert verletzt Unique-Constraint »users_pkey«")
                        {
                            return new JsonResult("User is already exist!");
                        }
                        else
                        {
                            return new JsonResult(ex);
                        }
                        
                    }
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(User user)
        {
            string query = @"
                update users
                set UserName = @UserName,
                    Password = @Password,
                    Coins = @Coins,
                    Bio = @Bio
                where UserName = @UserName
             ";

            // Getting data into the data table object
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ConnString");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    //myCommand.Parameters.AddWithValue("@PlayerId", pla.UserId);
                    myCommand.Parameters.AddWithValue("@UserName", user.UserName);
                    myCommand.Parameters.AddWithValue("@Password", user.Password);
                    myCommand.Parameters.AddWithValue("@Coins", Convert.ToInt32(user.Coins));
                    myCommand.Parameters.AddWithValue("@Bio", user.Bio);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{UserName}")]  // It have to accept id in URL
        public JsonResult Delete(string username)
        {
            string query = @"
                delete from users
                where UserName = @UserName
             ";

            // Getting data into the data table object
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ConnString");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@UserName", username);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Delete Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPaht = _environment.ContentRootPath + "/Photos/" + filename;
                using (var stream = new FileStream(physicalPaht, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                // once the file has saved, we will return the filename
                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
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
