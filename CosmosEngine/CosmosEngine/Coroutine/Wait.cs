using CosmosEngine.Async;
using CosmosEngine.Factory;
using CosmosEngine.ObjectPooling;
using System;

namespace CosmosEngine
{
	public static class Wait
	{
		internal class WaitForSecondsPool : YieldInstructionPool<WaitForSeconds>
		{

		}

		internal class WaitForSecondsUnscaledPool : YieldInstructionPool<WaitForSecondsUnscaled>
		{

		}
		
		internal class WaitWhilePool : YieldInstructionPool<WaitWhile>
		{

		}

		internal class WaitUntilPool : YieldInstructionPool<WaitUntil>
		{

		}

		public static WaitForSeconds ForSeconds(float seconds)
		{
			WaitForSeconds t = ScriptableObject.Instance<WaitForSecondsPool>().Request();
			t.seconds = seconds;
			return t;
		}

		public static WaitForSecondsUnscaled ForSecondsUnscaled(float seconds)
		{
			WaitForSecondsUnscaled t = ScriptableObject.Instance<WaitForSecondsUnscaledPool>().Request();
			t.seconds = seconds;
			return t;
		}

		public static WaitWhile While(Func<bool> predicate)
		{
			WaitWhile t = ScriptableObject.Instance<WaitWhilePool>().Request();
			t.predicate = predicate;
			return t;
		}

		public static WaitUntil Until(Func<bool> predicate)
		{
			WaitUntil t = ScriptableObject.Instance<WaitUntilPool>().Request();
			t.predicate = predicate;
			return t;
		}
	}
}