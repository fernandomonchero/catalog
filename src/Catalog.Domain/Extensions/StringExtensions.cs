using System.Text.RegularExpressions;

namespace Catalog.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string? NormalizeSpaces(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value?.Trim();

            return Regex.Replace(value.Trim(), @"\s+", " ");
        }
    }
}