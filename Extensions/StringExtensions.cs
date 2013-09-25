namespace DotNetNuke.Wiki.Extensions
{
    /// <summary>
    /// Provides extension methods to strings
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// truncates a string to the passed length
        /// </summary>
        /// <param name="strValue">the string to truncate</param>
        /// <param name="lenght">the maximum length</param>
        /// <returns>returns the string truncated to the specified value</returns>
        public static string TruncateString(this string strValue, int lenght)
        {
            if ((!string.IsNullOrWhiteSpace(strValue) && strValue.Length > lenght))
            {
                return strValue.Substring(0, lenght);
            }

            else
            {
                return strValue;
            }
        }
    }
}