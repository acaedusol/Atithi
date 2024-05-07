using System.Globalization;

namespace Atithi.Web.Services
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str; // Return original string if null or empty
            }

            // Create a TextInfo object for the current culture
            var textInfo = CultureInfo.CurrentCulture.TextInfo;

            // Convert the string to Title Case
            return textInfo.ToTitleCase(str.ToLower());
        }
    }
}
