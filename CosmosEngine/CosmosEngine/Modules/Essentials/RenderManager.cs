//Written by Philip Wittusen
using CosmosEngine.CoreModule;
using CosmosEngine.Collection;
using CosmosEngine.Rendering;

namespace CosmosEngine.Modules
{
	public sealed class RenderManager : ObserverManager<IRenderer, RenderManager>, IRenderModule
	{
		private readonly DirtyList<IRenderWorld> renderComponents = new DirtyList<IRenderWorld>();
		private readonly DirtyList<IRenderUI> uiComponents = new DirtyList<IRenderUI>();

		public override void Initialize()
		{
			base.Initialize();
			ObjectDelegater.CreateNewDelegation<IRenderer>(Subscribe);
		}

		protected override void Add(IRenderer item)
		{
			if(item is IRenderWorld)
			{
				renderComponents.Add((IRenderWorld)item);
			}
			if(item is IRenderUI)
			{
				uiComponents.Add((IRenderUI)item);
			}
		}

		public override void BeginEventCall() {	}

		public void RenderWorld()
		{
			Draw.Space = WorldSpace.World;
			foreach (IRenderWorld obj in renderComponents)
			{
				if (obj.Expired)
				{
					renderComponents.IsDirty = true;
					continue;
				}

				if (obj.Enabled)
				{
					obj.Render();
				}
			}
		}

		public void RenderUI()
		{
			Draw.Space = WorldSpace.Screen;
			foreach (IRenderUI obj in uiComponents)
			{
				if (obj.Expired)
				{
					uiComponents.IsDirty = true;
					continue;
				}

				if (obj.Enabled)
				{
					obj.UI();
				}
			}
		}
		public override void Update()
		{
			base.Update();
			if (renderComponents.IsDirty)
			{
				renderComponents.RemoveAll(RemoveAllPredicate());
				renderComponents.IsDirty = false;
			}
			if (uiComponents.IsDirty)
			{
				uiComponents.RemoveAll(RemoveAllPredicate());
				uiComponents.IsDirty = false;
			}
		}
		public override System.Predicate<IRenderer> RemoveAllPredicate() => item => item.Expired;
	}
}