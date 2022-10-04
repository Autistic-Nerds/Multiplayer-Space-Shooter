using CosmosEngine.Collection;
using CosmosEngine.CoreModule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CosmosEngine.Modules
{
	public sealed class CollisionManager : ObserverManager<IPhysicsComponent, CollisionManager>
	{
		private readonly DirtyList<Rigidbody> rigidbodies = new DirtyList<Rigidbody>();
		private readonly DirtyList<Collider> colliders = new DirtyList<Collider>();
		private System.Diagnostics.Stopwatch stopwatch;
		private static double elapsedTime;
		public static double Time => elapsedTime;
		private Thread physicsThread;
		private readonly object physicsLock = new object();

		private readonly List<CollisionHandle> handles = new List<CollisionHandle>();

		private struct CollisionHandle
		{
			public Collider origin;
			public Collider other;

			public CollisionHandle(Collider origin, Collider other)
			{
				this.origin = origin;
				this.other = other;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			ObjectDelegater.CreateNewDelegation<IPhysicsComponent>(Subscribe);

			stopwatch = System.Diagnostics.Stopwatch.StartNew();
			//elapsedTime = 0d;
			//physicsThread = new Thread(new ThreadStart(Main));
			//physicsThread.IsBackground = true;
			//physicsThread.Start();
		}

		protected override void Add(IPhysicsComponent item)
		{
			lock (physicsLock)
			{
				base.Add(item);
				if (item is Rigidbody rb)
				{
					rigidbodies.Add(rb);
				}
				if (item is Collider col)
				{
					colliders.Add(col);
				}
			}
		}

		public void Main()
		{
			while (Core.ApplicationIsRunning)
			{
				ColliderCollisionEvent();
				Thread.Sleep(2);
			}
		}

		public override void BeginEventCall()
		{
			Debug.LogTable($"Colliders: {colliders.Count}", colliders, LogFormat.Message);
			Debug.LogTable($"Rigidbodies: {rigidbodies.Count}", rigidbodies, LogFormat.Message);
			//RigidbodyCollision();
		}

		private void RigidbodyCollision()
		{
			stopwatch.Restart();
			//Debug.FastLog($"Rigidbodies to check ${rigidbodies.Count}");
			int rigidbodyCount = 0;
			int nestedObjects = 0;
			Stopwatch rootCheck = Stopwatch.StartNew();
			Stopwatch nestedCheck = Stopwatch.StartNew();
			double nest = 0d;
			for (int i = 0; i < rigidbodies.Count - 1; i++)
			{
				//Debug.FastLog(" ");
				Rigidbody a = rigidbodies[i];
				if (a.Expired)
				{
					rigidbodies.IsDirty = true;
					continue;
				}

				if (!a.Enabled)
					continue;
				rigidbodyCount++;

				//Debug.FastLog("Start at: " + a);

				nestedCheck.Restart();
				for (int j = i + 1; j < rigidbodies.Count; j++)
				{
					Rigidbody b = rigidbodies[j];
					if (b.Expired || !b.Enabled)
						continue;
					rigidbodyCount++;
					nestedObjects++;
					//Debug.FastLog("Other: " + b);

					//Check collisions between Rigidbody A's Colliders and Rigidbody B's Colliders
					//If just two of the colliders intersects with eachother we break out of the loop and mark the collision.
					bool collisionCheck = false;

					//if (a.Col == null || b.Col == null)
					//	continue;
					//if (a.Col.CheckCollision(b.Col))
					//	break;

					Action work = delegate
					{
						foreach (Collider cA in a.Colliders)
						{
							if (cA.Expired)
							{
								colliders.IsDirty = true;
								continue;
							}
							if (!cA.Enabled)
								continue;

							foreach (Collider cB in b.Colliders)
							{
								if (cB.Expired)
								{
									colliders.IsDirty = true;
									continue;
								}
								if (!cB.Enabled)
									continue;

								if (cA.CheckCollision(cB))
								{
									collisionCheck = true;
									handles.Add(new CollisionHandle(cA, cB));
								}
								//Debug.FastLog($"Check: {cA} - {cB} == {collisionCheck}");
								if (collisionCheck)
								{
									return;
								}
							}
						}
					};
					work.Invoke();
				}
				nest += nestedCheck.Elapsed.TotalMilliseconds;
			}
			//Debug.FastLog($"Nested Check took on avg: {(nest):F2}ms");
			//Debug.FastLog($"Check through a total of ${rigidbodyCount} rigidbodies");
			elapsedTime = stopwatch.Elapsed.TotalMilliseconds;

			//Debug.FastLog("Calculation took: " + Collider.elapsedTime.ToString("F2"));
			Collider.elapsedTime = 0;
		}

		private void ColliderCollision()
		{
			stopwatch.Restart();
			for(int a = 0; a < colliders.Count; a++)
			{
				Collider cA = colliders[a];
				if (cA.Expired)
				{
					colliders.IsDirty = true;
					continue;
				}

				if (!cA.Enabled)
					continue;

				for (int b = 1; b < colliders.Count; b++)
				{
					Collider cB = colliders[b];

					if (cB.Expired || !cB.Enabled)
						continue;

					if (cA.CheckCollision(cB))
					{
						continue;
					}
				}
			}
			elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
			Collider.elapsedTime = 0;
		}

		private void ColliderCollisionEvent()
		{
		}

		public override void Update()
		{
			base.Update();
			if(rigidbodies.IsDirty)
			{
				rigidbodies.RemoveAll(item => item.Expired);
				rigidbodies.IsDirty = false;
			}
		}

		public override System.Predicate<IPhysicsComponent> RemoveAllPredicate() => item => item.Expired;
	}
}