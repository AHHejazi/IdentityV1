// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Extensions
{
    #region

    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    #endregion

    /// <summary>
    ///     The string extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     The newline.
        /// </summary>
        private const string Newline = "\r\n";

        /// <summary>
        ///     The random.
        /// </summary>
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Creates a new random string of upper, lower case letters and digits.
        ///     Very useful for generating random data for storage in test data.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="size">
        /// The number of characters of the string to generate
        /// </param>
        /// <param name="includeNumbers">
        /// if set to <c>true</c> [include numbers].
        /// </param>
        /// <returns>
        /// randomized string
        /// </returns>
        public static string AppendRandomString(this string text, int length, bool includeNumbers = false)
        {
            string chars = includeNumbers ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" : "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return text + new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        
        public static string AppendRandomNumbers(this string text, int length)
        {
            string chars = "0123456789";
            return text + new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Appends random chars and numeric.
        /// Added logic to specify the format of the random string (# will be random string, 0 will be random numeric, other characters remain)
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string AppendRandomFormattedString(this string text, string format)
        {
            const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string Numbers = "0123456789";

            var result = new StringBuilder(text);
            for (int formatIndex = 0; formatIndex < format.Length; formatIndex++)
            {
                switch (format.ToUpper()[formatIndex])
                {
                    case '0':
                        result.Append(Numbers[Random.Next(Numbers.Length)]);
                        break;
                    case '#':
                        result.Append(Chars[Random.Next(Chars.Length)]);
                        break;
                    default: result.Append(format[formatIndex]);
                        break;
                }
            }
            return result.ToString();
        }

        public static string StringFormat(this string text, params object[] args)
        {
            return string.Format(text, args);
        }

        /// <summary>
        /// The as unicode string.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string AsUnicodeString(this string s)
        {
            var stringBuilder = new StringBuilder();
            foreach (var t in s)
            {
                stringBuilder.Append($"\\u{Convert.ToString(t, 16).PadLeft(4, '0')}");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Locates position to break the given line so as to avoid
        ///     breaking words.
        /// </summary>
        /// <param name="text">
        /// String that contains line of text
        /// </param>
        /// <param name="pos">
        /// Index where line of text starts
        /// </param>
        /// <param name="max">
        /// Maximum line length
        /// </param>
        /// <returns>
        /// The modified line length
        /// </returns>
        public static int BreakLine(this string text, int pos, int max)
        {
            // Find last whitespace in line
            var i = max - 1;

            while (i >= 0 && !char.IsWhiteSpace(text[pos + i]))
            {
                i--;
            }

            if (i < 0)
            {
                return max; // No whitespace found; break at maximum length
            }

            // Find start of whitespace
            while (i >= 0 && char.IsWhiteSpace(text[pos + i]))
            {
                i--;
            }

            // Return length of text before whitespace
            return i + 1;
        }

        /// <summary>
        /// Returns part of a string up to the specified number of characters, while maintaining full words
        /// </summary>
        /// <param name="s">
        /// </param>
        /// <param name="length">
        /// Maximum characters to be returned
        /// </param>
        /// <returns>
        /// String
        /// </returns>
        public static string Chop(this string s, int length)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            var words = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();

            foreach (var word in words.Where(word => sb.ToString().Length + word.Length <= length))
            {
                sb.Append(word + " ");
            }

            return sb.ToString().TrimEnd(' ') + "...";
        }

        /// <summary>
        /// Returns a line count for a string
        /// </summary>
        /// <param name="s">
        /// string to count lines for
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int CountLines(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }

            return s.Split('\n').Length;
        }

        /// <summary>
        /// The format url.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FormatUrl(this string url)
        {
            url = url.Replace("http://", "http:").Replace("https://", "https:");
            var pattern = @"//";
            var rgx = new Regex(pattern);
            var output = rgx.Replace(url.Trim(), "/");
            output = output.Replace("http:", "http://").Replace("https:", "https://");

            if (output.Length > 0 && output.Last() != '/')
            {
                output += '/';
            }

            // int lastSlash = output.LastIndexOf('/');
            // output = (lastSlash > -1) ? output.Substring(0, lastSlash) : output;
            return output;
        }

        /// <summary>
        /// Parses a string into an array of lines broken
        ///     by \r\n or \n
        /// </summary>
        /// <param name="s">
        /// String to check for lines
        /// </param>
        /// <returns>
        /// array of strings, or null if the string passed was a null
        /// </returns>
        public static string[] GetLines(this string s)
        {
            if (s == null)
            {
                return null;
            }

            s = s.Replace("\r\n", "\n");
            return s.Split('\n');
        }

        /// <summary>
        /// The remove from end.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="suffix">
        /// The suffix.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveFromEnd(this string s, string suffix)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(suffix))
            {
                return s;
            }

            if (s.IndexOf(suffix, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                return s;
            }

            return s.Substring(0, s.Length - suffix.Length);
        }

        /// <summary>
        /// The remove starting with.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="staringWith">
        /// The staring with.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveStartingWith(this string s, string staringWith)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(staringWith))
            {
                return s;
            }

            var index = s.IndexOf(staringWith, StringComparison.InvariantCultureIgnoreCase);

            if (index < 0)
            {
                return s;
            }

            return s.Substring(0, index);
        }

        /// <summary>
        /// The replace arabic by english numbers.
        /// </summary>
        /// <param name="str">
        /// The str.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ReplaceArabicByEnglishNumbers(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var englishNumber = str;

            try
            {
                foreach (var arn in str)
                {
                    switch (arn)
                    {
                        case '٠':
                            englishNumber = englishNumber.Replace('٠', '0');
                            break;
                        case '١':
                            englishNumber = englishNumber.Replace('١', '1');
                            break;
                        case '٢':
                            englishNumber = englishNumber.Replace('٢', '2');
                            break;
                        case '٣':
                            englishNumber = englishNumber.Replace('٣', '3');
                            break;
                        case '٤':
                            englishNumber = englishNumber.Replace('٤', '4');
                            break;
                        case '٥':
                            englishNumber = englishNumber.Replace('٥', '5');
                            break;
                        case '٦':
                            englishNumber = englishNumber.Replace('٦', '6');
                            break;
                        case '٧':
                            englishNumber = englishNumber.Replace('٧', '7');
                            break;
                        case '٨':
                            englishNumber = englishNumber.Replace('٨', '8');
                            break;
                        case '٩':
                            englishNumber = englishNumber.Replace('٩', '9');
                            break;
                    }
                }

                return englishNumber;
            }
            catch
            {
                return str;
            }
        }

        /// <summary>
        /// The strip html.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string StripHtml(this string input)
        {
            return input == null ? null : Regex.Replace(input, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Converts a string into bytes for storage in any byte[] types
        ///     buffer or stream format (like MemoryStream).
        /// </summary>
        /// <param name="text">
        /// </param>
        /// <param name="encoding">
        /// The character encoding to use. Defaults to Unicode
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] ToByteArray(this string text, Encoding encoding = null)
        {
            if (text == null)
            {
                return null;
            }

            if (encoding == null)
            {
                encoding = Encoding.Unicode;
            }

            return encoding.GetBytes(text);
        }

        /// <summary>
        /// Convert the string to camel case.
        /// </summary>
        /// <param name="str">
        /// the string to turn into Camel case
        /// </param>
        /// <returns>
        /// a string formatted as Camel case
        /// </returns>
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (!char.IsUpper(str[0]))
            {
                return str;
            }

            var camelCase = char.ToLower(str[0]).ToString();
            if (str.Length > 1)
            {
                camelCase += str.Substring(1);
            }

            return camelCase;
        }

        /// <summary>
        /// The to enum.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Convert the string to Pascal case.
        /// </summary>
        /// <param name="theString">
        /// the string to turn into Pascal case
        /// </param>
        /// <returns>
        /// a string formatted as Pascal case
        /// </returns>
        public static string ToPascalCase(this string theString)
        {
            // If there are 0 or 1 characters, just return the string.
            if (theString == null)
            {
                return null;
            }

            if (theString.Length < 2)
            {
                return theString.ToUpper();
            }

            // Split the string into words.
            var words = theString.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            return words.Aggregate(
                string.Empty,
                (current, word) => current + word.Substring(0, 1).ToUpper() + word.Substring(1));
        }

        /// <summary>
        /// Capitalize the first character and add a space before
        ///     each capitalized letter (except the first character).
        /// </summary>
        /// <param name="theString">
        /// the string to turn into Proper case
        /// </param>
        /// <returns>
        /// a string formatted as Proper case
        /// </returns>
        public static string ToProperCase(this string theString)
        {
            // If there are 0 or 1 characters, just return the string.
            if (theString == null)
            {
                return null;
            }

            if (theString.Length < 2)
            {
                return theString.ToUpper();
            }

            // Start with the first character.
            var result = theString.Substring(0, 1).ToUpper();

            // Add the remaining characters.
            for (var i = 1; i < theString.Length; i++)
            {
                if (char.IsUpper(theString[i]))
                {
                    result += " ";
                }

                result += theString[i];
            }

            return result;
        }

        /// <summary>
        /// Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">
        /// Text to be word wrapped
        /// </param>
        /// <param name="width">
        /// Width, in characters, to which the text
        ///     should be word wrapped
        /// </param>
        /// <returns>
        /// The modified text
        /// </returns>
        /// <see cref="http://www.softcircuits.com/Blog/post/2010/01/10/Implementing-Word-Wrap-in-C.aspx"/>
        public static string WordWrap(this string text, int width)
        {
            int pos, next;
            var sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
            {
                return text;
            }

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                var eol = text.IndexOf(Newline, pos, StringComparison.Ordinal);

                if (eol == -1)
                {
                    next = eol = text.Length;
                }
                else
                {
                    next = eol + Newline.Length;
                }

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        var len = eol - pos;

                        if (len > width)
                        {
                            len = BreakLine(text, pos, width);
                        }

                        sb.Append(text, pos, len);
                        sb.Append(Newline);

                        // Trim whitespace following break
                        pos += len;

                        while (pos < eol && char.IsWhiteSpace(text[pos]))
                        {
                            pos++;
                        }
                    }
                    while (eol > pos);
                }
                else
                {
                    sb.Append(Newline); // Empty line
                }
            }

            return sb.ToString();
        }
    }
}