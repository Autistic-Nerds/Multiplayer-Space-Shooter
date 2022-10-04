
using CosmosEngine.CoreModule;
using Microsoft.Xna.Framework;
using System;

namespace CosmosEngine.Modules
{
	public sealed class GizmosModule : ObserverManager<Behaviour, GizmosModule>, IRenderModule
	{
		private const string GizmosMethod = "OnDrawGizmos";
		public override void Initialize()
		{
			base.Initialize();
			ObjectDelegater.CreateNewDelegation<Behaviour>(Subscribe);
		}

		protected override void Add(Behaviour item)
		{
			if (!item.GetType().MethodExistsOnObject(GizmosMethod))
				return;

			base.Add(item);
		}

		public override void BeginEventCall() {	}
		public void RenderWorld()
		{
			foreach (Behaviour behaviour in observerList)
			{
				if (behaviour.Expired)
				{
					observerList.IsDirty = true;
					continue;
				}
				if (behaviour is IUIComponent)
					continue;

				if (behaviour.Enabled)
				{
					Gizmos.Colour = Colour.White;
					Gizmos.Matrix = Matrix.Identity;
					behaviour.Invoke(GizmosMethod);
				}
			}
		}
		public void RenderUI()
		{
			foreach (Behaviour behaviour in observerList)
			{
				if (behaviour.Expired)
				{
					observerList.IsDirty = true;
					continue;
				}
				if (!(behaviour is IUIComponent))
					continue;

				if (behaviour.Enabled)
				{
					Gizmos.Colour = Colour.White;
					Gizmos.Matrix = Matrix.Identity;
					behaviour.Invoke(GizmosMethod);
				}
			}
		}

		public override Predicate<Behaviour> RemoveAllPredicate() => item => item.Expired;
	}
}