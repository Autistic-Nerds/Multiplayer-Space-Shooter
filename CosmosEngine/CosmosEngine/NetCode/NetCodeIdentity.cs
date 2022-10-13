using CosmosEngine;
using CosmosEngine.Collection;
using CosmosEngine.Netcode.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;

namespace CosmosEngine.Netcode
{
	public class NetcodeIdentity : GameBehaviour
	{
		private uint reliableMsgKey;

		private const BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
		private readonly Dictionary<uint, NetcodeBehaviour> netcodeBehaviours = new Dictionary<uint, NetcodeBehaviour>();
		private readonly Dictionary<uint, RemoteProcedureCall> remoteProduceCallQueue = new Dictionary<uint, RemoteProcedureCall>();
		private readonly List<NetcodeRPC> remoteProduceCallAwaiting = new List<NetcodeRPC>();

		private uint netId;
		private uint netcodeId;

		private bool isServer;
		private bool isClient;
		private bool isLocal;
		private bool hasAuthority;

		/// <summary>
		/// 
		/// </summary>
		public bool IsConnected => NetcodeHandler.IsConnected;
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
		public bool HasAuthority { get => hasAuthority; set => hasAuthority = value; }

		/// <summary>
		/// The unique network Id of this object.
		/// <para>This is assigned at runtime by the network server and will be unique for all objects for that network session.</para>
		/// </summary>
		public uint NetId { get => netId; set => netId = value; }

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

		protected override void LateUpdate()
		{
			InvokeRemoteProduceCalls();
		}

		private void InvokeRemoteProduceCalls()
		{
			if(remoteProduceCallQueue.Count > 0)
			{
				foreach(RemoteProcedureCall call in remoteProduceCallQueue.Values)
				{
					NetcodeRPC rpc = new NetcodeRPC()
					{
						RPI = reliableMsgKey++,
						Call = call,
						NetId = this.NetId,
					};
					if (reliableMsgKey == uint.MaxValue)
						reliableMsgKey = 0;

					if (call.Target != null)
					{
						NetcodeServer.Instance.NetcodeTransport.SendToClient(new NetcodeMessage()
						{
							Data = rpc,
						}, call.Target);
					}
					else
					{
						if (NetcodeHandler.IsServer)
						{
							NetcodeServer.Instance.SendToConnectedClients(new NetcodeMessage()
							{
								Data = rpc,
							});
						}
						else if (NetcodeHandler.IsClient)
						{
							NetcodeServer.Instance.NetcodeTransport.SendToServer(new NetcodeMessage()
							{
								Data = rpc,
							});
						}
					}

					remoteProduceCallAwaiting.Add(rpc);
				}
				remoteProduceCallQueue.Clear();
			}
		}

		#region Remote Procedure Call (RPC)

		/// <summary>
		/// Invokes a remote procedure call on this <see cref="CosmosEngine.Netcode.NetcodeIdentity"/>, a method must be marked for RPC operation.A method must be marked with an appropriate attribute to work as an RPC.
		/// <list type="bullet">
		/// <item><see cref="CosmosEngine.Netcode.ClientRPCAttribute"/>: Can only be invoked by clients, this is also true if the client is the host. This method will be executed on the server. For a client to invoke an RPC they must have Authority on the Netcode Object they call it on.</item>
		/// <item><see cref="CosmosEngine.Netcode.ServerRPCAttribute"/>: Can only be invoked by the server, this is also true if a client is the host. This method will be executed on all clients connected to the server.</item>
		/// </list>
		/// <para>It is possible to use RPC for overload methods, but can't differentiate between overload methods with the same number of parameters. Method overloading used for RPC should always have different parameter count.</para>
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="index"></param>
		/// <param name="parameters"></param>
		//public void Rpc(string methodName, uint index, params object[] parameters) => Rpc(methodName, index, null, parameters);

		/// <summary>
		/// <inheritdoc cref="Rpc(string, uint, object[])"/>
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="index"></param>>	CosmosEngine.dll!CosmosEngine.Netcode.NetcodeIdentity.Rpc(string methodName, uint index, object[] parameters) Line 162	C#

		/// <param name="target"></param>
		/// <param name="parameters"></param>
		public void Rpc(string methodName, uint index, NetcodeClient? target, params object[] parameters)
		{
			//Does this client have authority? Or should we ignore it?
			//Does the method has the right Attribute?
			//Does any method actually exist with these parameters.
			//If all are true - Add to RpcCallstack

			if (!NetcodeHandler.IsConnected)
				return;

			Debug.Log($"RPC: {methodName}");

			NetcodeBehaviour behaviour = netcodeBehaviours[index];
			System.Type[] parametersType = new System.Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
				parametersType[i] = parameters[i].GetType();

			MethodInfo[] methodInfos = behaviour.GetType().GetMethods(Flags);
			MethodInfo method = null;
			foreach (MethodInfo info in methodInfos)
			{
				if (info.Name.Equals(methodName))
				{
					if (info.GetParameters().Length == parameters.Length)
					{
						method = info;
						break;
					}
				}
			}

			if (method == null)
			{
				Debug.Log($"No method named {methodName} match parameters {parameters.ParametersTypeToString()} on {GetType().FullName}. RPC was unsuccessful.", LogFormat.Error);
				return;
			}

			string[] args = new string[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				args[i] = JsonConvert.SerializeObject(parameters[i]);
			}
			RemoteProcedureCall rpc = new RemoteProcedureCall()
			{
				Method = methodName,
				RPI = reliableMsgKey++,
				Index = index,
				Args = args,
				Target = target,
			};

			remoteProduceCallQueue.Add(reliableMsgKey, rpc);
		}

		internal void ExecuteRpc(RemoteProcedureCall call)
		{
			UpdateBehaviourDictionary();
			NetcodeBehaviour behaviour = netcodeBehaviours[call.Index];
			Debug.Log($"Executing RPC: {call.Method} on {(behaviour == null ? "null" : behaviour.GetType().Name)}");
			if (behaviour != null)
			{
				MethodInfo[] methodOnBehaviour = behaviour.GetType().GetMethods(Flags);
				foreach(MethodInfo method in methodOnBehaviour)
				{
					if(!method.Name.Equals(call.Method))
						continue;

					ParameterInfo[] methodParameters = method.GetParameters();
					if (methodParameters.Length != call.Args.Length)
						continue;

					object[] args = new object[call.Args.Length];
					for(int i = 0; i < args.Length; i++)
					{
						args[i] = JsonConvert.DeserializeObject(call.Args[i], methodParameters[i].ParameterType);
					}
					method.Invoke(behaviour, args);
					break;
				}
			}
		}

		#endregion

		#region Serialize

		//Convert object fields to data stream and hand them to NetcodeServer.
		internal SerializeNetcodeData? SerializeFromObject()
		{
			SerializeNetcodeData serializeData = new SerializeNetcodeData()
			{
				NetId = netId,
			};
			foreach (NetcodeBehaviour behaviour in netcodeBehaviours.Values)
			{
				if (!behaviour.Enabled)
					continue;

				SerializedObjectData data = behaviour.SerializeObject();
				if (!string.IsNullOrWhiteSpace(data.Stream) || data.Stream.Equals("||"))
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
			//If I have authority and is serializing to the object, I shouldn't care because my data is already correct.
			if (HasAuthority)
				return;

			foreach (SerializedObjectData data in serializeData.Data)
			{
				NetcodeReader stream = new NetcodeReader(data.Stream);
				stream.Open();
				netcodeBehaviours[data.BehaviourId].DeserializeObject(stream);
				stream.Close();
			}
		}

		#endregion
	}
}