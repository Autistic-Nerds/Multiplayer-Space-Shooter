
using System.Text;

namespace CosmosEngine
{
	public static class StringExtension
	{
		public static string ToDisplayString(this string s)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < s.Length; i++)
			{
				if (i == 0)
				{
					sb.Append(char.ToUpper(s[i]));
					continue;
				}
				else if (char.IsUpper(s[i]) && !char.IsUpper(s[i - 1]))
					sb.Append(" ");
				else if(char.Equals(s[i], '_'))
				{
					sb.Append(" ");
					continue;
				}

				sb.Append(s[i]);

			}
			return sb.ToString();
		}

		public static Vector2 MeasureString(this string s, Font font) => font.MeasureString(s);
		public static Vector2 MeasureString(this string s, Font font, int fontSize) => font.MeasureString(s, fontSize);
	}
}