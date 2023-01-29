using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MTCG
{
    public class StringHandler
    {
        // This method recieve a string like "WaterGoblin" and divide it to two strings "Water" and "Goblin"
        public string[] DivideString(string input)
        {
            Regex regex = new Regex(@"([A-Z][a-z]*)([A-Z][a-z]*)");
            Match match = regex.Match(input);
            if (match.Success)
            {
                string firstPart = match.Groups[1].Value;
                string secondPart = match.Groups[2].Value;

                return new string[] { firstPart, secondPart };
            }
            else
            {
                return new string[] { input };
            }
        }

        // This method recieve a string like "Basic admin-mtcgToken" and remove the "Basic " and return "admin-mtcgToken"
        public string TokenDistinguisher(string input)
        {
            string token = input.Replace("Basic ", "");
            return (token);
        }

        // This method recieve a string like "Basic admin-mtcgToken" and remove the "Basic " and "_mtcgToken" and return "admin"
        public string UserDistinguisher (string input)
        {
            string variablePart = input.Replace("Basic ", "").Replace("-mtcgToken", "");
            return (variablePart);
        }
    }
}
