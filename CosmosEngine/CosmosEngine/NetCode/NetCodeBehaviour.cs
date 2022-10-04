using CosmosEngine;
using CosmosEngine.Netcode.Serialization;
using Newtonsoft.Json;
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
		private Dictionary<string, FieldInfo> syncVarFields;

		public NetcodeIdentity NetIdentity => netIdentity;

		/// <summary>
		/// <inheritdoc cref="Netcode.NetcodeIdentity.IsServer"/>
		/// </summary>
		public bool IsServer => NetIdentity.IsServer;

		/// <summary>
		/// <inheritdoc cref="Netcode.NetcodeIdentity.IsClient"/>
		/// </summary>
		public bool IsClient => NetIdentity.IsClient;

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

		protected override void Start()
		{
			InitialSyncFields();
		}

		private void InitialSyncFields()
		{
			syncVarFields = new Dictionary<string, FieldInfo>();
			FieldInfo[] fields = this.GetType().GetFields(Flags);
			syncVarDirtyBits = new object[fields.Length];
			int index = 0;
			foreach (FieldInfo field in fields)
			{
				if (field.IsSyncVar())
				{
					this.syncVarFields.Add(field.Name, field);
					syncVarDirtyBits[index] = field.GetValue(this);
					index++;
				}
			}
		}

		public void Rpc(string methodName, params object[] parameters) => NetIdentity.Rpc(methodName, parameters);

		internal void RecieveRpc(NetcodeRPC call)
		{
			MethodInfo method = GetType().GetMethod(call.Method, Flags);
			method.Invoke(method, null);
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
				FieldInfo field = syncVarFields[data.Name];
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

			SerializeSyncVars(ref stream);
			Serialize(ref stream);

			stream.Close();

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

		}

		public virtual void Serialize(ref NetcodeWriter stream)
		{

		}

		#endregion

		#region Deserialize

		internal void DeserializeObject(NetcodeReader stream)
		{
			DeserializeSyncVars(ref stream);
			Deserialize(ref stream);
		}

		private void DeserializeSyncVars(ref NetcodeReader stream)
		{

		}

		public virtual void Deserialize(ref NetcodeReader stream)
		{


		}

		#endregion

		#endregion
	}
}