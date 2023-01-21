using MTCG.Models;
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
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS packages (card_id SERIAL PRIMARY KEY, id VARCHAR(255), name VARCHAR(255), damage FLOAT, element VARCHAR(255), type VARCHAR(255))";
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

                //int i = 0;
                //for (int i = 0; i < jArray.Count; i++)
                foreach (JObject jObject in jArray)
                {
                    //JObject jObject = JObject.Parse(jArray);
                    //var jObject = (JObject)jArray[i];
                    //var id = (string)jObject["Id"];
                    //var name = (string)jObject["Name"];
                    var damage = (float)jObject["Damage"];

                    cmd.CommandText = "INSERT INTO packages (id, name, damage, element, type) VALUES (@id, @name, @damage, @element, @type)";

                    var card = new Card();                                    // Make an object from the class Card
                    var cardId = card.Id;                                     // Put the Id of this object to variable cardId
                    cardId = (string)jObject["Id"];                           // Put the Id from the e.payload into this variable
                    var cardName = card.Name;
                    cardName = (string)jObject["Name"];
                    var cardDamage = card.Damage;
                    cardDamage = (float)jObject["Damage"];
                    var cardElement = card.Element;
                    var cardType = card.Type;

                    StringHandler seperate = new StringHandler();             // Seperate cardName to two parts: cardElement and carType
                    string[] parts = seperate.DivideString(cardName);

                    if (parts != null && parts.Length >= 1)
                    {
                        if (parts[0] == "Water" || parts[0] == "Fire" || parts[0] == "Regular")   // WaterGoblin -> Water    Goblin
                        {
                            cardElement = parts[0];
                            cardType = parts[1];
                        }
                        else                                                                      // Dragon ->      -----    Dragon
                        {
                            cardElement = "";
                            cardType = parts[0];
                        }
                    }

                    IDataParameter p1 = cmd.CreateParameter();
                    p1.ParameterName = ":id";
                    p1.Value = cardId;                                        // Put value of this variable in the p1.Value to send for Db
                    cmd.Parameters.Add(p1);
                    //Console.WriteLine("id for index " + i + " is : " + cmd.Parameters[i].Value);

                    IDataParameter p2 = cmd.CreateParameter();
                    p2.ParameterName = ":name";
                    p2.Value = cardName;
                    cmd.Parameters.Add(p2);

                    IDataParameter p3 = cmd.CreateParameter();
                    p3.ParameterName = ":damage";
                    p3.Value = cardDamage;
                    cmd.Parameters.Add(p3);

                    
                    IDataParameter p4 = cmd.CreateParameter();
                    p4.ParameterName = ":element";
                    p4.Value = cardElement;
                    cmd.Parameters.Add(p4);

                    IDataParameter p5 = cmd.CreateParameter();
                    p5.ParameterName = ":type";
                    p5.Value = cardType;
                    cmd.Parameters.Add(p5);
                    //cmd.ExecuteNonQuery();
                    //Console.WriteLine(p1.Value);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Card with Id \"" + cardId + "\" is inserted in table!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Card with this ID \"" + cardId + "\" is already exist!");
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
