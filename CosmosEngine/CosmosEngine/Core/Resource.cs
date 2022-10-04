
using System;

namespace CosmosEngine.CoreModule
{
	public abstract class Resource : IDisposable
	{
		private bool disposed;
		public bool IsDisposed => disposed;
		~Resource() => this.Dispose(false);
		/// <summary>
		/// <inheritdoc cref="System.IDisposable.Dispose"/> 
		/// </summary>
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