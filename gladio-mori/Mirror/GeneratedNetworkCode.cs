using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Dissonance.Integrations.MirrorIgnorance;
using MoveClasses;
using UnityEngine;
using Utils;

namespace Mirror
{
	// Token: 0x02000317 RID: 791
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public static class GeneratedNetworkCode
	{
		// Token: 0x060017A0 RID: 6048 RVA: 0x00077248 File Offset: 0x00075448
		public static TimeSnapshotMessage TimeSnapshotMessage(NetworkReader reader)
		{
			return default(TimeSnapshotMessage);
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x00077260 File Offset: 0x00075460
		public static void TimeSnapshotMessage(NetworkWriter writer, TimeSnapshotMessage value)
		{
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x00077270 File Offset: 0x00075470
		public static ReadyMessage ReadyMessage(NetworkReader reader)
		{
			return default(ReadyMessage);
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00077288 File Offset: 0x00075488
		public static void ReadyMessage(NetworkWriter writer, ReadyMessage value)
		{
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x00077298 File Offset: 0x00075498
		public static NotReadyMessage NotReadyMessage(NetworkReader reader)
		{
			return default(NotReadyMessage);
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x000772B0 File Offset: 0x000754B0
		public static void NotReadyMessage(NetworkWriter writer, NotReadyMessage value)
		{
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x000772C0 File Offset: 0x000754C0
		public static AddPlayerMessage AddPlayerMessage(NetworkReader reader)
		{
			return default(AddPlayerMessage);
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x000772D8 File Offset: 0x000754D8
		public static void AddPlayerMessage(NetworkWriter writer, AddPlayerMessage value)
		{
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x000772E8 File Offset: 0x000754E8
		public static SceneMessage SceneMessage(NetworkReader reader)
		{
			return new SceneMessage
			{
				sceneName = reader.ReadString(),
				sceneOperation = GeneratedNetworkCode._Read_Mirror.SceneOperation(reader),
				customHandling = reader.ReadBool()
			};
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x00077330 File Offset: 0x00075530
		public static SceneOperation SceneOperation(NetworkReader reader)
		{
			return (SceneOperation)reader.ReadByte();
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x00077344 File Offset: 0x00075544
		public static void SceneMessage(NetworkWriter writer, SceneMessage value)
		{
			writer.WriteString(value.sceneName);
			GeneratedNetworkCode._Write_Mirror.SceneOperation(writer, value.sceneOperation);
			writer.WriteBool(value.customHandling);
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x00077378 File Offset: 0x00075578
		public static void SceneOperation(NetworkWriter writer, SceneOperation value)
		{
			writer.WriteByte((byte)value);
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x0007738C File Offset: 0x0007558C
		public static CommandMessage CommandMessage(NetworkReader reader)
		{
			return new CommandMessage
			{
				netId = reader.ReadUInt(),
				componentIndex = reader.ReadByte(),
				functionHash = reader.ReadUShort(),
				payload = reader.ReadBytesAndSizeSegment()
			};
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x000773E0 File Offset: 0x000755E0
		public static void CommandMessage(NetworkWriter writer, CommandMessage value)
		{
			writer.WriteUInt(value.netId);
			writer.WriteByte(value.componentIndex);
			writer.WriteUShort(value.functionHash);
			writer.WriteBytesAndSizeSegment(value.payload);
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x00077420 File Offset: 0x00075620
		public static RpcMessage RpcMessage(NetworkReader reader)
		{
			return new RpcMessage
			{
				netId = reader.ReadUInt(),
				componentIndex = reader.ReadByte(),
				functionHash = reader.ReadUShort(),
				payload = reader.ReadBytesAndSizeSegment()
			};
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x00077474 File Offset: 0x00075674
		public static void RpcMessage(NetworkWriter writer, RpcMessage value)
		{
			writer.WriteUInt(value.netId);
			writer.WriteByte(value.componentIndex);
			writer.WriteUShort(value.functionHash);
			writer.WriteBytesAndSizeSegment(value.payload);
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x000774B4 File Offset: 0x000756B4
		public static SpawnMessage SpawnMessage(NetworkReader reader)
		{
			return new SpawnMessage
			{
				netId = reader.ReadUInt(),
				isLocalPlayer = reader.ReadBool(),
				isOwner = reader.ReadBool(),
				sceneId = reader.ReadULong(),
				assetId = reader.ReadUInt(),
				position = reader.ReadVector3(),
				rotation = reader.ReadQuaternion(),
				scale = reader.ReadVector3(),
				payload = reader.ReadBytesAndSizeSegment()
			};
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x00077554 File Offset: 0x00075754
		public static void SpawnMessage(NetworkWriter writer, SpawnMessage value)
		{
			writer.WriteUInt(value.netId);
			writer.WriteBool(value.isLocalPlayer);
			writer.WriteBool(value.isOwner);
			writer.WriteULong(value.sceneId);
			writer.WriteUInt(value.assetId);
			writer.WriteVector3(value.position);
			writer.WriteQuaternion(value.rotation);
			writer.WriteVector3(value.scale);
			writer.WriteBytesAndSizeSegment(value.payload);
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x000775D0 File Offset: 0x000757D0
		public static ChangeOwnerMessage ChangeOwnerMessage(NetworkReader reader)
		{
			return new ChangeOwnerMessage
			{
				netId = reader.ReadUInt(),
				isOwner = reader.ReadBool(),
				isLocalPlayer = reader.ReadBool()
			};
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x00077618 File Offset: 0x00075818
		public static void ChangeOwnerMessage(NetworkWriter writer, ChangeOwnerMessage value)
		{
			writer.WriteUInt(value.netId);
			writer.WriteBool(value.isOwner);
			writer.WriteBool(value.isLocalPlayer);
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x0007764C File Offset: 0x0007584C
		public static ObjectSpawnStartedMessage ObjectSpawnStartedMessage(NetworkReader reader)
		{
			return default(ObjectSpawnStartedMessage);
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00077664 File Offset: 0x00075864
		public static void ObjectSpawnStartedMessage(NetworkWriter writer, ObjectSpawnStartedMessage value)
		{
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x00077674 File Offset: 0x00075874
		public static ObjectSpawnFinishedMessage ObjectSpawnFinishedMessage(NetworkReader reader)
		{
			return default(ObjectSpawnFinishedMessage);
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x0007768C File Offset: 0x0007588C
		public static void ObjectSpawnFinishedMessage(NetworkWriter writer, ObjectSpawnFinishedMessage value)
		{
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x0007769C File Offset: 0x0007589C
		public static ObjectDestroyMessage ObjectDestroyMessage(NetworkReader reader)
		{
			return new ObjectDestroyMessage
			{
				netId = reader.ReadUInt()
			};
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x000776C4 File Offset: 0x000758C4
		public static void ObjectDestroyMessage(NetworkWriter writer, ObjectDestroyMessage value)
		{
			writer.WriteUInt(value.netId);
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x000776E0 File Offset: 0x000758E0
		public static ObjectHideMessage ObjectHideMessage(NetworkReader reader)
		{
			return new ObjectHideMessage
			{
				netId = reader.ReadUInt()
			};
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x00077708 File Offset: 0x00075908
		public static void ObjectHideMessage(NetworkWriter writer, ObjectHideMessage value)
		{
			writer.WriteUInt(value.netId);
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x00077724 File Offset: 0x00075924
		public static EntityStateMessage EntityStateMessage(NetworkReader reader)
		{
			return new EntityStateMessage
			{
				netId = reader.ReadUInt(),
				payload = reader.ReadBytesAndSizeSegment()
			};
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x0007775C File Offset: 0x0007595C
		public static void EntityStateMessage(NetworkWriter writer, EntityStateMessage value)
		{
			writer.WriteUInt(value.netId);
			writer.WriteBytesAndSizeSegment(value.payload);
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x00077784 File Offset: 0x00075984
		public static NetworkPingMessage NetworkPingMessage(NetworkReader reader)
		{
			return new NetworkPingMessage
			{
				localTime = reader.ReadDouble()
			};
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x000777AC File Offset: 0x000759AC
		public static void NetworkPingMessage(NetworkWriter writer, NetworkPingMessage value)
		{
			writer.WriteDouble(value.localTime);
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x000777C8 File Offset: 0x000759C8
		public static NetworkPongMessage NetworkPongMessage(NetworkReader reader)
		{
			return new NetworkPongMessage
			{
				localTime = reader.ReadDouble()
			};
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x000777F0 File Offset: 0x000759F0
		public static void NetworkPongMessage(NetworkWriter writer, NetworkPongMessage value)
		{
			writer.WriteDouble(value.localTime);
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x0007780C File Offset: 0x00075A0C
		public static MultiplayerRoomManager.SceneLoaded _Read_MultiplayerRoomManager/SceneLoaded(NetworkReader reader)
		{
			return new MultiplayerRoomManager.SceneLoaded
			{
				loaded = reader.ReadBool()
			};
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x00077834 File Offset: 0x00075A34
		public static void _Write_MultiplayerRoomManager/SceneLoaded(NetworkWriter writer, MultiplayerRoomManager.SceneLoaded value)
		{
			writer.WriteBool(value.loaded);
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x00077850 File Offset: 0x00075A50
		public static void _Write_LobbyPrivacyType(NetworkWriter writer, LobbyPrivacyType value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x00077864 File Offset: 0x00075A64
		public static void _Write_AllowedMovesetTypes(NetworkWriter writer, AllowedMovesetTypes value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x00077878 File Offset: 0x00075A78
		public static void _Write_GameTypes(NetworkWriter writer, GameTypes value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x0007788C File Offset: 0x00075A8C
		public static void List(NetworkWriter writer, List<EquipmentType> value)
		{
			writer.WriteList(value);
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x000778A0 File Offset: 0x00075AA0
		public static void EquipmentType(NetworkWriter writer, EquipmentType value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x000778B4 File Offset: 0x00075AB4
		public static LobbyPrivacyType _Read_LobbyPrivacyType(NetworkReader reader)
		{
			return (LobbyPrivacyType)reader.ReadInt();
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x000778C8 File Offset: 0x00075AC8
		public static AllowedMovesetTypes _Read_AllowedMovesetTypes(NetworkReader reader)
		{
			return (AllowedMovesetTypes)reader.ReadInt();
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x000778DC File Offset: 0x00075ADC
		public static GameTypes _Read_GameTypes(NetworkReader reader)
		{
			return (GameTypes)reader.ReadInt();
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x000778F0 File Offset: 0x00075AF0
		public static List<EquipmentType> List(NetworkReader reader)
		{
			return reader.ReadList<EquipmentType>();
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x00077904 File Offset: 0x00075B04
		public static EquipmentType EquipmentType(NetworkReader reader)
		{
			return (EquipmentType)reader.ReadInt();
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x00077918 File Offset: 0x00075B18
		public static void DeathReason(NetworkWriter writer, DeathReason value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x0007792C File Offset: 0x00075B2C
		public static DeathReason DeathReason(NetworkReader reader)
		{
			return (DeathReason)reader.ReadInt();
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x00077940 File Offset: 0x00075B40
		public static void List(NetworkWriter writer, List<EquippedEquipment> value)
		{
			writer.WriteList(value);
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x00077954 File Offset: 0x00075B54
		public static void EquippedEquipment(NetworkWriter writer, EquippedEquipment value)
		{
			if (value == null)
			{
				writer.WriteBool(false);
				return;
			}
			writer.WriteBool(true);
			writer.WriteInt(value.positionInt);
			writer.WriteInt(value.equipmentTypeInt);
			writer.WriteInt(value.equipmentStartHoldTypeInt);
			writer.WriteFloat(value.equipmentStartHoldPosition);
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x000779A8 File Offset: 0x00075BA8
		public static List<EquippedEquipment> List(NetworkReader reader)
		{
			return reader.ReadList<EquippedEquipment>();
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x000779BC File Offset: 0x00075BBC
		public static EquippedEquipment EquippedEquipment(NetworkReader reader)
		{
			if (!reader.ReadBool())
			{
				return null;
			}
			return new EquippedEquipment
			{
				positionInt = reader.ReadInt(),
				equipmentTypeInt = reader.ReadInt(),
				equipmentStartHoldTypeInt = reader.ReadInt(),
				equipmentStartHoldPosition = reader.ReadFloat()
			};
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x00077A1C File Offset: 0x00075C1C
		public static void _Write_CollisionSoundType(NetworkWriter writer, CollisionSoundType value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x00077A30 File Offset: 0x00075C30
		public static CollisionSoundType _Read_CollisionSoundType(NetworkReader reader)
		{
			return (CollisionSoundType)reader.ReadInt();
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00077A44 File Offset: 0x00075C44
		public static void _Write_BluntDamageEffect(NetworkWriter writer, BluntDamageEffect value)
		{
			GeneratedNetworkCode._Write_MoveClasses.JointType(writer, value.BodyPart);
			writer.WriteVector3(value.Position);
			writer.WriteFloat(value.Damage);
			writer.WriteFloat(value.BloodDamage);
			writer.WriteFloat(value.Volume);
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00077A90 File Offset: 0x00075C90
		public static void JointType(NetworkWriter writer, JointType value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00077AA4 File Offset: 0x00075CA4
		public static BluntDamageEffect _Read_BluntDamageEffect(NetworkReader reader)
		{
			return new BluntDamageEffect
			{
				BodyPart = GeneratedNetworkCode._Read_MoveClasses.JointType(reader),
				Position = reader.ReadVector3(),
				Damage = reader.ReadFloat(),
				BloodDamage = reader.ReadFloat(),
				Volume = reader.ReadFloat()
			};
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x00077B08 File Offset: 0x00075D08
		public static JointType JointType(NetworkReader reader)
		{
			return (JointType)reader.ReadInt();
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x00077B1C File Offset: 0x00075D1C
		public static void _Write_DamageOrigin(NetworkWriter writer, DamageOrigin value)
		{
			GeneratedNetworkCode._Write_EnvironmentSoundType(writer, value.EnvironmentSoundType);
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00077B38 File Offset: 0x00075D38
		public static void _Write_EnvironmentSoundType(NetworkWriter writer, EnvironmentSoundType value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x00077B4C File Offset: 0x00075D4C
		public static DamageOrigin _Read_DamageOrigin(NetworkReader reader)
		{
			return new DamageOrigin
			{
				EnvironmentSoundType = GeneratedNetworkCode._Read_EnvironmentSoundType(reader)
			};
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x00077B74 File Offset: 0x00075D74
		public static EnvironmentSoundType _Read_EnvironmentSoundType(NetworkReader reader)
		{
			return (EnvironmentSoundType)reader.ReadInt();
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00077B88 File Offset: 0x00075D88
		public static void NetworkByteMessage(NetworkWriter writer, NetworkByteMessage value)
		{
			writer.WriteInt(value.id);
			writer.WriteInt(value.p);
			writer.WriteInt(value.tp);
			writer.WriteBytesAndSize(value.m);
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x00077BC8 File Offset: 0x00075DC8
		public static NetworkByteMessage NetworkByteMessage(NetworkReader reader)
		{
			return new NetworkByteMessage
			{
				id = reader.ReadInt(),
				p = reader.ReadInt(),
				tp = reader.ReadInt(),
				m = reader.ReadBytesAndSize()
			};
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x00077C1C File Offset: 0x00075E1C
		public static void DefaultMovesetSettings(NetworkWriter writer, DefaultMovesetSettings value)
		{
			if (value == null)
			{
				writer.WriteBool(false);
				return;
			}
			writer.WriteBool(true);
			writer.WriteBool(value.invertVerticalAttacks);
			writer.WriteBool(value.invertHorizontalAttacks);
			writer.WriteBool(value.invertVerticalBlocks);
			writer.WriteBool(value.invertHorizontalBlocks);
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x00077C70 File Offset: 0x00075E70
		public static DefaultMovesetSettings DefaultMovesetSettings(NetworkReader reader)
		{
			if (!reader.ReadBool())
			{
				return null;
			}
			return new DefaultMovesetSettings
			{
				invertVerticalAttacks = reader.ReadBool(),
				invertHorizontalAttacks = reader.ReadBool(),
				invertVerticalBlocks = reader.ReadBool(),
				invertHorizontalBlocks = reader.ReadBool()
			};
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x00077CD0 File Offset: 0x00075ED0
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void InitReadWriters()
		{
			Writer<byte>.write = new Action<NetworkWriter, byte>(NetworkWriterExtensions.WriteByte);
			Writer<byte?>.write = new Action<NetworkWriter, byte?>(NetworkWriterExtensions.WriteByteNullable);
			Writer<sbyte>.write = new Action<NetworkWriter, sbyte>(NetworkWriterExtensions.WriteSByte);
			Writer<sbyte?>.write = new Action<NetworkWriter, sbyte?>(NetworkWriterExtensions.WriteSByteNullable);
			Writer<char>.write = new Action<NetworkWriter, char>(NetworkWriterExtensions.WriteChar);
			Writer<char?>.write = new Action<NetworkWriter, char?>(NetworkWriterExtensions.WriteCharNullable);
			Writer<bool>.write = new Action<NetworkWriter, bool>(NetworkWriterExtensions.WriteBool);
			Writer<bool?>.write = new Action<NetworkWriter, bool?>(NetworkWriterExtensions.WriteBoolNullable);
			Writer<short>.write = new Action<NetworkWriter, short>(NetworkWriterExtensions.WriteShort);
			Writer<short?>.write = new Action<NetworkWriter, short?>(NetworkWriterExtensions.WriteShortNullable);
			Writer<ushort>.write = new Action<NetworkWriter, ushort>(NetworkWriterExtensions.WriteUShort);
			Writer<ushort?>.write = new Action<NetworkWriter, ushort?>(NetworkWriterExtensions.WriteUShortNullable);
			Writer<int>.write = new Action<NetworkWriter, int>(NetworkWriterExtensions.WriteInt);
			Writer<int?>.write = new Action<NetworkWriter, int?>(NetworkWriterExtensions.WriteIntNullable);
			Writer<uint>.write = new Action<NetworkWriter, uint>(NetworkWriterExtensions.WriteUInt);
			Writer<uint?>.write = new Action<NetworkWriter, uint?>(NetworkWriterExtensions.WriteUIntNullable);
			Writer<long>.write = new Action<NetworkWriter, long>(NetworkWriterExtensions.WriteLong);
			Writer<long?>.write = new Action<NetworkWriter, long?>(NetworkWriterExtensions.WriteLongNullable);
			Writer<ulong>.write = new Action<NetworkWriter, ulong>(NetworkWriterExtensions.WriteULong);
			Writer<ulong?>.write = new Action<NetworkWriter, ulong?>(NetworkWriterExtensions.WriteULongNullable);
			Writer<float>.write = new Action<NetworkWriter, float>(NetworkWriterExtensions.WriteFloat);
			Writer<float?>.write = new Action<NetworkWriter, float?>(NetworkWriterExtensions.WriteFloatNullable);
			Writer<double>.write = new Action<NetworkWriter, double>(NetworkWriterExtensions.WriteDouble);
			Writer<double?>.write = new Action<NetworkWriter, double?>(NetworkWriterExtensions.WriteDoubleNullable);
			Writer<decimal>.write = new Action<NetworkWriter, decimal>(NetworkWriterExtensions.WriteDecimal);
			Writer<decimal?>.write = new Action<NetworkWriter, decimal?>(NetworkWriterExtensions.WriteDecimalNullable);
			Writer<string>.write = new Action<NetworkWriter, string>(NetworkWriterExtensions.WriteString);
			Writer<ArraySegment<byte>>.write = new Action<NetworkWriter, ArraySegment<byte>>(NetworkWriterExtensions.WriteBytesAndSizeSegment);
			Writer<byte[]>.write = new Action<NetworkWriter, byte[]>(NetworkWriterExtensions.WriteBytesAndSize);
			Writer<Vector2>.write = new Action<NetworkWriter, Vector2>(NetworkWriterExtensions.WriteVector2);
			Writer<Vector2?>.write = new Action<NetworkWriter, Vector2?>(NetworkWriterExtensions.WriteVector2Nullable);
			Writer<Vector3>.write = new Action<NetworkWriter, Vector3>(NetworkWriterExtensions.WriteVector3);
			Writer<Vector3?>.write = new Action<NetworkWriter, Vector3?>(NetworkWriterExtensions.WriteVector3Nullable);
			Writer<Vector4>.write = new Action<NetworkWriter, Vector4>(NetworkWriterExtensions.WriteVector4);
			Writer<Vector4?>.write = new Action<NetworkWriter, Vector4?>(NetworkWriterExtensions.WriteVector4Nullable);
			Writer<Vector2Int>.write = new Action<NetworkWriter, Vector2Int>(NetworkWriterExtensions.WriteVector2Int);
			Writer<Vector2Int?>.write = new Action<NetworkWriter, Vector2Int?>(NetworkWriterExtensions.WriteVector2IntNullable);
			Writer<Vector3Int>.write = new Action<NetworkWriter, Vector3Int>(NetworkWriterExtensions.WriteVector3Int);
			Writer<Vector3Int?>.write = new Action<NetworkWriter, Vector3Int?>(NetworkWriterExtensions.WriteVector3IntNullable);
			Writer<Color>.write = new Action<NetworkWriter, Color>(NetworkWriterExtensions.WriteColor);
			Writer<Color?>.write = new Action<NetworkWriter, Color?>(NetworkWriterExtensions.WriteColorNullable);
			Writer<Color32>.write = new Action<NetworkWriter, Color32>(NetworkWriterExtensions.WriteColor32);
			Writer<Color32?>.write = new Action<NetworkWriter, Color32?>(NetworkWriterExtensions.WriteColor32Nullable);
			Writer<Quaternion>.write = new Action<NetworkWriter, Quaternion>(NetworkWriterExtensions.WriteQuaternion);
			Writer<Quaternion?>.write = new Action<NetworkWriter, Quaternion?>(NetworkWriterExtensions.WriteQuaternionNullable);
			Writer<Rect>.write = new Action<NetworkWriter, Rect>(NetworkWriterExtensions.WriteRect);
			Writer<Rect?>.write = new Action<NetworkWriter, Rect?>(NetworkWriterExtensions.WriteRectNullable);
			Writer<Plane>.write = new Action<NetworkWriter, Plane>(NetworkWriterExtensions.WritePlane);
			Writer<Plane?>.write = new Action<NetworkWriter, Plane?>(NetworkWriterExtensions.WritePlaneNullable);
			Writer<Ray>.write = new Action<NetworkWriter, Ray>(NetworkWriterExtensions.WriteRay);
			Writer<Ray?>.write = new Action<NetworkWriter, Ray?>(NetworkWriterExtensions.WriteRayNullable);
			Writer<Matrix4x4>.write = new Action<NetworkWriter, Matrix4x4>(NetworkWriterExtensions.WriteMatrix4x4);
			Writer<Matrix4x4?>.write = new Action<NetworkWriter, Matrix4x4?>(NetworkWriterExtensions.WriteMatrix4x4Nullable);
			Writer<Guid>.write = new Action<NetworkWriter, Guid>(NetworkWriterExtensions.WriteGuid);
			Writer<Guid?>.write = new Action<NetworkWriter, Guid?>(NetworkWriterExtensions.WriteGuidNullable);
			Writer<NetworkIdentity>.write = new Action<NetworkWriter, NetworkIdentity>(NetworkWriterExtensions.WriteNetworkIdentity);
			Writer<NetworkBehaviour>.write = new Action<NetworkWriter, NetworkBehaviour>(NetworkWriterExtensions.WriteNetworkBehaviour);
			Writer<Transform>.write = new Action<NetworkWriter, Transform>(NetworkWriterExtensions.WriteTransform);
			Writer<GameObject>.write = new Action<NetworkWriter, GameObject>(NetworkWriterExtensions.WriteGameObject);
			Writer<Uri>.write = new Action<NetworkWriter, Uri>(NetworkWriterExtensions.WriteUri);
			Writer<Texture2D>.write = new Action<NetworkWriter, Texture2D>(NetworkWriterExtensions.WriteTexture2D);
			Writer<Sprite>.write = new Action<NetworkWriter, Sprite>(NetworkWriterExtensions.WriteSprite);
			Writer<DateTime>.write = new Action<NetworkWriter, DateTime>(NetworkWriterExtensions.WriteDateTime);
			Writer<DateTime?>.write = new Action<NetworkWriter, DateTime?>(NetworkWriterExtensions.WriteDateTimeNullable);
			Writer<TimeSnapshotMessage>.write = new Action<NetworkWriter, TimeSnapshotMessage>(GeneratedNetworkCode._Write_Mirror.TimeSnapshotMessage);
			Writer<ReadyMessage>.write = new Action<NetworkWriter, ReadyMessage>(GeneratedNetworkCode._Write_Mirror.ReadyMessage);
			Writer<NotReadyMessage>.write = new Action<NetworkWriter, NotReadyMessage>(GeneratedNetworkCode._Write_Mirror.NotReadyMessage);
			Writer<AddPlayerMessage>.write = new Action<NetworkWriter, AddPlayerMessage>(GeneratedNetworkCode._Write_Mirror.AddPlayerMessage);
			Writer<SceneMessage>.write = new Action<NetworkWriter, SceneMessage>(GeneratedNetworkCode._Write_Mirror.SceneMessage);
			Writer<SceneOperation>.write = new Action<NetworkWriter, SceneOperation>(GeneratedNetworkCode._Write_Mirror.SceneOperation);
			Writer<CommandMessage>.write = new Action<NetworkWriter, CommandMessage>(GeneratedNetworkCode._Write_Mirror.CommandMessage);
			Writer<RpcMessage>.write = new Action<NetworkWriter, RpcMessage>(GeneratedNetworkCode._Write_Mirror.RpcMessage);
			Writer<SpawnMessage>.write = new Action<NetworkWriter, SpawnMessage>(GeneratedNetworkCode._Write_Mirror.SpawnMessage);
			Writer<ChangeOwnerMessage>.write = new Action<NetworkWriter, ChangeOwnerMessage>(GeneratedNetworkCode._Write_Mirror.ChangeOwnerMessage);
			Writer<ObjectSpawnStartedMessage>.write = new Action<NetworkWriter, ObjectSpawnStartedMessage>(GeneratedNetworkCode._Write_Mirror.ObjectSpawnStartedMessage);
			Writer<ObjectSpawnFinishedMessage>.write = new Action<NetworkWriter, ObjectSpawnFinishedMessage>(GeneratedNetworkCode._Write_Mirror.ObjectSpawnFinishedMessage);
			Writer<ObjectDestroyMessage>.write = new Action<NetworkWriter, ObjectDestroyMessage>(GeneratedNetworkCode._Write_Mirror.ObjectDestroyMessage);
			Writer<ObjectHideMessage>.write = new Action<NetworkWriter, ObjectHideMessage>(GeneratedNetworkCode._Write_Mirror.ObjectHideMessage);
			Writer<EntityStateMessage>.write = new Action<NetworkWriter, EntityStateMessage>(GeneratedNetworkCode._Write_Mirror.EntityStateMessage);
			Writer<NetworkPingMessage>.write = new Action<NetworkWriter, NetworkPingMessage>(GeneratedNetworkCode._Write_Mirror.NetworkPingMessage);
			Writer<NetworkPongMessage>.write = new Action<NetworkWriter, NetworkPongMessage>(GeneratedNetworkCode._Write_Mirror.NetworkPongMessage);
			Writer<DissonanceNetworkMessage>.write = new Action<NetworkWriter, DissonanceNetworkMessage>(DissonanceNetworkMessageExtensions.Serialize);
			Writer<MultiplayerRoomManager.SceneLoaded>.write = new Action<NetworkWriter, MultiplayerRoomManager.SceneLoaded>(GeneratedNetworkCode._Write_MultiplayerRoomManager/SceneLoaded);
			Writer<LobbyPrivacyType>.write = new Action<NetworkWriter, LobbyPrivacyType>(GeneratedNetworkCode._Write_LobbyPrivacyType);
			Writer<AllowedMovesetTypes>.write = new Action<NetworkWriter, AllowedMovesetTypes>(GeneratedNetworkCode._Write_AllowedMovesetTypes);
			Writer<GameTypes>.write = new Action<NetworkWriter, GameTypes>(GeneratedNetworkCode._Write_GameTypes);
			Writer<List<EquipmentType>>.write = new Action<NetworkWriter, List<EquipmentType>>(GeneratedNetworkCode._Write_System.Collections.Generic.List`1<MoveClasses.EquipmentType>);
			Writer<EquipmentType>.write = new Action<NetworkWriter, EquipmentType>(GeneratedNetworkCode._Write_MoveClasses.EquipmentType);
			Writer<DeathReason>.write = new Action<NetworkWriter, DeathReason>(GeneratedNetworkCode._Write_MoveClasses.DeathReason);
			Writer<List<EquippedEquipment>>.write = new Action<NetworkWriter, List<EquippedEquipment>>(GeneratedNetworkCode._Write_System.Collections.Generic.List`1<MoveClasses.EquippedEquipment>);
			Writer<EquippedEquipment>.write = new Action<NetworkWriter, EquippedEquipment>(GeneratedNetworkCode._Write_MoveClasses.EquippedEquipment);
			Writer<CollisionSoundType>.write = new Action<NetworkWriter, CollisionSoundType>(GeneratedNetworkCode._Write_CollisionSoundType);
			Writer<BluntDamageEffect>.write = new Action<NetworkWriter, BluntDamageEffect>(GeneratedNetworkCode._Write_BluntDamageEffect);
			Writer<JointType>.write = new Action<NetworkWriter, JointType>(GeneratedNetworkCode._Write_MoveClasses.JointType);
			Writer<DamageOrigin>.write = new Action<NetworkWriter, DamageOrigin>(GeneratedNetworkCode._Write_DamageOrigin);
			Writer<EnvironmentSoundType>.write = new Action<NetworkWriter, EnvironmentSoundType>(GeneratedNetworkCode._Write_EnvironmentSoundType);
			Writer<NetworkByteMessage>.write = new Action<NetworkWriter, NetworkByteMessage>(GeneratedNetworkCode._Write_Utils.NetworkByteMessage);
			Writer<DefaultMovesetSettings>.write = new Action<NetworkWriter, DefaultMovesetSettings>(GeneratedNetworkCode._Write_Utils.DefaultMovesetSettings);
			Reader<byte>.read = new Func<NetworkReader, byte>(NetworkReaderExtensions.ReadByte);
			Reader<byte?>.read = new Func<NetworkReader, byte?>(NetworkReaderExtensions.ReadByteNullable);
			Reader<sbyte>.read = new Func<NetworkReader, sbyte>(NetworkReaderExtensions.ReadSByte);
			Reader<sbyte?>.read = new Func<NetworkReader, sbyte?>(NetworkReaderExtensions.ReadSByteNullable);
			Reader<char>.read = new Func<NetworkReader, char>(NetworkReaderExtensions.ReadChar);
			Reader<char?>.read = new Func<NetworkReader, char?>(NetworkReaderExtensions.ReadCharNullable);
			Reader<bool>.read = new Func<NetworkReader, bool>(NetworkReaderExtensions.ReadBool);
			Reader<bool?>.read = new Func<NetworkReader, bool?>(NetworkReaderExtensions.ReadBoolNullable);
			Reader<short>.read = new Func<NetworkReader, short>(NetworkReaderExtensions.ReadShort);
			Reader<short?>.read = new Func<NetworkReader, short?>(NetworkReaderExtensions.ReadShortNullable);
			Reader<ushort>.read = new Func<NetworkReader, ushort>(NetworkReaderExtensions.ReadUShort);
			Reader<ushort?>.read = new Func<NetworkReader, ushort?>(NetworkReaderExtensions.ReadUShortNullable);
			Reader<int>.read = new Func<NetworkReader, int>(NetworkReaderExtensions.ReadInt);
			Reader<int?>.read = new Func<NetworkReader, int?>(NetworkReaderExtensions.ReadIntNullable);
			Reader<uint>.read = new Func<NetworkReader, uint>(NetworkReaderExtensions.ReadUInt);
			Reader<uint?>.read = new Func<NetworkReader, uint?>(NetworkReaderExtensions.ReadUIntNullable);
			Reader<long>.read = new Func<NetworkReader, long>(NetworkReaderExtensions.ReadLong);
			Reader<long?>.read = new Func<NetworkReader, long?>(NetworkReaderExtensions.ReadLongNullable);
			Reader<ulong>.read = new Func<NetworkReader, ulong>(NetworkReaderExtensions.ReadULong);
			Reader<ulong?>.read = new Func<NetworkReader, ulong?>(NetworkReaderExtensions.ReadULongNullable);
			Reader<float>.read = new Func<NetworkReader, float>(NetworkReaderExtensions.ReadFloat);
			Reader<float?>.read = new Func<NetworkReader, float?>(NetworkReaderExtensions.ReadFloatNullable);
			Reader<double>.read = new Func<NetworkReader, double>(NetworkReaderExtensions.ReadDouble);
			Reader<double?>.read = new Func<NetworkReader, double?>(NetworkReaderExtensions.ReadDoubleNullable);
			Reader<decimal>.read = new Func<NetworkReader, decimal>(NetworkReaderExtensions.ReadDecimal);
			Reader<decimal?>.read = new Func<NetworkReader, decimal?>(NetworkReaderExtensions.ReadDecimalNullable);
			Reader<string>.read = new Func<NetworkReader, string>(NetworkReaderExtensions.ReadString);
			Reader<byte[]>.read = new Func<NetworkReader, byte[]>(NetworkReaderExtensions.ReadBytesAndSize);
			Reader<ArraySegment<byte>>.read = new Func<NetworkReader, ArraySegment<byte>>(NetworkReaderExtensions.ReadBytesAndSizeSegment);
			Reader<Vector2>.read = new Func<NetworkReader, Vector2>(NetworkReaderExtensions.ReadVector2);
			Reader<Vector2?>.read = new Func<NetworkReader, Vector2?>(NetworkReaderExtensions.ReadVector2Nullable);
			Reader<Vector3>.read = new Func<NetworkReader, Vector3>(NetworkReaderExtensions.ReadVector3);
			Reader<Vector3?>.read = new Func<NetworkReader, Vector3?>(NetworkReaderExtensions.ReadVector3Nullable);
			Reader<Vector4>.read = new Func<NetworkReader, Vector4>(NetworkReaderExtensions.ReadVector4);
			Reader<Vector4?>.read = new Func<NetworkReader, Vector4?>(NetworkReaderExtensions.ReadVector4Nullable);
			Reader<Vector2Int>.read = new Func<NetworkReader, Vector2Int>(NetworkReaderExtensions.ReadVector2Int);
			Reader<Vector2Int?>.read = new Func<NetworkReader, Vector2Int?>(NetworkReaderExtensions.ReadVector2IntNullable);
			Reader<Vector3Int>.read = new Func<NetworkReader, Vector3Int>(NetworkReaderExtensions.ReadVector3Int);
			Reader<Vector3Int?>.read = new Func<NetworkReader, Vector3Int?>(NetworkReaderExtensions.ReadVector3IntNullable);
			Reader<Color>.read = new Func<NetworkReader, Color>(NetworkReaderExtensions.ReadColor);
			Reader<Color?>.read = new Func<NetworkReader, Color?>(NetworkReaderExtensions.ReadColorNullable);
			Reader<Color32>.read = new Func<NetworkReader, Color32>(NetworkReaderExtensions.ReadColor32);
			Reader<Color32?>.read = new Func<NetworkReader, Color32?>(NetworkReaderExtensions.ReadColor32Nullable);
			Reader<Quaternion>.read = new Func<NetworkReader, Quaternion>(NetworkReaderExtensions.ReadQuaternion);
			Reader<Quaternion?>.read = new Func<NetworkReader, Quaternion?>(NetworkReaderExtensions.ReadQuaternionNullable);
			Reader<Rect>.read = new Func<NetworkReader, Rect>(NetworkReaderExtensions.ReadRect);
			Reader<Rect?>.read = new Func<NetworkReader, Rect?>(NetworkReaderExtensions.ReadRectNullable);
			Reader<Plane>.read = new Func<NetworkReader, Plane>(NetworkReaderExtensions.ReadPlane);
			Reader<Plane?>.read = new Func<NetworkReader, Plane?>(NetworkReaderExtensions.ReadPlaneNullable);
			Reader<Ray>.read = new Func<NetworkReader, Ray>(NetworkReaderExtensions.ReadRay);
			Reader<Ray?>.read = new Func<NetworkReader, Ray?>(NetworkReaderExtensions.ReadRayNullable);
			Reader<Matrix4x4>.read = new Func<NetworkReader, Matrix4x4>(NetworkReaderExtensions.ReadMatrix4x4);
			Reader<Matrix4x4?>.read = new Func<NetworkReader, Matrix4x4?>(NetworkReaderExtensions.ReadMatrix4x4Nullable);
			Reader<Guid>.read = new Func<NetworkReader, Guid>(NetworkReaderExtensions.ReadGuid);
			Reader<Guid?>.read = new Func<NetworkReader, Guid?>(NetworkReaderExtensions.ReadGuidNullable);
			Reader<NetworkIdentity>.read = new Func<NetworkReader, NetworkIdentity>(NetworkReaderExtensions.ReadNetworkIdentity);
			Reader<NetworkBehaviour>.read = new Func<NetworkReader, NetworkBehaviour>(NetworkReaderExtensions.ReadNetworkBehaviour);
			Reader<NetworkBehaviourSyncVar>.read = new Func<NetworkReader, NetworkBehaviourSyncVar>(NetworkReaderExtensions.ReadNetworkBehaviourSyncVar);
			Reader<Transform>.read = new Func<NetworkReader, Transform>(NetworkReaderExtensions.ReadTransform);
			Reader<GameObject>.read = new Func<NetworkReader, GameObject>(NetworkReaderExtensions.ReadGameObject);
			Reader<Uri>.read = new Func<NetworkReader, Uri>(NetworkReaderExtensions.ReadUri);
			Reader<Texture2D>.read = new Func<NetworkReader, Texture2D>(NetworkReaderExtensions.ReadTexture2D);
			Reader<Sprite>.read = new Func<NetworkReader, Sprite>(NetworkReaderExtensions.ReadSprite);
			Reader<DateTime>.read = new Func<NetworkReader, DateTime>(NetworkReaderExtensions.ReadDateTime);
			Reader<DateTime?>.read = new Func<NetworkReader, DateTime?>(NetworkReaderExtensions.ReadDateTimeNullable);
			Reader<TimeSnapshotMessage>.read = new Func<NetworkReader, TimeSnapshotMessage>(GeneratedNetworkCode._Read_Mirror.TimeSnapshotMessage);
			Reader<ReadyMessage>.read = new Func<NetworkReader, ReadyMessage>(GeneratedNetworkCode._Read_Mirror.ReadyMessage);
			Reader<NotReadyMessage>.read = new Func<NetworkReader, NotReadyMessage>(GeneratedNetworkCode._Read_Mirror.NotReadyMessage);
			Reader<AddPlayerMessage>.read = new Func<NetworkReader, AddPlayerMessage>(GeneratedNetworkCode._Read_Mirror.AddPlayerMessage);
			Reader<SceneMessage>.read = new Func<NetworkReader, SceneMessage>(GeneratedNetworkCode._Read_Mirror.SceneMessage);
			Reader<SceneOperation>.read = new Func<NetworkReader, SceneOperation>(GeneratedNetworkCode._Read_Mirror.SceneOperation);
			Reader<CommandMessage>.read = new Func<NetworkReader, CommandMessage>(GeneratedNetworkCode._Read_Mirror.CommandMessage);
			Reader<RpcMessage>.read = new Func<NetworkReader, RpcMessage>(GeneratedNetworkCode._Read_Mirror.RpcMessage);
			Reader<SpawnMessage>.read = new Func<NetworkReader, SpawnMessage>(GeneratedNetworkCode._Read_Mirror.SpawnMessage);
			Reader<ChangeOwnerMessage>.read = new Func<NetworkReader, ChangeOwnerMessage>(GeneratedNetworkCode._Read_Mirror.ChangeOwnerMessage);
			Reader<ObjectSpawnStartedMessage>.read = new Func<NetworkReader, ObjectSpawnStartedMessage>(GeneratedNetworkCode._Read_Mirror.ObjectSpawnStartedMessage);
			Reader<ObjectSpawnFinishedMessage>.read = new Func<NetworkReader, ObjectSpawnFinishedMessage>(GeneratedNetworkCode._Read_Mirror.ObjectSpawnFinishedMessage);
			Reader<ObjectDestroyMessage>.read = new Func<NetworkReader, ObjectDestroyMessage>(GeneratedNetworkCode._Read_Mirror.ObjectDestroyMessage);
			Reader<ObjectHideMessage>.read = new Func<NetworkReader, ObjectHideMessage>(GeneratedNetworkCode._Read_Mirror.ObjectHideMessage);
			Reader<EntityStateMessage>.read = new Func<NetworkReader, EntityStateMessage>(GeneratedNetworkCode._Read_Mirror.EntityStateMessage);
			Reader<NetworkPingMessage>.read = new Func<NetworkReader, NetworkPingMessage>(GeneratedNetworkCode._Read_Mirror.NetworkPingMessage);
			Reader<NetworkPongMessage>.read = new Func<NetworkReader, NetworkPongMessage>(GeneratedNetworkCode._Read_Mirror.NetworkPongMessage);
			Reader<DissonanceNetworkMessage>.read = new Func<NetworkReader, DissonanceNetworkMessage>(DissonanceNetworkMessageExtensions.Deserialize);
			Reader<MultiplayerRoomManager.SceneLoaded>.read = new Func<NetworkReader, MultiplayerRoomManager.SceneLoaded>(GeneratedNetworkCode._Read_MultiplayerRoomManager/SceneLoaded);
			Reader<LobbyPrivacyType>.read = new Func<NetworkReader, LobbyPrivacyType>(GeneratedNetworkCode._Read_LobbyPrivacyType);
			Reader<AllowedMovesetTypes>.read = new Func<NetworkReader, AllowedMovesetTypes>(GeneratedNetworkCode._Read_AllowedMovesetTypes);
			Reader<GameTypes>.read = new Func<NetworkReader, GameTypes>(GeneratedNetworkCode._Read_GameTypes);
			Reader<List<EquipmentType>>.read = new Func<NetworkReader, List<EquipmentType>>(GeneratedNetworkCode._Read_System.Collections.Generic.List`1<MoveClasses.EquipmentType>);
			Reader<EquipmentType>.read = new Func<NetworkReader, EquipmentType>(GeneratedNetworkCode._Read_MoveClasses.EquipmentType);
			Reader<DeathReason>.read = new Func<NetworkReader, DeathReason>(GeneratedNetworkCode._Read_MoveClasses.DeathReason);
			Reader<List<EquippedEquipment>>.read = new Func<NetworkReader, List<EquippedEquipment>>(GeneratedNetworkCode._Read_System.Collections.Generic.List`1<MoveClasses.EquippedEquipment>);
			Reader<EquippedEquipment>.read = new Func<NetworkReader, EquippedEquipment>(GeneratedNetworkCode._Read_MoveClasses.EquippedEquipment);
			Reader<CollisionSoundType>.read = new Func<NetworkReader, CollisionSoundType>(GeneratedNetworkCode._Read_CollisionSoundType);
			Reader<BluntDamageEffect>.read = new Func<NetworkReader, BluntDamageEffect>(GeneratedNetworkCode._Read_BluntDamageEffect);
			Reader<JointType>.read = new Func<NetworkReader, JointType>(GeneratedNetworkCode._Read_MoveClasses.JointType);
			Reader<DamageOrigin>.read = new Func<NetworkReader, DamageOrigin>(GeneratedNetworkCode._Read_DamageOrigin);
			Reader<EnvironmentSoundType>.read = new Func<NetworkReader, EnvironmentSoundType>(GeneratedNetworkCode._Read_EnvironmentSoundType);
			Reader<NetworkByteMessage>.read = new Func<NetworkReader, NetworkByteMessage>(GeneratedNetworkCode._Read_Utils.NetworkByteMessage);
			Reader<DefaultMovesetSettings>.read = new Func<NetworkReader, DefaultMovesetSettings>(GeneratedNetworkCode._Read_Utils.DefaultMovesetSettings);
		}
	}
}
