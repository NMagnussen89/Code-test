using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    public static class Extensions
    {
        public static string RemoveWhitespaceAndSpecialCharacters(this string input)
        {
            input = input.Replace(" ", string.Empty);
            return Regex.Replace(input, "[^a-zA-Z0-9_.]+", string.Empty);
        }
    }
}
