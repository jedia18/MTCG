using MTCG.Controller;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace MTCG
{
    public class IncomingHandler
    {
        /// <summary>Database connection.</summary>
        public static IDbConnection _Cn;

        /// <summary>Initializes the database connection.</summary>
        public static void InitDb()
        {
            _Cn = new NpgsqlConnection("Server=localhost;Database=MTCG2-DB;Port=5432;User Id=postgres;Password=@Qedipost3@;");
            _Cn.Open();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nConnected with Database!\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Please run the CURL!\n");
        }

        /// <summary>Processes an incoming HTTP request.</summary>
        /// <param name="evt">Event arguments.</param>
        /// This code is a part of a server application that handles incoming HTTP requests.
        public static void _Svr_Incoming(object evt)
        {
            //The method takes an object "evt" which is cast to an HttpSvrEventArgs object, which contains 
            //information about the incoming request. 
            HttpSvrEventArgs e = (HttpSvrEventArgs)evt;
            StringHandler token = new StringHandler();
            StringHandler distinguisher = new StringHandler();

            switch (e.Path)
            {
                case "/users":                                                  // The code checks if the path of the request is "/users"
                    UserController.AddUser(e, (NpgsqlConnection)_Cn);           // AddUser method to handle User
                    break;

                case "/sessions":                                               // The code checks if the path of the request is "/sessions".
                    SessionController.Login(e, (NpgsqlConnection)_Cn);          // Login the user and make a token and assign it to the user
                    break;

                case "/packages":                                               // The code checks if the path of the request is "/packages".
                    for (int j = 0; j < e.Headers.Length; j++)
                    {
                        
                        if (token.TokenDistinguisher(e.Headers[j].Value) == "admin-mtcgToken")
                        {
                            PackageController.AddPackage(e, (NpgsqlConnection)_Cn);
                        }
                    }
                    break;

                case "/transactions/packages":
                    for (int j = 0; j < e.Headers.Length; j++)
                    {
                        if (token.TokenDistinguisher(e.Headers[j].Value) == "kienboec-mtcgToken")
                        {
                            string user = distinguisher.UserDistinguisher(e.Headers[j].Value);
                            CardAcquireController.CardAcquire(e, (NpgsqlConnection)_Cn, user);
                        }
                        else if (token.TokenDistinguisher(e.Headers[j].Value) == "altenhof-mtcgToken")
                        {
                            string user = distinguisher.UserDistinguisher(e.Headers[j].Value);
                            CardAcquireController.CardAcquire(e, (NpgsqlConnection)_Cn, user);
                        }
                    }
                    break;

                //case "/cards":
                //    for (int j = 0; j < e.Headers.Length; j++)
                //    {
                //        if (token.TokenDistinguisher(e.Headers[j].Value) == "kienboec-mtcgToken")
                //        {
                //            // TODO
                //        }
                //        else if (token.TokenDistinguisher(e.Headers[j].Value) == "altenhof-mtcgToken")
                //        {
                //            // TODO
                //        }
                //    }
                //    break;

                default:
                    Console.WriteLine("Rejected message.");
                    e.Reply(400);
                    break;
            }




            Console.WriteLine();
        }
    }
}