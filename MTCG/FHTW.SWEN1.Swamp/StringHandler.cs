using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MTCG
{
    internal class StringHandler
    {
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

    }

}
