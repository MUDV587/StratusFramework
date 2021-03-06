using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Stratus
{
	public static partial class Extensions
	{
		/// <summary>
		/// Counts the number of lines in this string
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static int ToLines(this string str)
		{
			return str.Split('\n').Length;
		}

		/// <summary>
		/// Strips all newlines in the string, replacing them with spaces
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string TrimNewlines(this string str)
		{
			return str.Replace('\n', ' ');
		}

		/// <summary>
		/// Formats this string, applying rich text formatting to it
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string Style(this string str, Color color, TextStyle style)
		{
			StringBuilder builder = new StringBuilder();

			// Italic
			if ((style & TextStyle.Italic) == TextStyle.Italic)
			{
				builder.Append("<i>");
			}

			// Bold
			if ((style & TextStyle.Bold) == TextStyle.Bold)
			{
				builder.Append("<b>");
			}

			// Color
			builder.Append("<color=#" + color.ToHex() + ">");
			builder.Append(str);
			builder.Append("</color>");

			// Bold
			if ((style & TextStyle.Bold) == TextStyle.Bold)
			{
				builder.Append("</b>");
			}

			// Italic
			if ((style & TextStyle.Italic) == TextStyle.Italic)
			{
				builder.Append("</i>");
			}

			return builder.ToString();
		}

		/// <summary>
		/// Converts a string from CamelCase to a human readable format. 
		/// Inserts spaces between upper and lower case letters. 
		/// Also strips the leading "_" character, if it exists.
		/// </summary>
		/// <param name="str"></param>
		/// <returns>A human readable string.</returns>
		public static string FromCamelCase(this string str)
		{
			return Regex.Replace(str, "(\\B[A-Z0-9])", " $1");
		}

		/// <summary>
		/// Converts a string to title case. ("HelloThere")
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ToTitleCase(this string str)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
		}
	}


}