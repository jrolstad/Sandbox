using System;
using System.Linq;

namespace MyApp
{
    public class StringService
    {
        public string Reverse(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var reversedArray = input
                .ToCharArray()
                .Reverse()
                .ToArray();

            return new string(reversedArray);
        }
    }
}
