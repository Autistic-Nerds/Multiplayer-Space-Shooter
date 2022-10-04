using CosmosEngine;
using CosmosEngine.NetCode.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace CosmosEngine.NetCode
{
	[RequireComponent(typeof(NetCodeIdentity))]
	public abstract class NetCodeBehaviour : GameBehaviour
	{
		private const BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

		private NetCodeIdentity netIdentity;
		private uint behaviourIndex;
		private object[] syncVarDirtyBits;
		private Dictionary<string, FieldInfo> syncVarFields;

		public NetCodeIdentity NetIdentity => netIdentity;

		/// <summary>
		/// <inheritdoc cref="NetCode.NetCodeIdentity.IsServer"/>
		/// </summary>
		public bool IsServer => NetIdentity.IsServer;

		/// <summary>
		/// <inheritdoc cref="NetCode.NetCodeIdentity.IsClient"/>
		/// </summary>
		public bool IsClient => NetIdentity.IsClient;

		/// <summary>
		/// <inheritdoc cref="NetCode.NetCodeIdentity.IsLocal"/>
		/// </summary>
		public bool IsLocal => NetIdentity.IsLocal;

		/// <summary>
		/// <inheritdoc cref="NetCode.NetCodeIdentity.HasAuthority"/>
		/// </summary>
		public bool HasAuthority => NetIdentity.HasAuthority;

		/// <summary>
		/// <inheritdoc cref="NetCode.NetCodeIdentity.NetId"/>
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

		internal SerializedObjectData Serialize()
		{
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

		public virtual void OnSerialize()
		{

		}

		internal void Deserialize(ObjectStream dataStream)
		{
			foreach(SerializedField data in dataStream.ReadToEnd())
			{
				FieldInfo field = syncVarFields[data.Name];
				object value = JsonConvert.DeserializeObject((string)data.Value, field.FieldType);
				//Debug.Log($"FIELD: {data.Name} = {value.GetType()} | {field.FieldType} DATA: {value}");

				if (value != null)
				{
					field.SetValue(this, value);
				}

				SyncVarAttribute syncVarAttribute = field.GetCustomAttribute<SyncVarAttribute>();
				if(!string.IsNullOrWhiteSpace(syncVarAttribute.hook))
				{
					MethodInfo methodHook = GetType().GetMethod(syncVarAttribute.hook, Flags);
					if(methodHook != null)
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

		public virtual void OnDeserialize()
		{

		}
	}
}