
using System;

namespace CosmosEngine.Modules
{
	public abstract class BaseModule : IModule, IDisposable
	{
		private bool disposed;
		private int executionOrder;

		~BaseModule() => this.Dispose(false);
		public bool IsDisposed => disposed;
		/// <summary>
		/// The execution order determines in which order the modules execute their functions. Modules with a higher execution order will execute their function first, while modules with lower execution order execute their function last. Execution order needs to be set before the application is launched.
		/// </summary>
		public int ExecutionOrder { get => executionOrder; set => executionOrder = value; }
		public abstract void Initialize();
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize((object)this);
		}

		/// <summary>
		/// This method should derived classes override to implement disposing of managed resources. Immediately releases the unmanaged resources used by this object.
		/// </summary>
		/// <param name="disposing">True if objects should be disposed.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed)
				return;
			if (disposing)
			{

			}
			this.disposed = true;
		}
	}
}