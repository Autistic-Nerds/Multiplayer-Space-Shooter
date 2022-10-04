
using System.Reflection;
using System.Text;

namespace CosmosEngine
{
	public static class ReflectionExtension
	{
		public static string ParameterName(this MethodInfo method)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(method.Name);
			sb.Append(method.GetParameters().ParameterName());
			return sb.ToString();
		}

		public static string ParameterName(this MethodBase method)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(method.Name);
			sb.Append(method.GetParameters().ParameterName());
			return sb.ToString();
		}

		public static string ParameterName(this ParameterInfo[] parameterInfo)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("(");
			for (int i = 0; i < parameterInfo.Length; i++)
			{
				sb.Append(parameterInfo[i].ParameterType.Name);
				if (i != parameterInfo.Length - 1)
					sb.Append(", ");
			}
			sb.Append(")");
			return sb.ToString();
		}

		public static string ParametersTypeToString(this object[] parameters)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("(");
			for (int i = 0; i < parameters.Length; i++)
			{
				sb.Append(parameters[i].GetType().Name);
				if (i != parameters.Length - 1)
					sb.Append(", ");
			}
			sb.Append(")");
			return sb.ToString();
		}
	}
}