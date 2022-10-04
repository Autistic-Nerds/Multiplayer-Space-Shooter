using CosmosEngine;
using CosmosEngine.Collection;
using CosmosEngine.Netcode.Serialization;
using System.Collections.Generic;

namespace CosmosEngine.Netcode
{
	public class NetcodeIdentity : GameBehaviour
	{
		private readonly Dictionary<uint, NetcodeBehaviour> netcodeBehaviours = new Dictionary<uint, NetcodeBehaviour>();

		private uint netId;
		private uint netcodeId;

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
			NetcodeBehaviour[] behaviours = GetComponents<NetcodeBehaviour>();
			for(int i = 0; i < behaviours.Length; i++)
			{
				NetcodeBehaviour netBehaviour = behaviours[i];
				if(!netcodeBehaviours.ContainsValue(netBehaviour))
				{
					netBehaviour.NetBehaviourIndex = ++netcodeId;
					netcodeBehaviours.Add(netcodeId, netBehaviour);
				}
			}
		}

		#region Remote Procedure Call (RPC)

		public void Rpc(string methodName, params object[] parameters)
		{
			//Does this client have authority? Or should we ignore it?
			//Does the method has the right Attribute?
			//Does any method actually exist with these parameters.

			//If all are true - Add to RpcCallstack
		}

		internal void RecieveRpc(NetcodeRPC call)
		{
			netcodeBehaviours[call.BehaviourId].RecieveRpc(call);
		}

		#endregion

		#region Serialize

		//Convert object fields to data stream and hand them to NetcodeServer.
		internal SerializeNetcodeData SerializeFromObject()
		{
			SerializeNetcodeData serializeData = new SerializeNetcodeData()
			{
				NetId = netId,
			};
			foreach (NetcodeBehaviour behaviour in netcodeBehaviours.Values)
			{
				if (!behaviour.Enabled)
					continue;
				SerializedObjectData data = behaviour.OnSerialize();
				if (!string.IsNullOrWhiteSpace(data.Stream))
				{
					serializeData.Data.Add(data);
				}
			}
			return serializeData;
		}

		#endregion

		#region Deserialize

		//Recieve data stream from NetcodeServer and update the objects field.
		internal void DeserializeToObject(SerializeNetcodeData serializeData)
		{
			foreach (SerializedObjectData data in serializeData.Data)
			{
				ObjectStream stream = new ObjectStream();
				stream.Stream = data.Stream;
				netcodeBehaviours[data.BehaviourId].OnDeserialize(stream);
			}
		}

		#endregion
	}
}