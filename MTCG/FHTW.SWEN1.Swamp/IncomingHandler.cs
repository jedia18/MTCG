using MTCG.Controller;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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

            if (e.Path == "/users")                                         // The code checks if the path of the request is "/messages".
            {
                UserController.AddUser(e, (NpgsqlConnection)_Cn);           // AddUser method to handle User

                NpgsqlCommand cmd = (NpgsqlCommand)_Cn.CreateCommand();     // The code then retrieves the auto-incremented ID
                cmd.CommandText = "SELECT CURRVAL('users_id_seq')";         // read autoincremented message ID
                int id = Convert.ToInt32(cmd.ExecuteScalar());              // put ID into variable
                cmd.Dispose();
                //Console.WriteLine("Saved message: \"{0}\" as {1}", e.Payload.Replace("\n", "").Replace("\r", ""), id);
                e.Reply(200, id.ToString());                                // create reply

            }
            else if (e.Path == "/sessions")
            {
                SessionController.Login(e, (NpgsqlConnection)_Cn);

                //NpgsqlCommand cmd = (NpgsqlCommand)_Cn.CreateCommand();     // The code then retrieves the auto-incremented ID
                //cmd.CommandText = "SELECT CURRVAL('users_id_seq')";         // read autoincremented message ID
                //int id = Convert.ToInt32(cmd.ExecuteScalar());              // put ID into variable
                //cmd.Dispose();
                ////Console.WriteLine("Saved message: \"{0}\" as {1}", e.Payload.Replace("\n", "").Replace("\r", ""), id);
                //e.Reply(200, id.ToString());                                // create reply
            }





            //If the path starts with "/messages/", the code will try to parse the message ID from the path, and if it is 
            //a valid ID, it will check if the method is "GET" or "PUT".
            else if (e.Path.StartsWith("/messages/"))
            {
                int msg = -1;
                int.TryParse(e.Path.Substring(10), out msg);

                if (msg == -1)
                {
                    Console.WriteLine("Request malformed.");
                    e.Reply(400);
                }
                else
                {
                    //If the method is "GET" it will read the message from the database with the provided ID, if the 
                    //message is found it will send a response with a status code of 200 and the message as the 
                    //content, if not it will send a response with a status code of 404.
                    if (e.Method == "GET")
                    {
                        //string data = _ReadMessages(msg);

                        //if (string.IsNullOrEmpty(data))
                        //{
                        //    Console.WriteLine("Message #" + msg + " not available.");
                        //    e.Reply(404);
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Showed message #" + msg + ".");
                        //    e.Reply(200, _ReadMessages(msg));
                        //}
                    }
                    //If the method is "PUT" it will update the message in the database with the provided ID and payload, 
                    //if the message is found it will send a response with a status code of 200 and the message as the 
                    //content, if not it will send a response with a status code of 404.
                    else if (e.Method == "PUT")
                    {
                        IDbCommand cmd = _Cn.CreateCommand();                       // create database command, insert message into database
                        cmd.CommandText = "UPDATE MESSAGES SET DATA = :m WHERE ID = :id";
                        IDataParameter p = cmd.CreateParameter();                   // make and bind parameter for message body
                        p.ParameterName = ":m";
                        p.Value = e.Payload;
                        cmd.Parameters.Add(p);

                        p = cmd.CreateParameter();
                        p.ParameterName = ":id";
                        p.Value = msg;
                        cmd.Parameters.Add(p);

                        int n = cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (n < 1)
                        {
                            Console.WriteLine("Failed to update nonexistent message.");
                            e.Reply(404);
                        }
                        else
                        {
                            Console.WriteLine("Updated message #" + msg + ".");
                            e.Reply(200);
                        }
                    }
                    else if (e.Method == "DELETE")
                    {
                        IDbCommand cmd = _Cn.CreateCommand();                       // create database command, insert message into database
                        cmd.CommandText = "DELETE FROM MESSAGES WHERE ID = :id";
                        IDataParameter p = cmd.CreateParameter();                   // make and bind parameter for message body
                        p = cmd.CreateParameter();
                        p.ParameterName = ":id";
                        p.Value = msg;
                        cmd.Parameters.Add(p);

                        int n = cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (n < 1)
                        {
                            Console.WriteLine("Failed to delete nonexistent message.");
                            e.Reply(404);
                        }
                        else
                        {
                            Console.WriteLine("Deleted message #" + msg + ".");
                            e.Reply(200);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Rejected message.");
                e.Reply(400);
            }

            Console.WriteLine();
        }
    }
}
