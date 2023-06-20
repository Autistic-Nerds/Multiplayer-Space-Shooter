using System;

namespace CosmosEngine.Async
{
	public class WaitWhile : YieldInstruction
	{
		internal Func<bool> predicate;
		public override bool KeepWaiting => predicate();

		public WaitWhile() => predicate = () => { return false; };
		public WaitWhile(Func<bool> predicate) => this.predicate = predicate;

		public override void Complete() => ScriptableObject.Instance<Wait.WaitWhilePool>().Return(this);
	}
}