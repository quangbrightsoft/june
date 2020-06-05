namespace Brightsoft.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

        public static string ToPascalCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
    }
}
