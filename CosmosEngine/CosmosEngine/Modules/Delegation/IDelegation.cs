
using System;

namespace CosmosEngine.Modules
{
	public interface IDelegation
	{
		Type Type { get; }

		void Invoke(object obj);
		bool Match(object obj);
	}
}