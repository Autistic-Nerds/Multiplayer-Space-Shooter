using CosmosEngine;
using CosmosEngine.Netcode.Serialization;
using Newtonsoft.Json;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CosmosEngine.Netcode
{
	[RequireComponent(typeof(NetcodeIdentity))]
	public abstract class NetcodeBehaviour : GameBehaviour
	{
		private const BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

		private NetcodeIdentity netIdentity;
		private uint behaviourIndex;
		private object[] syncVarDirtyBits;
		private Dictionary<byte, FieldInfo> syncVarFields;

		public NetcodeIdentity NetIdentity => netIdentity ??= GetComponent<NetcodeIdentity>();

		/// <summary>
		/// <inheritdoc cref="Netcode.NetcodeIdentity.IsConnected"></inheritdoc>/>
		/// </summary>
		public bool IsConnected => NetIdentity.IsConnected;

		/// <summary>
		/// <inheritdoc cref="Netcode.NetcodeIdentity.IsServer"/>
		/// </summary>
		public bool IsServer => NetIdentity.IsServer;

		/// <summary>
		/// <inheritdoc cref="Netcode.NetcodeIdentity.IsClient"/>
		/// </summary>
		public bool IsClient => NetIdentity.IsClient && NetIdentity;

		/// <summary>
		/// <inheritdoc cref="Netcode.NetcodeIdentity.IsLocal"/>
		/// </summary>
		public bool IsLocal => NetIdentity.IsLocal;

		/// <summary>
		/// <inheritdoc cref="Netcode.NetcodeIdentity.HasAuthority"/>
		/// </summary>
		public bool HasAuthority => NetIdentity.HasAuthority;

		/// <summary>
		/// <inheritdoc cref="Netcode.NetcodeIdentity.NetId"/>
		/// </summary>
		public uint NetId => NetIdentity.NetId;

		internal uint NetBehaviourIndex { get => behaviourIndex; set => behaviourIndex = value; }

		protected override void OnInstantiated()
		{
			base.OnInstantiated();
			InitialSyncFields();
		}

		private void InitialSyncFields()
		{
			syncVarFields = new Dictionary<byte, FieldInfo>();
			FieldInfo[] fields = this.GetType().GetFields(Flags);
			syncVarDirtyBits = new object[fields.Length];
			byte index = 0;
			foreach (FieldInfo field in fields)
			{
				if (field.IsSyncVar())
				{
					this.syncVarFields.Add(index, field);
					syncVarDirtyBits[index] = field.GetValue(this);
					index++;
				}
			}
		}

		//public void Rpc(string methodName, params object[] parameters)
		//{
		//	if(NetIdentity == null)
		//	{
		//		Debug.Log($"Attempting to send RPC without an NetcodeIdentity", LogFormat.Warning);
		//		return;
		//	}
		//	NetIdentity.Rpc(methodName, NetBehaviourIndex, parameters);
		//}

		public void Rpc(string methodName, NetcodeClient? target, params object[] parameters)
		{
			if (NetIdentity == null)
			{
				Debug.Log($"Attempting to send RPC without an NetcodeIdentity", LogFormat.Warning);
				return;
			}
			NetIdentity.Rpc(methodName, NetBehaviourIndex, target, parameters);
		}

		internal void ExecuteRpc(RemoteProcedureCall call)
		{
			//MethodInfo method = GetType().GetMethod(call.Method, Flags);
			//method.Invoke(method, null);
		}

		#region Serialize / Deserialize


		//Seralize =>
		//Write all changed sync variables to the NetcodeStream
		//Allow the author to write to the NetcodeStream custom data

		//Deserialize =>
		//Read all data that is named (sync vars)
		//Allow user to read data the same sequence as it's written.

		[System.Obsolete("Old version of serialize method - use SerializeObject instead", false)]
		internal SerializedObjectData OnSerialize()
		{
			SerializeObject();
			ObjectStream stream = new ObjectStream();
			stream.Open();
			int index = 0;
			foreach (FieldInfo field in syncVarFields.Values)
			{
				object value = field.GetValue(this);

				SyncVarAttribute syncVarAttribute = field.GetCustomAttribute<SyncVarAttribute>();
				if (!value.Equals(syncVarDirtyBits[index]) || syncVarAttribute.ForceSync)
				{
					stream.Write(field, this);
					syncVarDirtyBits[index] = value;
				}
				index++;
			}
			stream.Close();
			SerializedObjectData data = new SerializedObjectData()
			{
				BehaviourId = behaviourIndex,
				Stream = stream.Stream,
			};
			return data;
		}

		[System.Obsolete("Old version of deserialize method - use DeserializeObject instead", false)]
		internal void OnDeserialize(ObjectStream dataStream)
		{
			foreach (SerializedField data in dataStream.ReadToEnd())
			{
				FieldInfo field = syncVarFields[data.Index];
				object value = JsonConvert.DeserializeObject((string)data.Value, field.FieldType);
				//Debug.Log($"FIELD: {data.Name} = {value.GetType()} | {field.FieldType} DATA: {value}");

				if (value != null)
				{
					field.SetValue(this, value);
				}

				SyncVarAttribute syncVarAttribute = field.GetCustomAttribute<SyncVarAttribute>();
				if (!string.IsNullOrWhiteSpace(syncVarAttribute.hook))
				{
					MethodInfo methodHook = GetType().GetMethod(syncVarAttribute.hook, Flags);
					if (methodHook != null)
					{
						methodHook.Invoke(this, null);
					}
					else
					{
						Debug.Log($"Hook ({syncVarAttribute.hook}) attached to SyncVar '{field.Name}' - but no such method exist on {GetType().Name}", LogFormat.Warning);
					}
				}
			}
		}

		#region Serialize

		internal SerializedObjectData SerializeObject()
		{
			NetcodeWriter stream = new NetcodeWriter();
			stream.Open();

			if (HasAuthority)
			{
				SerializeSyncVars(ref stream);
				Serialize(ref stream);
			}

			stream.Close();
			return new SerializedObjectData()
			{
				Stream = stream.Export(),
				BehaviourId = behaviourIndex,
			};

			//SerializedObjectData serializedData = new SerializedObjectData()
			//{
			//	BehaviourId = behaviourIndex,
			//	Stream = stream.Stream,
			//};
			//return serializedData;
			return default(SerializedObjectData);
		}

		private void SerializeSyncVars(ref NetcodeWriter stream)
		{
			byte index = 0;
			bool dirtyData = false;
			foreach (FieldInfo field in syncVarFields.Values)
			{
				object value = field.GetValue(this);
				SyncVarAttribute syncVarAttribute = field.GetCustomAttribute<SyncVarAttribute>();
				if (!value.Equals(syncVarDirtyBits[index]) || syncVarAttribute.ForceSync)
				{
					stream.WriteSyncVar(index, value);
					syncVarDirtyBits[index] = value;
					dirtyData = true;
				}
				index++;
			}
			if (dirtyData)
				stream.FinializeSyncVar();
		}

		public virtual void Serialize(ref NetcodeWriter stream)
		{

		}

		#endregion

		#region Deserialize

		internal void DeserializeObject(NetcodeReader stream)
		{
			if (!HasAuthority)
			{
				DeserializeSyncVars(ref stream);
				Deserialize(ref stream);
			}
		}

		private void DeserializeSyncVars(ref NetcodeReader stream)
		{
			foreach (SerializedField data in stream.ReadSyncVars())
			{
				//Debug.Log($"DESERIALIZE DATA: {data.Name} - VALUE: {data.Value}");
				//continue;
				FieldInfo field = syncVarFields[data.Index];
				object value = JsonConvert.DeserializeObject(data.Value, field.FieldType);

				if (value != null)
				{
					field.SetValue(this, value);
					SyncVarAttribute syncVarAttribute = field.GetCustomAttribute<SyncVarAttribute>();
					if (!string.IsNullOrWhiteSpace(syncVarAttribute.hook))
					{
						MethodInfo methodHook = GetType().GetMethod(syncVarAttribute.hook, Flags);
						if (methodHook != null)
						{
							methodHook.Invoke(this, null);
						}
						else
						{
							Debug.Log($"Hook ({syncVarAttribute.hook}) attached to SyncVar '{field.Name}' - but no such method exist on {GetType().Name}", LogFormat.Warning);
						}
					}
				}
			}
		}

		public virtual void Deserialize(ref NetcodeReader stream)
		{


		}

		#endregion

		#endregion
	}
}