using MTCG.Controller;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml.Linq;

namespace MTCG
{
    internal class IncomingHandler
    {
        /// <summary>Database connection.</summary>
        public static IDbConnection _Cn;

        /// <summary>Initializes the database connection.</summary>
        public static void InitDb()
        {
            _Cn = new NpgsqlConnection("Server=localhost;Database=MTCG2-DB;Port=5432;User Id=postgres;Password=@Qedipost3@;");
            _Cn.Open();
            Console.WriteLine("Connected with Database!");

        }

        /// <summary>Processes an incoming HTTP request.</summary>
        /// <param name="evt">Event arguments.</param>
        /// This code is a part of a server application that handles incoming HTTP requests.
        public static void _Svr_Incoming(object evt)
        {
            //The method takes an object "evt" which is cast to an HttpSvrEventArgs object, which contains 
            //information about the incoming request. 
            HttpSvrEventArgs e = (HttpSvrEventArgs)evt;

            switch (e.Path)
            {
                case "/users":
                    UserController.AddUser(e, (NpgsqlConnection)_Cn);
                    break;

                case "/sessions":
                    SessionController.Login(e, (NpgsqlConnection)_Cn);
                    break;

                case "/packages":
                    for (int j = 0; j < e.Headers.Length; j++)
                    {
                        if (e.Headers[j].Value == "Basic admin-mtcgToken")
                        {
                            PackageController.AddPackage(e, (NpgsqlConnection)_Cn);
                        }
                    }
                    break;

                default:
                    Console.WriteLine("Rejected message.");
                    e.Reply(400);
                    break;
            }





            //if (e.Path == "/users")                                         // The code checks if the path of the request is "/users".
            //{
            //    UserController.AddUser(e, (NpgsqlConnection)_Cn);           // AddUser method to handle User
            //}
            //else if (e.Path == "/sessions")                                 // The code checks if the path of the request is "/sessions".
            //{
            //    SessionController.Login(e, (NpgsqlConnection)_Cn);          // Login the user and make a token and assign it to the user
            //}
            //else if (e.Path == "/packages")                                 // The code checks if the path of the request is "/packages".
            //{
            //    for (int j = 0; j < e.Headers.Length; j++)
            //    {
            //        if (e.Headers[j].Value == "Basic admin-mtcgToken")
            //        {
            //            PackageController.AddPackage(e, (NpgsqlConnection)_Cn);
            //        }
            //    }
            //}
            //else if (e.Path == "/packages")                                 // The code checks if the path of the request is "/packages".
            //{
            //    for (int j = 0; j < e.Headers.Length; j++)
            //    {
            //        if (e.Headers[j].Value == "Basic kienboec-mtcgToken")
            //        {
            //            // TODO
            //        }
            //    }
            //}
            //else if (e.Path == "/packages")                                 // The code checks if the path of the request is "/packages".
            //{
            //    for (int j = 0; j < e.Headers.Length; j++)
            //    {
            //        if (e.Headers[j].Value == "Basic kienboec-mtcgToken")
            //        {
            //            // TODO
            //        }
            //    }
            //}




            //else
            //{
            //    Console.WriteLine("Rejected message.");
            //    e.Reply(400);
            //}

            Console.WriteLine();
        }
    }
}
