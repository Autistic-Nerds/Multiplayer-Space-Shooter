using CosmosEngine;
using CosmosEngine.NetCode.Serialization;
using System.Collections.Generic;

namespace CosmosEngine.NetCode
{
	public class NetCodeIdentity : GameBehaviour
	{
		private readonly Dictionary<uint, NetCodeBehaviour> netCodeBehaviours = new Dictionary<uint, NetCodeBehaviour>();

		private uint netId;
		private uint netCodeId;

		private bool isServer;
		private bool isClient;
		private bool isLocal;
		private bool hasAuthority;

		/// <summary>
		/// Returns <see langword="true"/> if this object was instantiated and is controlled by the server.
		/// </summary>
		public bool IsServer { get => isServer; internal set => isServer = value; }
		/// <summary>
		/// Returns <see langword="true"/> if this object was instantiated and is controlled by a client.
		/// </summary>
		public bool IsClient { get => isClient; internal set => isClient = value; }
		/// <summary>
		/// Returns <see langword="true"/> if this object was instantiated and is controlled by the local connection. (I.e this client) in a network session.
		/// </summary>
		public bool IsLocal { get => isLocal; internal set => isLocal = value;  }
		/// <summary>
		/// Returns <see langword="true"/> if the local client has the authority on this object.
		/// <para>Authority determines who syncs the data from this object to the server. For most objects this should be held by the server.</para> 
		/// <para>Authority can be transferred using <see cref="TransferAuthority"/>. Data can still be written from the local client to the network session using other means.</para>
		/// </summary>
		public bool HasAuthority { get => hasAuthority; internal set => hasAuthority = value; }

		/// <summary>
		/// The unique network Id of this object.
		/// <para>This is assigned at runtime by the network server and will be unique for all objects for that network session.</para>
		/// </summary>
		public uint NetId { get => netId; internal set => netId = value; }

		protected override void Awake()
		{
			Init();
		}

		private void Init()
		{
			UpdateBehaviourDictionary();
		}

		protected override void OnEnable()
		{
			GameObject.ModifiedEvent.Add(GameObjectModified);
		}

		protected override void OnDisable()
		{
			GameObject.ModifiedEvent.Remove(GameObjectModified);
		}

		private void GameObjectModified(GameObjectChange change)
		{
			if(change == GameObjectChange.ComponentStructure)
			{
				UpdateBehaviourDictionary();
			}
		}

		private void UpdateBehaviourDictionary()
		{
			NetCodeBehaviour[] behaviours = GetComponents<NetCodeBehaviour>();
			for(int i = 0; i < behaviours.Length; i++)
			{
				NetCodeBehaviour netBehaviour = behaviours[i];
				if(!netCodeBehaviours.ContainsValue(netBehaviour))
				{
					netBehaviour.NetBehaviourIndex = ++netCodeId;
					netCodeBehaviours.Add(netCodeId, netBehaviour);
				}
			}
		}

		//Convert object fields to data stream and hand them to NetCodeServer.
		internal SerializeNetCodeData SerializeFromObject()
		{
			SerializeNetCodeData serializeData = new SerializeNetCodeData()
			{
				NetId = netId,
			};
			foreach (NetCodeBehaviour behaviour in netCodeBehaviours.Values)
			{
				if (!behaviour.Enabled)
					continue;
				SerializedObjectData data = behaviour.Serialize();
				if (!string.IsNullOrWhiteSpace(data.Stream))
				{
					serializeData.Data.Add(data);
				}
			}
			return serializeData;
		}

		//Recieve data stream from NetCodeServer and update the objects field.
		internal void DeserializeToObject(SerializeNetCodeData serializeData)
		{
			foreach (SerializedObjectData data in serializeData.Data)
			{
				ObjectStream stream = new ObjectStream();
				stream.Stream = data.Stream;
				netCodeBehaviours[data.BehaviourId].Deserialize(stream);
			}
		}
	}
}