using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;



namespace MTCG
{
    /// <summary>This is the program class for the application.</summary>
    public static class Program
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Database connection.</summary>
        private static IDbConnection _Cn;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // main entry point                                                                                         //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Entry point.</summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            InitDb();

            HttpSvr svr = new HttpSvr();
            svr.Incoming += _Svr_Incoming;

            svr.Run();
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                    //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Initializes the database connection.</summary>
        public static void InitDb()
        {
            //_Cn = new SQLiteConnection("Data Source=swamp.db;Version=3;");
            _Cn = new NpgsqlConnection("Server=localhost;Database=MTCG2-DB;Port=5432;User Id=postgres;Password=@Qedipost3@;");
            _Cn.Open();
            Console.WriteLine("Connected with Database!");

        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static methods                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Reads messages from database.</summary>
        /// <param name="msg">Message number.</param>
        /// <returns>Message text.</returns>
        
        //private static string _ReadMessages(int msg = -1)
        //{
            //NpgsqlCommand cmd = (NpgsqlCommand)_Cn.CreateCommand();
            ////IDbCommand cmd = _Cn.CreateCommand();                       // create database command, insert message into database
            //cmd.CommandText = "CREATE TABLE IF NOT EXISTS messages (id SERIAL PRIMARY KEY, data TEXT)";
            //cmd.ExecuteNonQuery();
            //cmd.Dispose();

            //cmd = (NpgsqlCommand)_Cn.CreateCommand();
            //cmd.CommandText = "SELECT id, data FROM messages";
            //IDbCommand cmd = _Cn.CreateCommand();
            //cmd.CommandText = "SELECT * FROM MESSAGES";

            //if (msg >= 0)
            //{
            //    cmd.CommandText += " WHERE id = :id";
            //    NpgsqlParameter p = cmd.CreateParameter();
            //    p.ParameterName = ":id";
            //    p.Value = msg;
            //    cmd.Parameters.Add(p);
            //}

            //NpgsqlDataReader re = cmd.ExecuteReader();
            //IDataReader re = cmd.ExecuteReader();

            //StringBuilder rval = new StringBuilder();
            //while (re.Read())
            //{
            //    rval.Append("[");
            //    rval.Append(re.GetInt32(0));
            //    rval.Append("] ");
            //    rval.AppendLine(re.GetString(1));
            //}

            //re.Close();
            //re.Dispose();
            //cmd.Dispose();

        //    return rval.ToString();
        //}


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // event handlers                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Processes an incoming HTTP request.</summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        public static void _Svr_Incoming(object sender, HttpSvrEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(_Svr_Incoming, e);
        }

        /// <summary>Processes an incoming HTTP request.</summary>
        /// <param name="evt">Event arguments.</param>
        /// This code is a part of a server application that handles incoming HTTP requests.
        public static void _Svr_Incoming(object evt)
        {
            //The method takes an object "evt" which is cast to an HttpSvrEventArgs object, which contains 
            //information about the incoming request. 
            HttpSvrEventArgs e = (HttpSvrEventArgs)evt;
            //The method first writes the plain message of the request to the console in gray color. 
            //Console.ForegroundColor = ConsoleColor.Gray;
            //Console.WriteLine(e.PlainMessage);
            //Console.ForegroundColor = ConsoleColor.White;

            //Then the code checks if the path of the request is "/messages".
            
            if (e.Path == "/users")
            {
                //If the path is "/messages" and the method is "POST", the code will insert a new message into the
                //database using an IDbCommand object, with the message data taken from the payload of the request. // no message id provided
                
                if (e.Method == "POST")                                         // POST: add new message
                {
                    NpgsqlCommand cmd = (NpgsqlCommand)_Cn.CreateCommand();

                    // create database command, insert messages into database
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS users (id SERIAL PRIMARY KEY, username VARCHAR(255) UNIQUE, password VARCHAR(255))";
                    cmd.ExecuteNonQuery();

                    // insert message into database
                    cmd.CommandText = "INSERT INTO users (username, password) VALUES (@username, @password) RETURNING id";
                    
                    JObject jObject = JObject.Parse(e.Payload);
                    string username = (string)jObject["Username"];
                    string password = (string)jObject["Password"];

                    IDataParameter p1 = cmd.CreateParameter();          // make and bind parameter for username
                    p1.ParameterName = ":username";
                    p1.Value = username;
                    cmd.Parameters.Add(p1);

                    IDataParameter p2 = cmd.CreateParameter();          // make and bind parameter for password
                    p2.ParameterName = ":password";
                    p2.Value = password;
                    cmd.Parameters.Add(p2);

                    Console.WriteLine("Username: " + username);
                    Console.WriteLine("Password: " + password);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine("Error: The username already exists.");
                        // To Do : make the catch correctly
                        //if (ex.Code == "23505")
                        //{
                        //    Console.WriteLine("Error: The username already exists.");
                        //    e.Reply(400, "Error: The username already exists.");
                        //}
                        //else
                        //{
                        //    throw;
                        //}
                    }


                    //The code then retrieves the auto-incremented ID of the new message and sends a response with 
                    //a status code of 200 and the ID as the content.
                    //cmd = (NpgsqlCommand)_Cn.CreateCommand();                                  // read autoincremented message ID
                    cmd.CommandText = "SELECT CURRVAL('users_id_seq')";
                    int id = Convert.ToInt32(cmd.ExecuteScalar());              // put ID into variable
                    cmd.Dispose();

                    Console.WriteLine("Saved message: \"{0}\" as {1}", e.Payload.Replace("\n", "").Replace("\r", ""), id);
                    e.Reply(200, id.ToString());                                // create reply
                }


                //if (e.Method == "POST")
                //{
                //    using (NpgsqlCommand cmd = connection.CreateCommand())
                //    {
                //        cmd.CommandText = "INSERT INTO messages (data) VALUES (@data)";
                //        cmd.Parameters.AddWithValue("@data", e.Payload);
                //        cmd.ExecuteNonQuery();
                //    }

                //    using (NpgsqlCommand cmd = connection.CreateCommand())
                //    {
                //        cmd.CommandText = "SELECT currval(pg_get_serial_sequence('messages','id'))";
                //        int id = Convert.ToInt32(cmd.ExecuteScalar());
                //        Console.WriteLine("Saved message: \"{0}\" as {1}", e.Payload.Replace("\n", "").Replace("\r", ""), id);
                //        e.Reply(200, id.ToString());
                //    }
                //}





                //If the path is "/messages" and the method is "GET", the code will get all the messages from the 
                //database and send a response with a status code of 200 and the messages as the content.
                else if (e.Method == "GET")
            {
                Console.WriteLine("Showed all messages.");
                //e.Reply(200, _ReadMessages());
            }
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
