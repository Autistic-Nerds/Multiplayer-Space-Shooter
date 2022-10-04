using System;
using System.ComponentModel;
using System.Globalization;

namespace CosmosEngine.Converter
{
	internal class Vector2Converter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

		public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
		{
			if (value is string s)
			{
				string[] values = s.Split(':');
				string x = values[0];
				string y = values[1];
				return new Vector2(float.Parse(x), float.Parse(y));
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				if (value is Vector2)
				{
					Vector2 v = (Vector2)value;
					return $"{v.X}:{v.Y}";
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}