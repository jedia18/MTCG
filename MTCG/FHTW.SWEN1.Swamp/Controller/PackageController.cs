using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MTCG.Controller
{
    internal class PackageController
    {
        public static void AddPackage(object evt, NpgsqlConnection _Cn)
        {
            HttpSvrEventArgs e = (HttpSvrEventArgs)evt;

            if (e.Method == "POST")
            {
                NpgsqlCommand cmd = _Cn.CreateCommand();

                // create database command, create table if it doesn't exist 
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS packages (card_id SERIAL PRIMARY KEY, id VARCHAR(255), name VARCHAR(255), damage FLOAT)";
                cmd.ExecuteNonQuery();

                //Console.WriteLine(e.Payload);

                JArray jArray = JArray.Parse(e.Payload);

                //foreach (JObject jObject in jArray)
                //{
                //    Console.WriteLine((string)jObject["Id"]);
                //    Console.WriteLine((string)jObject["Name"]);
                //}


                //int k;
                //for (k = 0; k < jArray.Count; k++)
                //{
                //    var jObject = (JObject)jArray[k];
                //    Console.WriteLine((string)jObject["Id"]);
                //    Console.WriteLine((string)jObject["Name"]);
                //}

                int i = 0;
                //for (i = 0; i < jArray.Count; i++)
                foreach (JObject jObject in jArray)
                {
                    //var jObject = (JObject)jArray[i];
                    var id = (string)jObject["Id"];
                    var name = (string)jObject["Name"];
                    var damage = (float)jObject["Damage"];

                    cmd.CommandText = "INSERT INTO packages (id, name, damage) VALUES (@id, @name, @damage)";

                    IDataParameter p1 = cmd.CreateParameter();
                    p1.ParameterName = ":id";
                    p1.Value = id;
                    cmd.Parameters.Add(p1);
                    //Console.WriteLine("id for index " + i + " is : " + cmd.Parameters[i].Value);

                    IDataParameter p2 = cmd.CreateParameter();
                    p2.ParameterName = ":name";
                    p2.Value = name;
                    cmd.Parameters.Add(p2);

                    IDataParameter p3 = cmd.CreateParameter();
                    p3.ParameterName = ":damage";
                    p3.Value = damage;
                    cmd.Parameters.Add(p3);
                    //cmd.ExecuteNonQuery();
                    //Console.WriteLine(p1.Value);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Card with Id \"" + id + "\" is inserted in table!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Card with this ID \"" + id + "\" is already exist!");
                        Console.ForegroundColor = ConsoleColor.White;

                        if (ex.ErrorCode.Equals(23505))
                        {
                            Console.WriteLine("Error: The username already exists.");
                            e.Reply(400, "Error: The username already exists.");
                        }
                    }
                    //i += 3;
                }

                e.Reply(200, "Packages inserted successfully");
                cmd.Dispose();
            }
        }

    }
}
