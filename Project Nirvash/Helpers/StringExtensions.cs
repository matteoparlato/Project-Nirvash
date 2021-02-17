using System.Linq;

namespace Project_Nirvash.Helpers
{
    /// <summary>
    /// StringExtensions class
    /// </summary>
    internal static class StringExtensions
    {
        public static string RemoveAllButNumbers(this string value)
        {
            return new string(value.Where(c => "0123456789".Contains(c)).ToArray());
        }

        public static string NullValueToEmpty(this string value)
        {
            return value ?? string.Empty;
        }
    }
}
