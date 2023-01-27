using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using MTCG.Models;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MTCG.Authentication;

namespace MTCG.Controller
{
    public class UserController
    {
        //If the path is "/users" and the method is "POST", the code will creat the users table in Db and insert some values into the
        //database, with the values taken from the payload of the request. 
        public static void AddUser(object evt, NpgsqlConnection _Cn)
        {
            HttpSvrEventArgs e = (HttpSvrEventArgs)evt;

            if (e.Method == "POST")                                              // POST: add new message
            {
                NpgsqlCommand cmd = _Cn.CreateCommand();

                // create database command, insert messages into database
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS users (id SERIAL PRIMARY KEY, username VARCHAR(255) UNIQUE, password VARCHAR(255), coins INTEGER, wins INTEGER, defeats INTEGER, draws INTEGER, played INTEGER, Image VARCHAR(255), bio VARCHAR(255), token VARCHAR(255))";
                cmd.ExecuteNonQuery();

                // insert message into database
                cmd.CommandText = "INSERT INTO users (username, password, coins) VALUES (@username, @password, 20) RETURNING id";

                Users user = new Users();
                JObject jObject = JObject.Parse(e.Payload);
                _ = user.UserName;
                string username = (string)jObject["Username"];
                _ = user.Password;
                string password = (string)jObject["Password"];

                IDataParameter p1 = cmd.CreateParameter();                      // make and bind parameter for username
                p1.ParameterName = ":username"; 
                p1.Value = username;
                cmd.Parameters.Add(p1);

                IDataParameter p2 = cmd.CreateParameter();                      // make and bind parameter for password                       
                PasswordHandler passwordhandler = new PasswordHandler();        // make a hasched password from the password which user gives
                string hashedpassword = passwordhandler.HashPassword(password);
                p2.ParameterName = ":password";
                p2.Value = hashedpassword;
                cmd.Parameters.Add(p2);

                try
                {
                    cmd.ExecuteNonQuery();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("User with username: \"" + username + "\" is registerd!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch (NpgsqlException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: The username: \"" + username + "\" is already exist!");
                    Console.ForegroundColor = ConsoleColor.White;

                    if (ex.ErrorCode.Equals(23505))
                    {
                        Console.WriteLine("Error: The username already exists.");
                        e.Reply(400, "Error: The username already exists.");
                    }
                }

                cmd = (NpgsqlCommand)_Cn.CreateCommand();     // The code then retrieves the auto-incremented ID
                cmd.CommandText = "SELECT CURRVAL('users_id_seq')";         // read autoincremented message ID
                int id = Convert.ToInt32(cmd.ExecuteScalar());              // put ID into variable
                cmd.Dispose();
                //Console.WriteLine("Saved message: \"{0}\" as {1}", e.Payload.Replace("\n", "").Replace("\r", ""), id);
                e.Reply(200, id.ToString());                                // create reply

                cmd.Dispose();
            }

        }
    }
}
