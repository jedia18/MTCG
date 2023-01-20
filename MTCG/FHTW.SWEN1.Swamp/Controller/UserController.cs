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
    internal class UserController
    {

        public static void AddUser(object evt, NpgsqlConnection _Cn)
        {
            HttpSvrEventArgs e = (HttpSvrEventArgs)evt;

            //If the path is "/messages" and the method is "POST", the code will insert a new message into the
            //database using an IDbCommand object, with the message data taken from the payload of the request. // no message id provided
            if (e.Method == "POST")                                         // POST: add new message
            {
                NpgsqlCommand cmd = _Cn.CreateCommand();

                // create database command, insert messages into database
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS users (id SERIAL PRIMARY KEY, username VARCHAR(255) UNIQUE, password VARCHAR(255), coins INTEGER, wins INTEGER, defeats INTEGER, draws INTEGER, played INTEGER, Image VARCHAR(255), bio VARCHAR(255))";
                cmd.ExecuteNonQuery();

                // insert message into database
                cmd.CommandText = "INSERT INTO users (username, password, coins) VALUES (@username, @password, 20) RETURNING id";

                var user = new Users();
                JObject jObject = JObject.Parse(e.Payload);
                var username = user.UserName;
                username = (string)jObject["Username"];
                var password = user.Password;
                password = (string)jObject["Password"];
                    
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

                cmd.Dispose();
            }

        }
    }
}
