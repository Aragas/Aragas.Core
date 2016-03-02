using System.Linq;

namespace Aragas.Core.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveWhitespace(this string input) => new string(input.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());
    }
}