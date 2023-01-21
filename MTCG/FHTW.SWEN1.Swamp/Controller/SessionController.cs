using MTCG;
using MTCG.Authentication;
using MTCG.Models;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace MTCG.Controller
{
    public class SessionController
    {
        //If the path is "/sessions" and the method is "POST", the code will check the user name and password with the username and password in Db and 
        //if they match, write a login successful in console and save a tocken for the user.  
        public static void Login(object evt, NpgsqlConnection _Cn)
        {
            HttpSvrEventArgs e = (HttpSvrEventArgs)evt;

            if (e.Method == "POST")                                              // POST: add new message
            {
                NpgsqlCommand cmd = _Cn.CreateCommand();

                // To check if the provided username exists in the "users" table.
                cmd.CommandText = "SELECT * FROM users WHERE username = @username";

                var user = new Users();
                JObject jObject = JObject.Parse(e.Payload);
                var username = user.UserName;
                username = (string)jObject["Username"];
                var password = user.Password;
                password = (string)jObject["Password"];

                // make a hasched password from the password which user gives
                PasswordHandler passwordhandler = new PasswordHandler();
                string curlHashedPassword = passwordhandler.HashPassword(password);

                IDataParameter p1 = cmd.CreateParameter();                      // make and bind parameter for username
                p1.ParameterName = ":username";
                p1.Value = username;
                cmd.Parameters.Add(p1);

                // to see if the returned data reader has rows (meaning a user with the provided username exists)
                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    var hashedpassword = reader["password"].ToString();
                    reader.Close();

                    // To see if the provided password matches the hashed password stored in the table for the given username.
                    if (hashedpassword == curlHashedPassword)
                    {
                        var token = "Basic " + username + "-mtcgToken";         // Generate a unique token for the username

                        // Add the token to the user in the database
                        cmd.CommandText = "UPDATE users SET token = @token WHERE username = @username";
                        IDataParameter p2 = cmd.CreateParameter();
                        p2.ParameterName = ":token";
                        p2.Value = token;
                        cmd.Parameters.Add(p2);
                        cmd.ExecuteNonQuery();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("User with username: \"" + username + "\" is logged in!");
                        Console.WriteLine("The token for the " + username + " is " + token);
                        Console.ForegroundColor = ConsoleColor.White;


                        // Send the token back to the client
                        e.Reply(200, "Successful login", token);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Incorrect password for user: \"" + username + "\"!");
                        Console.ForegroundColor = ConsoleColor.White;
                        e.Reply(400, "Error: Incorrect password");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: User with username: \"" + username + "\" does not exist!");
                    Console.ForegroundColor = ConsoleColor.White;
                    e.Reply(400, "Error: User does not exist");
                }

                cmd.Dispose();
            }
        }
    }
}

