using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.Controller
{
    public class CardAcquireController
    {
        //If the path is "/transactions/packages" and the method is "POST", the code will add the username of the user to the packages table in Db
        //to the first five card  that has no user and decrease five coins from the user in users table in DB.
        public static void CardAcquire(object evt, NpgsqlConnection _Cn, string username)
        {
            HttpSvrEventArgs e = (HttpSvrEventArgs)evt;
            // Add the username to the first five rows of the username column in the packages table

            // check before the user can acquire any packages from the packages table to see if any card with a null username exists
            using (var cmd = new NpgsqlCommand("SELECT count(*) FROM packages WHERE username IS NULL", _Cn))
            {
                long count = (long)cmd.ExecuteScalar();
                if (count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is no more packages!");
                    Console.ForegroundColor = ConsoleColor.White;
                    e.Reply(400, "There is no more packages!");
                    return; // exits the method
                }
            }

            // check if the user have enough coins
            using (var cmd = new NpgsqlCommand("SELECT coins FROM users WHERE username = @username", _Cn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                int coins = (int)cmd.ExecuteScalar();
                if (coins < 5)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("user has not enough coins");
                    Console.ForegroundColor = ConsoleColor.White;
                    e.Reply(400, "user has not enough coins");
                    return; // exits the method
                }
            }

            using (var cmd = new NpgsqlCommand("UPDATE packages SET username = @username FROM (SELECT * FROM packages WHERE username IS NULL LIMIT 5) as subquery WHERE subquery.id = packages.id;", _Cn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                try
                {
                    cmd.ExecuteNonQuery();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("One package successfully for " + username + " aquired!");
                    Console.ForegroundColor = ConsoleColor.White;
                    e.Reply(200, "Package successfully aquired");
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Package is already aquired!");
                    Console.ForegroundColor = ConsoleColor.White;
                    e.Reply(400, "Error: The username already exists.");
                }
            }

            // Subtract 5 units from the amount of coins related to this username in the users table
            using (var cmd = new NpgsqlCommand("UPDATE users SET coins = coins - 5 WHERE username = @username", _Cn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                try
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Five coins subtracted from " + username + "'s coins.");
                    e.Reply(200, "Five coins subtracted from user's coins.");
                }
                catch (Exception)
                {

                }
            }
        }
    }
}

