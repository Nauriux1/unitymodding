using System;
using System.Runtime.InteropServices;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror.Experimental
{
	// Token: 0x020002BF RID: 703
	[AddComponentMenu("Network/Experimental/NetworkPlayerPart")]
	[HelpURL("https://mirror-networking.com/docs/Components/NetworkRigidbody.html")]
	public class NetworkPlayerPart : NetworkBehaviour
	{
		// Token: 0x0600150F RID: 5391 RVA: 0x0000777A File Offset: 0x0000597A
		private new void OnValidate()
		{
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06001510 RID: 5392 RVA: 0x00069779 File Offset: 0x00067979
		private bool IgnoreSync
		{
			get
			{
				return base.isServer || this.ClientWithAuthority;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001511 RID: 5393 RVA: 0x0006978B File Offset: 0x0006798B
		private bool ClientWithAuthority
		{
			get
			{
				return this.clientAuthority && base.hasAuthority;
			}
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0006979D File Offset: 0x0006799D
		private void OnTargetJoint1RotationChanged(Quaternion _, Quaternion newValue)
		{
			if (this.IgnoreSync)
			{
				return;
			}
			this.targetJoint1.targetRotation = newValue;
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x000697B4 File Offset: 0x000679B4
		private void OnTargetJoint2RotationChanged(Quaternion _, Quaternion newValue)
		{
			if (this.IgnoreSync)
			{
				return;
			}
			this.targetJoint2.targetRotation = newValue;
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x000697CB File Offset: 0x000679CB
		private void OnVelocityChanged(Vector3 _, Vector3 newValue)
		{
			if (this.IgnoreSync)
			{
				return;
			}
			if (this.targetRigidbody != null)
			{
				this.targetRigidbody.velocity = newValue;
			}
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x000697F0 File Offset: 0x000679F0
		private void OnAngularVelocityChanged(Vector3 _, Vector3 newValue)
		{
			if (this.IgnoreSync)
			{
				return;
			}
			if (this.targetRigidbody != null)
			{
				this.targetRigidbody.angularVelocity = newValue;
			}
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x00069815 File Offset: 0x00067A15
		private void OnIsKinematicChanged(bool _, bool newValue)
		{
			if (this.IgnoreSync)
			{
				return;
			}
			if (this.targetRigidbody != null)
			{
				this.targetRigidbody.isKinematic = newValue;
			}
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0006983A File Offset: 0x00067A3A
		private void OnUseGravityChanged(bool _, bool newValue)
		{
			if (this.IgnoreSync)
			{
				return;
			}
			if (this.targetRigidbody != null)
			{
				this.targetRigidbody.useGravity = newValue;
			}
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x0006985F File Offset: 0x00067A5F
		private void OnuDragChanged(float _, float newValue)
		{
			if (this.IgnoreSync)
			{
				return;
			}
			if (this.targetRigidbody != null)
			{
				this.targetRigidbody.drag = newValue;
			}
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x00069884 File Offset: 0x00067A84
		private void OnAngularDragChanged(float _, float newValue)
		{
			if (this.IgnoreSync)
			{
				return;
			}
			if (this.targetRigidbody != null)
			{
				this.targetRigidbody.angularDrag = newValue;
			}
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x000698A9 File Offset: 0x00067AA9
		internal void Update()
		{
			if (base.isServer)
			{
				this.SyncToClients();
				return;
			}
			if (this.ClientWithAuthority)
			{
				this.SendToServer();
			}
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x000698C8 File Offset: 0x00067AC8
		internal void FixedUpdate()
		{
			if (this.targetRigidbody != null && this.clearAngularVelocity && !this.syncAngularVelocity)
			{
				this.targetRigidbody.angularVelocity = Vector3.zero;
			}
			if (this.targetRigidbody != null && this.clearVelocity && !this.syncVelocity)
			{
				this.targetRigidbody.velocity = Vector3.zero;
			}
			if (base.isServer && this.HasEitherMovedRotatedScaled())
			{
				this.RpcMove(this.targetTransform.localPosition, this.targetTransform.localRotation, this.targetTransform.localScale);
			}
			if (base.isClient)
			{
				if (this.IsOwnerWithClientAuthority)
				{
					if (!base.isServer && this.HasEitherMovedRotatedScaled())
					{
						this.CmdClientToServerSync(this.targetTransform.localPosition, this.targetTransform.localRotation, this.targetTransform.localScale);
						return;
					}
				}
				else if (this.goal.isValid)
				{
					if (this.NeedsTeleport())
					{
						this.ApplyPositionRotationScale(this.goal.localPosition, this.goal.localRotation, this.goal.localScale);
						this.start = default(NetworkPlayerPart.DataPoint);
						this.goal = default(NetworkPlayerPart.DataPoint);
						return;
					}
					this.ApplyPositionRotationScale(this.InterpolatePosition(this.start, this.goal, this.targetTransform.localPosition), this.InterpolateRotation(this.start, this.goal, this.targetTransform.localRotation), this.InterpolateScale(this.start, this.goal, this.targetTransform.localScale));
				}
			}
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x00069A70 File Offset: 0x00067C70
		[Server]
		private void SyncToClients()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void Mirror.Experimental.NetworkPlayerPart::SyncToClients()' called when server was not active");
				return;
			}
			if (this.targetJoint1 != null)
			{
				Quaternion networktargetJoint1Rotation = this.syncTargetRotation ? this.targetJoint1.targetRotation : default(Quaternion);
				if (this.syncTargetRotation)
				{
					this.NetworktargetJoint1Rotation = networktargetJoint1Rotation;
					this.previousValue.targetJoint1Rotation = networktargetJoint1Rotation;
				}
			}
			if (this.targetJoint2 != null)
			{
				Quaternion networktargetJoint2Rotation = this.syncTargetRotation ? this.targetJoint2.targetRotation : default(Quaternion);
				if (this.syncTargetRotation)
				{
					this.NetworktargetJoint2Rotation = networktargetJoint2Rotation;
					this.previousValue.targetJoint2Rotation = networktargetJoint2Rotation;
				}
			}
			if (this.targetRigidbody != null)
			{
				Vector3 vector = this.syncVelocity ? this.targetRigidbody.velocity : default(Vector3);
				Vector3 vector2 = this.syncAngularVelocity ? this.targetRigidbody.angularVelocity : default(Vector3);
				bool flag = this.syncVelocity && (this.previousValue.velocity - vector).sqrMagnitude > this.velocitySensitivity * this.velocitySensitivity;
				bool flag2 = this.syncAngularVelocity && (this.previousValue.angularVelocity - vector2).sqrMagnitude > this.angularVelocitySensitivity * this.angularVelocitySensitivity;
				if (flag)
				{
					this.Networkvelocity = vector;
					this.previousValue.velocity = vector;
				}
				if (flag2)
				{
					this.NetworkangularVelocity = vector2;
					this.previousValue.angularVelocity = vector2;
				}
				this.NetworkisKinematic = this.targetRigidbody.isKinematic;
				this.NetworkuseGravity = this.targetRigidbody.useGravity;
				this.Networkdrag = this.targetRigidbody.drag;
				this.NetworkangularDrag = this.targetRigidbody.angularDrag;
			}
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x00069C51 File Offset: 0x00067E51
		[Client]
		private void SendToServer()
		{
			if (!NetworkClient.active)
			{
				Debug.LogWarning("[Client] function 'System.Void Mirror.Experimental.NetworkPlayerPart::SendToServer()' called when client was not active");
				return;
			}
			if (!base.hasAuthority)
			{
				return;
			}
			this.SendTargetRotation();
			this.SendVelocity();
			this.SendRigidBodySettings();
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x00069C84 File Offset: 0x00067E84
		[Client]
		private void SendVelocity()
		{
			if (!NetworkClient.active)
			{
				Debug.LogWarning("[Client] function 'System.Void Mirror.Experimental.NetworkPlayerPart::SendVelocity()' called when client was not active");
				return;
			}
			if (this.targetRigidbody == null)
			{
				return;
			}
			float time = Time.time;
			if (time < this.previousValue.nextSyncTime)
			{
				return;
			}
			Vector3 b = this.syncVelocity ? this.targetRigidbody.velocity : default(Vector3);
			Vector3 b2 = this.syncAngularVelocity ? this.targetRigidbody.angularVelocity : default(Vector3);
			bool flag = this.syncVelocity && (this.previousValue.velocity - b).sqrMagnitude > this.velocitySensitivity * this.velocitySensitivity;
			bool flag2 = this.syncAngularVelocity && (this.previousValue.angularVelocity - b2).sqrMagnitude > this.angularVelocitySensitivity * this.angularVelocitySensitivity;
			if (flag2)
			{
				this.CmdSendVelocityAndAngular(b, b2);
				this.previousValue.velocity = b;
				this.previousValue.angularVelocity = b2;
			}
			else if (flag)
			{
				this.CmdSendVelocity(b);
				this.previousValue.velocity = b;
			}
			if (flag2 || flag)
			{
				this.previousValue.nextSyncTime = time + this.syncInterval;
			}
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x00069DC8 File Offset: 0x00067FC8
		[Client]
		private void SendRigidBodySettings()
		{
			if (!NetworkClient.active)
			{
				Debug.LogWarning("[Client] function 'System.Void Mirror.Experimental.NetworkPlayerPart::SendRigidBodySettings()' called when client was not active");
				return;
			}
			if (this.targetRigidbody == null)
			{
				return;
			}
			if (this.previousValue.isKinematic != this.targetRigidbody.isKinematic)
			{
				this.CmdSendIsKinematic(this.targetRigidbody.isKinematic);
				this.previousValue.isKinematic = this.targetRigidbody.isKinematic;
			}
			if (this.previousValue.useGravity != this.targetRigidbody.useGravity)
			{
				this.CmdSendUseGravity(this.targetRigidbody.useGravity);
				this.previousValue.useGravity = this.targetRigidbody.useGravity;
			}
			if (this.previousValue.drag != this.targetRigidbody.drag)
			{
				this.CmdSendDrag(this.targetRigidbody.drag);
				this.previousValue.drag = this.targetRigidbody.drag;
			}
			if (this.previousValue.angularDrag != this.targetRigidbody.angularDrag)
			{
				this.CmdSendAngularDrag(this.targetRigidbody.angularDrag);
				this.previousValue.angularDrag = this.targetRigidbody.angularDrag;
			}
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x00069EF8 File Offset: 0x000680F8
		[Command]
		private void CmdSendVelocity(Vector3 velocity)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(velocity);
			base.SendCommandInternal("System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendVelocity(UnityEngine.Vector3)", -827261472, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x00069F34 File Offset: 0x00068134
		[Command]
		private void CmdSendVelocityAndAngular(Vector3 velocity, Vector3 angularVelocity)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(velocity);
			writer.WriteVector3(angularVelocity);
			base.SendCommandInternal("System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendVelocityAndAngular(UnityEngine.Vector3,UnityEngine.Vector3)", -1581804120, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x00069F78 File Offset: 0x00068178
		[Command]
		private void CmdSendIsKinematic(bool isKinematic)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(isKinematic);
			base.SendCommandInternal("System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendIsKinematic(System.Boolean)", 1794684760, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x00069FB4 File Offset: 0x000681B4
		[Command]
		private void CmdSendUseGravity(bool useGravity)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(useGravity);
			base.SendCommandInternal("System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendUseGravity(System.Boolean)", -1487205830, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x00069FF0 File Offset: 0x000681F0
		[Command]
		private void CmdSendDrag(float drag)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(drag);
			base.SendCommandInternal("System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendDrag(System.Single)", -191401413, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x0006A02C File Offset: 0x0006822C
		[Command]
		private void CmdSendAngularDrag(float angularDrag)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(angularDrag);
			base.SendCommandInternal("System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendAngularDrag(System.Single)", -1198716401, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x0006A068 File Offset: 0x00068268
		[Client]
		private void SendTargetRotation()
		{
			if (!NetworkClient.active)
			{
				Debug.LogWarning("[Client] function 'System.Void Mirror.Experimental.NetworkPlayerPart::SendTargetRotation()' called when client was not active");
				return;
			}
			float time = Time.time;
			if (time < this.previousValue.nextSyncTime)
			{
				return;
			}
			if (this.targetJoint1 != null)
			{
				Quaternion targetRotation = this.syncTargetRotation ? this.targetJoint1.targetRotation : default(Quaternion);
				bool flag = this.syncTargetRotation;
				if (flag)
				{
					this.CmdSendTargetRotation(targetRotation);
					this.previousValue.targetJoint1Rotation = targetRotation;
				}
				if (flag)
				{
					this.previousValue.nextSyncTime = time + this.syncInterval;
				}
			}
			if (this.targetJoint2 != null)
			{
				Quaternion targetRotation2 = this.syncTargetRotation ? this.targetJoint2.targetRotation : default(Quaternion);
				bool flag2 = this.syncTargetRotation;
				if (flag2)
				{
					this.CmdSendTargetRotation2(targetRotation2);
					this.previousValue.targetJoint2Rotation = targetRotation2;
				}
				if (flag2)
				{
					this.previousValue.nextSyncTime = time + this.syncInterval;
				}
			}
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x0006A15C File Offset: 0x0006835C
		[Command]
		private void CmdSendTargetRotation(Quaternion targetRotation)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteQuaternion(targetRotation);
			base.SendCommandInternal("System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendTargetRotation(UnityEngine.Quaternion)", 992277878, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x0006A198 File Offset: 0x00068398
		[Command]
		private void CmdSendTargetRotation2(Quaternion targetRotation)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteQuaternion(targetRotation);
			base.SendCommandInternal("System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendTargetRotation2(UnityEngine.Quaternion)", 123115028, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x0006A1D2 File Offset: 0x000683D2
		private bool IsOwnerWithClientAuthority
		{
			get
			{
				return base.hasAuthority && this.clientAuthority;
			}
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x0006A1E4 File Offset: 0x000683E4
		private bool HasEitherMovedRotatedScaled()
		{
			bool flag = this.HasMoved || this.HasRotated || this.HasScaled;
			if (flag)
			{
				if (this.syncPosition)
				{
					this.lastPosition = this.targetTransform.localPosition;
				}
				if (this.syncRotation)
				{
					this.lastRotation = this.targetTransform.localRotation;
				}
				if (this.syncScale)
				{
					this.lastScale = this.targetTransform.localScale;
				}
			}
			return flag;
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600152B RID: 5419 RVA: 0x0006A258 File Offset: 0x00068458
		private bool HasMoved
		{
			get
			{
				try
				{
					return this.syncPosition && Vector3.SqrMagnitude(this.lastPosition - this.targetTransform.localPosition) > this.localPositionSensitivity * this.localPositionSensitivity;
				}
				catch (Exception message)
				{
					Debug.Log(message);
				}
				return false;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x0006A2B8 File Offset: 0x000684B8
		private bool HasRotated
		{
			get
			{
				return this.syncRotation && Quaternion.Angle(this.lastRotation, this.targetTransform.localRotation) > this.localRotationSensitivity;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x0600152D RID: 5421 RVA: 0x0006A2E2 File Offset: 0x000684E2
		private bool HasScaled
		{
			get
			{
				return this.syncScale && Vector3.SqrMagnitude(this.lastScale - this.targetTransform.localScale) > this.localScaleSensitivity * this.localScaleSensitivity;
			}
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0006A318 File Offset: 0x00068518
		private bool NeedsTeleport()
		{
			float num = this.start.isValid ? this.start.timeStamp : (Time.time - Time.fixedDeltaTime);
			float num2 = this.goal.isValid ? this.goal.timeStamp : Time.time;
			float num3 = num2 - num;
			return Time.time - num2 > num3 * 5f;
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x0006A380 File Offset: 0x00068580
		[Command]
		private void CmdClientToServerSync(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			writer.WriteVector3(scale);
			base.SendCommandInternal("System.Void Mirror.Experimental.NetworkPlayerPart::CmdClientToServerSync(UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Vector3)", -1359207677, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x0006A3D0 File Offset: 0x000685D0
		[ClientRpc]
		private void RpcMove(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			writer.WriteVector3(scale);
			this.SendRPCInternal("System.Void Mirror.Experimental.NetworkPlayerPart::RpcMove(UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Vector3)", 1836537467, writer, 0, true);
			NetworkWriterPool.Return(writer);
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x0006A420 File Offset: 0x00068620
		private void SetGoal(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			NetworkPlayerPart.DataPoint dataPoint = new NetworkPlayerPart.DataPoint
			{
				localPosition = position,
				localRotation = rotation,
				localScale = scale,
				timeStamp = Time.time
			};
			dataPoint.movementSpeed = NetworkPlayerPart.EstimateMovementSpeed(this.goal, dataPoint, this.targetTransform, Time.fixedDeltaTime);
			if (this.start.timeStamp == 0f)
			{
				this.start = new NetworkPlayerPart.DataPoint
				{
					timeStamp = Time.time - Time.fixedDeltaTime,
					localPosition = this.targetTransform.localPosition,
					localRotation = this.targetTransform.localRotation,
					localScale = this.targetTransform.localScale,
					movementSpeed = dataPoint.movementSpeed
				};
			}
			else
			{
				float num = Vector3.Distance(this.start.localPosition, this.goal.localPosition);
				float num2 = Vector3.Distance(this.goal.localPosition, dataPoint.localPosition);
				this.start = this.goal;
				if (Vector3.Distance(this.targetTransform.localPosition, this.start.localPosition) < num + num2)
				{
					this.start.localPosition = this.targetTransform.localPosition;
					this.start.localRotation = this.targetTransform.localRotation;
					this.start.localScale = this.targetTransform.localScale;
				}
			}
			this.goal = dataPoint;
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x0006A59C File Offset: 0x0006879C
		private static float EstimateMovementSpeed(NetworkPlayerPart.DataPoint from, NetworkPlayerPart.DataPoint to, Transform transform, float sendInterval)
		{
			Vector3 vector = to.localPosition - ((from.localPosition != transform.localPosition) ? from.localPosition : transform.localPosition);
			float num = from.isValid ? (to.timeStamp - from.timeStamp) : sendInterval;
			if (num <= 0f)
			{
				return 0f;
			}
			return vector.magnitude / num;
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x0006A607 File Offset: 0x00068807
		private void ApplyPositionRotationScale(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			if (this.syncPosition)
			{
				this.targetTransform.localPosition = position;
			}
			if (this.syncRotation)
			{
				this.targetTransform.localRotation = rotation;
			}
			if (this.syncScale)
			{
				this.targetTransform.localScale = scale;
			}
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x0006A648 File Offset: 0x00068848
		private Vector3 InterpolatePosition(NetworkPlayerPart.DataPoint start, NetworkPlayerPart.DataPoint goal, Vector3 currentPosition)
		{
			if (!this.interpolatePosition)
			{
				return currentPosition;
			}
			if (start.movementSpeed != 0f)
			{
				float num = Mathf.Max(start.movementSpeed, goal.movementSpeed);
				return Vector3.MoveTowards(currentPosition, goal.localPosition, num * Time.deltaTime);
			}
			return currentPosition;
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x0006A694 File Offset: 0x00068894
		private Quaternion InterpolateRotation(NetworkPlayerPart.DataPoint start, NetworkPlayerPart.DataPoint goal, Quaternion defaultRotation)
		{
			if (!this.interpolateRotation)
			{
				return defaultRotation;
			}
			if (start.localRotation != goal.localRotation)
			{
				float t = NetworkPlayerPart.CurrentInterpolationFactor(start, goal);
				return Quaternion.Slerp(start.localRotation, goal.localRotation, t);
			}
			return defaultRotation;
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x0006A6DC File Offset: 0x000688DC
		private Vector3 InterpolateScale(NetworkPlayerPart.DataPoint start, NetworkPlayerPart.DataPoint goal, Vector3 currentScale)
		{
			if (!this.interpolateScale)
			{
				return currentScale;
			}
			if (start.localScale != goal.localScale)
			{
				float t = NetworkPlayerPart.CurrentInterpolationFactor(start, goal);
				return Vector3.Lerp(start.localScale, goal.localScale, t);
			}
			return currentScale;
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x0006A724 File Offset: 0x00068924
		private static float CurrentInterpolationFactor(NetworkPlayerPart.DataPoint start, NetworkPlayerPart.DataPoint goal)
		{
			if (!start.isValid)
			{
				return 1f;
			}
			float num = goal.timeStamp - start.timeStamp;
			float num2 = Time.time - goal.timeStamp;
			if (num <= 0f)
			{
				return 1f;
			}
			return num2 / num;
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
		public override bool Weaved()
		{
			return true;
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x0600153A RID: 5434 RVA: 0x0006A814 File Offset: 0x00068A14
		// (set) Token: 0x0600153B RID: 5435 RVA: 0x0006A827 File Offset: 0x00068A27
		public Quaternion NetworktargetJoint1Rotation
		{
			get
			{
				return this.targetJoint1Rotation;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<Quaternion>(value, ref this.targetJoint1Rotation, 1UL, new Action<Quaternion, Quaternion>(this.OnTargetJoint1RotationChanged));
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x0600153C RID: 5436 RVA: 0x0006A84C File Offset: 0x00068A4C
		// (set) Token: 0x0600153D RID: 5437 RVA: 0x0006A85F File Offset: 0x00068A5F
		public Quaternion NetworktargetJoint2Rotation
		{
			get
			{
				return this.targetJoint2Rotation;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<Quaternion>(value, ref this.targetJoint2Rotation, 2UL, new Action<Quaternion, Quaternion>(this.OnTargetJoint2RotationChanged));
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x0600153E RID: 5438 RVA: 0x0006A884 File Offset: 0x00068A84
		// (set) Token: 0x0600153F RID: 5439 RVA: 0x0006A897 File Offset: 0x00068A97
		public Vector3 Networkvelocity
		{
			get
			{
				return this.velocity;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<Vector3>(value, ref this.velocity, 4UL, new Action<Vector3, Vector3>(this.OnVelocityChanged));
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x0006A8BC File Offset: 0x00068ABC
		// (set) Token: 0x06001541 RID: 5441 RVA: 0x0006A8CF File Offset: 0x00068ACF
		public Vector3 NetworkangularVelocity
		{
			get
			{
				return this.angularVelocity;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<Vector3>(value, ref this.angularVelocity, 8UL, new Action<Vector3, Vector3>(this.OnAngularVelocityChanged));
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06001542 RID: 5442 RVA: 0x0006A8F4 File Offset: 0x00068AF4
		// (set) Token: 0x06001543 RID: 5443 RVA: 0x0006A907 File Offset: 0x00068B07
		public bool NetworkisKinematic
		{
			get
			{
				return this.isKinematic;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<bool>(value, ref this.isKinematic, 16UL, new Action<bool, bool>(this.OnIsKinematicChanged));
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06001544 RID: 5444 RVA: 0x0006A92C File Offset: 0x00068B2C
		// (set) Token: 0x06001545 RID: 5445 RVA: 0x0006A93F File Offset: 0x00068B3F
		public bool NetworkuseGravity
		{
			get
			{
				return this.useGravity;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<bool>(value, ref this.useGravity, 32UL, new Action<bool, bool>(this.OnUseGravityChanged));
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06001546 RID: 5446 RVA: 0x0006A964 File Offset: 0x00068B64
		// (set) Token: 0x06001547 RID: 5447 RVA: 0x0006A977 File Offset: 0x00068B77
		public float Networkdrag
		{
			get
			{
				return this.drag;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<float>(value, ref this.drag, 64UL, new Action<float, float>(this.OnuDragChanged));
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06001548 RID: 5448 RVA: 0x0006A99C File Offset: 0x00068B9C
		// (set) Token: 0x06001549 RID: 5449 RVA: 0x0006A9AF File Offset: 0x00068BAF
		public float NetworkangularDrag
		{
			get
			{
				return this.angularDrag;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<float>(value, ref this.angularDrag, 128UL, new Action<float, float>(this.OnAngularDragChanged));
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600154A RID: 5450 RVA: 0x0006A9D4 File Offset: 0x00068BD4
		// (set) Token: 0x0600154B RID: 5451 RVA: 0x0006A9E7 File Offset: 0x00068BE7
		public bool NetworkexcludeOwnerUpdate
		{
			get
			{
				return this.excludeOwnerUpdate;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<bool>(value, ref this.excludeOwnerUpdate, 256UL, null);
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x0006AA04 File Offset: 0x00068C04
		// (set) Token: 0x0600154D RID: 5453 RVA: 0x0006AA17 File Offset: 0x00068C17
		public bool NetworksyncPosition
		{
			get
			{
				return this.syncPosition;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<bool>(value, ref this.syncPosition, 512UL, null);
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x0006AA34 File Offset: 0x00068C34
		// (set) Token: 0x0600154F RID: 5455 RVA: 0x0006AA47 File Offset: 0x00068C47
		public bool NetworksyncRotation
		{
			get
			{
				return this.syncRotation;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<bool>(value, ref this.syncRotation, 1024UL, null);
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06001550 RID: 5456 RVA: 0x0006AA64 File Offset: 0x00068C64
		// (set) Token: 0x06001551 RID: 5457 RVA: 0x0006AA77 File Offset: 0x00068C77
		public bool NetworksyncScale
		{
			get
			{
				return this.syncScale;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<bool>(value, ref this.syncScale, 2048UL, null);
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06001552 RID: 5458 RVA: 0x0006AA94 File Offset: 0x00068C94
		// (set) Token: 0x06001553 RID: 5459 RVA: 0x0006AAA7 File Offset: 0x00068CA7
		public bool NetworkinterpolatePosition
		{
			get
			{
				return this.interpolatePosition;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<bool>(value, ref this.interpolatePosition, 4096UL, null);
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x0006AAC4 File Offset: 0x00068CC4
		// (set) Token: 0x06001555 RID: 5461 RVA: 0x0006AAD7 File Offset: 0x00068CD7
		public bool NetworkinterpolateRotation
		{
			get
			{
				return this.interpolateRotation;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<bool>(value, ref this.interpolateRotation, 8192UL, null);
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06001556 RID: 5462 RVA: 0x0006AAF4 File Offset: 0x00068CF4
		// (set) Token: 0x06001557 RID: 5463 RVA: 0x0006AB07 File Offset: 0x00068D07
		public bool NetworkinterpolateScale
		{
			get
			{
				return this.interpolateScale;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<bool>(value, ref this.interpolateScale, 16384UL, null);
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06001558 RID: 5464 RVA: 0x0006AB24 File Offset: 0x00068D24
		// (set) Token: 0x06001559 RID: 5465 RVA: 0x0006AB37 File Offset: 0x00068D37
		public float NetworklocalPositionSensitivity
		{
			get
			{
				return this.localPositionSensitivity;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<float>(value, ref this.localPositionSensitivity, 32768UL, null);
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600155A RID: 5466 RVA: 0x0006AB54 File Offset: 0x00068D54
		// (set) Token: 0x0600155B RID: 5467 RVA: 0x0006AB67 File Offset: 0x00068D67
		public float NetworklocalRotationSensitivity
		{
			get
			{
				return this.localRotationSensitivity;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<float>(value, ref this.localRotationSensitivity, 65536UL, null);
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x0006AB84 File Offset: 0x00068D84
		// (set) Token: 0x0600155D RID: 5469 RVA: 0x0006AB97 File Offset: 0x00068D97
		public float NetworklocalScaleSensitivity
		{
			get
			{
				return this.localScaleSensitivity;
			}
			[param: In]
			set
			{
				base.GeneratedSyncVarSetter<float>(value, ref this.localScaleSensitivity, 131072UL, null);
			}
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x0006ABB1 File Offset: 0x00068DB1
		protected void UserCode_CmdSendVelocity__Vector3(Vector3 velocity)
		{
			if (this.targetRigidbody == null)
			{
				return;
			}
			if (!this.clientAuthority)
			{
				return;
			}
			this.Networkvelocity = velocity;
			this.targetRigidbody.velocity = velocity;
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x0006ABDE File Offset: 0x00068DDE
		protected static void InvokeUserCode_CmdSendVelocity__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSendVelocity called on client.");
				return;
			}
			((NetworkPlayerPart)obj).UserCode_CmdSendVelocity__Vector3(reader.ReadVector3());
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0006AC08 File Offset: 0x00068E08
		protected void UserCode_CmdSendVelocityAndAngular__Vector3__Vector3(Vector3 velocity, Vector3 angularVelocity)
		{
			if (this.targetRigidbody == null)
			{
				return;
			}
			if (!this.clientAuthority)
			{
				return;
			}
			if (this.syncVelocity)
			{
				this.Networkvelocity = velocity;
				this.targetRigidbody.velocity = velocity;
			}
			this.NetworkangularVelocity = angularVelocity;
			this.targetRigidbody.angularVelocity = angularVelocity;
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x0006AC5B File Offset: 0x00068E5B
		protected static void InvokeUserCode_CmdSendVelocityAndAngular__Vector3__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSendVelocityAndAngular called on client.");
				return;
			}
			((NetworkPlayerPart)obj).UserCode_CmdSendVelocityAndAngular__Vector3__Vector3(reader.ReadVector3(), reader.ReadVector3());
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x0006AC8A File Offset: 0x00068E8A
		protected void UserCode_CmdSendIsKinematic__Boolean(bool isKinematic)
		{
			if (this.targetRigidbody == null)
			{
				return;
			}
			if (!this.clientAuthority)
			{
				return;
			}
			this.NetworkisKinematic = isKinematic;
			this.targetRigidbody.isKinematic = isKinematic;
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x0006ACB7 File Offset: 0x00068EB7
		protected static void InvokeUserCode_CmdSendIsKinematic__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSendIsKinematic called on client.");
				return;
			}
			((NetworkPlayerPart)obj).UserCode_CmdSendIsKinematic__Boolean(reader.ReadBool());
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0006ACE0 File Offset: 0x00068EE0
		protected void UserCode_CmdSendUseGravity__Boolean(bool useGravity)
		{
			if (this.targetRigidbody == null)
			{
				return;
			}
			if (!this.clientAuthority)
			{
				return;
			}
			this.NetworkuseGravity = useGravity;
			this.targetRigidbody.useGravity = useGravity;
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x0006AD0D File Offset: 0x00068F0D
		protected static void InvokeUserCode_CmdSendUseGravity__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSendUseGravity called on client.");
				return;
			}
			((NetworkPlayerPart)obj).UserCode_CmdSendUseGravity__Boolean(reader.ReadBool());
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0006AD36 File Offset: 0x00068F36
		protected void UserCode_CmdSendDrag__Single(float drag)
		{
			if (this.targetRigidbody == null)
			{
				return;
			}
			if (!this.clientAuthority)
			{
				return;
			}
			this.Networkdrag = drag;
			this.targetRigidbody.drag = drag;
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0006AD63 File Offset: 0x00068F63
		protected static void InvokeUserCode_CmdSendDrag__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSendDrag called on client.");
				return;
			}
			((NetworkPlayerPart)obj).UserCode_CmdSendDrag__Single(reader.ReadFloat());
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0006AD8D File Offset: 0x00068F8D
		protected void UserCode_CmdSendAngularDrag__Single(float angularDrag)
		{
			if (this.targetRigidbody == null)
			{
				return;
			}
			if (!this.clientAuthority)
			{
				return;
			}
			this.NetworkangularDrag = angularDrag;
			this.targetRigidbody.angularDrag = angularDrag;
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0006ADBA File Offset: 0x00068FBA
		protected static void InvokeUserCode_CmdSendAngularDrag__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSendAngularDrag called on client.");
				return;
			}
			((NetworkPlayerPart)obj).UserCode_CmdSendAngularDrag__Single(reader.ReadFloat());
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0006ADE4 File Offset: 0x00068FE4
		protected void UserCode_CmdSendTargetRotation__Quaternion(Quaternion targetRotation)
		{
			if (!this.clientAuthority)
			{
				return;
			}
			this.NetworktargetJoint1Rotation = targetRotation;
			this.targetJoint1.targetRotation = targetRotation;
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0006AE02 File Offset: 0x00069002
		protected static void InvokeUserCode_CmdSendTargetRotation__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSendTargetRotation called on client.");
				return;
			}
			((NetworkPlayerPart)obj).UserCode_CmdSendTargetRotation__Quaternion(reader.ReadQuaternion());
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0006AE2B File Offset: 0x0006902B
		protected void UserCode_CmdSendTargetRotation2__Quaternion(Quaternion targetRotation)
		{
			if (!this.clientAuthority)
			{
				return;
			}
			this.NetworktargetJoint2Rotation = targetRotation;
			this.targetJoint2.targetRotation = targetRotation;
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0006AE49 File Offset: 0x00069049
		protected static void InvokeUserCode_CmdSendTargetRotation2__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSendTargetRotation2 called on client.");
				return;
			}
			((NetworkPlayerPart)obj).UserCode_CmdSendTargetRotation2__Quaternion(reader.ReadQuaternion());
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0006AE74 File Offset: 0x00069074
		protected void UserCode_CmdClientToServerSync__Vector3__Quaternion__Vector3(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			if (!this.clientAuthority)
			{
				return;
			}
			this.SetGoal(position, rotation, scale);
			if (base.isServer && !base.isClient)
			{
				this.ApplyPositionRotationScale(this.goal.localPosition, this.goal.localRotation, this.goal.localScale);
			}
			this.RpcMove(position, rotation, scale);
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0006AED3 File Offset: 0x000690D3
		protected static void InvokeUserCode_CmdClientToServerSync__Vector3__Quaternion__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdClientToServerSync called on client.");
				return;
			}
			((NetworkPlayerPart)obj).UserCode_CmdClientToServerSync__Vector3__Quaternion__Vector3(reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadVector3());
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0006AF08 File Offset: 0x00069108
		protected void UserCode_RpcMove__Vector3__Quaternion__Vector3(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			if (base.hasAuthority && this.excludeOwnerUpdate)
			{
				return;
			}
			if (!base.isServer)
			{
				this.SetGoal(position, rotation, scale);
			}
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0006AF2C File Offset: 0x0006912C
		protected static void InvokeUserCode_RpcMove__Vector3__Quaternion__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcMove called on server.");
				return;
			}
			((NetworkPlayerPart)obj).UserCode_RpcMove__Vector3__Quaternion__Vector3(reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadVector3());
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0006AF64 File Offset: 0x00069164
		static NetworkPlayerPart()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkPlayerPart), "System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendVelocity(UnityEngine.Vector3)", new RemoteCallDelegate(NetworkPlayerPart.InvokeUserCode_CmdSendVelocity__Vector3), true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkPlayerPart), "System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendVelocityAndAngular(UnityEngine.Vector3,UnityEngine.Vector3)", new RemoteCallDelegate(NetworkPlayerPart.InvokeUserCode_CmdSendVelocityAndAngular__Vector3__Vector3), true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkPlayerPart), "System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendIsKinematic(System.Boolean)", new RemoteCallDelegate(NetworkPlayerPart.InvokeUserCode_CmdSendIsKinematic__Boolean), true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkPlayerPart), "System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendUseGravity(System.Boolean)", new RemoteCallDelegate(NetworkPlayerPart.InvokeUserCode_CmdSendUseGravity__Boolean), true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkPlayerPart), "System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendDrag(System.Single)", new RemoteCallDelegate(NetworkPlayerPart.InvokeUserCode_CmdSendDrag__Single), true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkPlayerPart), "System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendAngularDrag(System.Single)", new RemoteCallDelegate(NetworkPlayerPart.InvokeUserCode_CmdSendAngularDrag__Single), true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkPlayerPart), "System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendTargetRotation(UnityEngine.Quaternion)", new RemoteCallDelegate(NetworkPlayerPart.InvokeUserCode_CmdSendTargetRotation__Quaternion), true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkPlayerPart), "System.Void Mirror.Experimental.NetworkPlayerPart::CmdSendTargetRotation2(UnityEngine.Quaternion)", new RemoteCallDelegate(NetworkPlayerPart.InvokeUserCode_CmdSendTargetRotation2__Quaternion), true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkPlayerPart), "System.Void Mirror.Experimental.NetworkPlayerPart::CmdClientToServerSync(UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Vector3)", new RemoteCallDelegate(NetworkPlayerPart.InvokeUserCode_CmdClientToServerSync__Vector3__Quaternion__Vector3), true);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkPlayerPart), "System.Void Mirror.Experimental.NetworkPlayerPart::RpcMove(UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Vector3)", new RemoteCallDelegate(NetworkPlayerPart.InvokeUserCode_RpcMove__Vector3__Quaternion__Vector3));
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x0006B0BC File Offset: 0x000692BC
		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteQuaternion(this.targetJoint1Rotation);
				writer.WriteQuaternion(this.targetJoint2Rotation);
				writer.WriteVector3(this.velocity);
				writer.WriteVector3(this.angularVelocity);
				writer.WriteBool(this.isKinematic);
				writer.WriteBool(this.useGravity);
				writer.WriteFloat(this.drag);
				writer.WriteFloat(this.angularDrag);
				writer.WriteBool(this.excludeOwnerUpdate);
				writer.WriteBool(this.syncPosition);
				writer.WriteBool(this.syncRotation);
				writer.WriteBool(this.syncScale);
				writer.WriteBool(this.interpolatePosition);
				writer.WriteBool(this.interpolateRotation);
				writer.WriteBool(this.interpolateScale);
				writer.WriteFloat(this.localPositionSensitivity);
				writer.WriteFloat(this.localRotationSensitivity);
				writer.WriteFloat(this.localScaleSensitivity);
				return;
			}
			writer.WriteULong(base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1UL) != 0UL)
			{
				writer.WriteQuaternion(this.targetJoint1Rotation);
			}
			if ((base.syncVarDirtyBits & 2UL) != 0UL)
			{
				writer.WriteQuaternion(this.targetJoint2Rotation);
			}
			if ((base.syncVarDirtyBits & 4UL) != 0UL)
			{
				writer.WriteVector3(this.velocity);
			}
			if ((base.syncVarDirtyBits & 8UL) != 0UL)
			{
				writer.WriteVector3(this.angularVelocity);
			}
			if ((base.syncVarDirtyBits & 16UL) != 0UL)
			{
				writer.WriteBool(this.isKinematic);
			}
			if ((base.syncVarDirtyBits & 32UL) != 0UL)
			{
				writer.WriteBool(this.useGravity);
			}
			if ((base.syncVarDirtyBits & 64UL) != 0UL)
			{
				writer.WriteFloat(this.drag);
			}
			if ((base.syncVarDirtyBits & 128UL) != 0UL)
			{
				writer.WriteFloat(this.angularDrag);
			}
			if ((base.syncVarDirtyBits & 256UL) != 0UL)
			{
				writer.WriteBool(this.excludeOwnerUpdate);
			}
			if ((base.syncVarDirtyBits & 512UL) != 0UL)
			{
				writer.WriteBool(this.syncPosition);
			}
			if ((base.syncVarDirtyBits & 1024UL) != 0UL)
			{
				writer.WriteBool(this.syncRotation);
			}
			if ((base.syncVarDirtyBits & 2048UL) != 0UL)
			{
				writer.WriteBool(this.syncScale);
			}
			if ((base.syncVarDirtyBits & 4096UL) != 0UL)
			{
				writer.WriteBool(this.interpolatePosition);
			}
			if ((base.syncVarDirtyBits & 8192UL) != 0UL)
			{
				writer.WriteBool(this.interpolateRotation);
			}
			if ((base.syncVarDirtyBits & 16384UL) != 0UL)
			{
				writer.WriteBool(this.interpolateScale);
			}
			if ((base.syncVarDirtyBits & 32768UL) != 0UL)
			{
				writer.WriteFloat(this.localPositionSensitivity);
			}
			if ((base.syncVarDirtyBits & 65536UL) != 0UL)
			{
				writer.WriteFloat(this.localRotationSensitivity);
			}
			if ((base.syncVarDirtyBits & 131072UL) != 0UL)
			{
				writer.WriteFloat(this.localScaleSensitivity);
			}
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x0006B424 File Offset: 0x00069624
		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				base.GeneratedSyncVarDeserialize<Quaternion>(ref this.targetJoint1Rotation, new Action<Quaternion, Quaternion>(this.OnTargetJoint1RotationChanged), reader.ReadQuaternion());
				base.GeneratedSyncVarDeserialize<Quaternion>(ref this.targetJoint2Rotation, new Action<Quaternion, Quaternion>(this.OnTargetJoint2RotationChanged), reader.ReadQuaternion());
				base.GeneratedSyncVarDeserialize<Vector3>(ref this.velocity, new Action<Vector3, Vector3>(this.OnVelocityChanged), reader.ReadVector3());
				base.GeneratedSyncVarDeserialize<Vector3>(ref this.angularVelocity, new Action<Vector3, Vector3>(this.OnAngularVelocityChanged), reader.ReadVector3());
				base.GeneratedSyncVarDeserialize<bool>(ref this.isKinematic, new Action<bool, bool>(this.OnIsKinematicChanged), reader.ReadBool());
				base.GeneratedSyncVarDeserialize<bool>(ref this.useGravity, new Action<bool, bool>(this.OnUseGravityChanged), reader.ReadBool());
				base.GeneratedSyncVarDeserialize<float>(ref this.drag, new Action<float, float>(this.OnuDragChanged), reader.ReadFloat());
				base.GeneratedSyncVarDeserialize<float>(ref this.angularDrag, new Action<float, float>(this.OnAngularDragChanged), reader.ReadFloat());
				base.GeneratedSyncVarDeserialize<bool>(ref this.excludeOwnerUpdate, null, reader.ReadBool());
				base.GeneratedSyncVarDeserialize<bool>(ref this.syncPosition, null, reader.ReadBool());
				base.GeneratedSyncVarDeserialize<bool>(ref this.syncRotation, null, reader.ReadBool());
				base.GeneratedSyncVarDeserialize<bool>(ref this.syncScale, null, reader.ReadBool());
				base.GeneratedSyncVarDeserialize<bool>(ref this.interpolatePosition, null, reader.ReadBool());
				base.GeneratedSyncVarDeserialize<bool>(ref this.interpolateRotation, null, reader.ReadBool());
				base.GeneratedSyncVarDeserialize<bool>(ref this.interpolateScale, null, reader.ReadBool());
				base.GeneratedSyncVarDeserialize<float>(ref this.localPositionSensitivity, null, reader.ReadFloat());
				base.GeneratedSyncVarDeserialize<float>(ref this.localRotationSensitivity, null, reader.ReadFloat());
				base.GeneratedSyncVarDeserialize<float>(ref this.localScaleSensitivity, null, reader.ReadFloat());
				return;
			}
			long num = (long)reader.ReadULong();
			if ((num & 1L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<Quaternion>(ref this.targetJoint1Rotation, new Action<Quaternion, Quaternion>(this.OnTargetJoint1RotationChanged), reader.ReadQuaternion());
			}
			if ((num & 2L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<Quaternion>(ref this.targetJoint2Rotation, new Action<Quaternion, Quaternion>(this.OnTargetJoint2RotationChanged), reader.ReadQuaternion());
			}
			if ((num & 4L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<Vector3>(ref this.velocity, new Action<Vector3, Vector3>(this.OnVelocityChanged), reader.ReadVector3());
			}
			if ((num & 8L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<Vector3>(ref this.angularVelocity, new Action<Vector3, Vector3>(this.OnAngularVelocityChanged), reader.ReadVector3());
			}
			if ((num & 16L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<bool>(ref this.isKinematic, new Action<bool, bool>(this.OnIsKinematicChanged), reader.ReadBool());
			}
			if ((num & 32L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<bool>(ref this.useGravity, new Action<bool, bool>(this.OnUseGravityChanged), reader.ReadBool());
			}
			if ((num & 64L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<float>(ref this.drag, new Action<float, float>(this.OnuDragChanged), reader.ReadFloat());
			}
			if ((num & 128L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<float>(ref this.angularDrag, new Action<float, float>(this.OnAngularDragChanged), reader.ReadFloat());
			}
			if ((num & 256L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<bool>(ref this.excludeOwnerUpdate, null, reader.ReadBool());
			}
			if ((num & 512L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<bool>(ref this.syncPosition, null, reader.ReadBool());
			}
			if ((num & 1024L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<bool>(ref this.syncRotation, null, reader.ReadBool());
			}
			if ((num & 2048L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<bool>(ref this.syncScale, null, reader.ReadBool());
			}
			if ((num & 4096L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<bool>(ref this.interpolatePosition, null, reader.ReadBool());
			}
			if ((num & 8192L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<bool>(ref this.interpolateRotation, null, reader.ReadBool());
			}
			if ((num & 16384L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<bool>(ref this.interpolateScale, null, reader.ReadBool());
			}
			if ((num & 32768L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<float>(ref this.localPositionSensitivity, null, reader.ReadFloat());
			}
			if ((num & 65536L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<float>(ref this.localRotationSensitivity, null, reader.ReadFloat());
			}
			if ((num & 131072L) != 0L)
			{
				base.GeneratedSyncVarDeserialize<float>(ref this.localScaleSensitivity, null, reader.ReadFloat());
			}
		}

		// Token: 0x04000F7E RID: 3966
		[Header("Settings")]
		[SerializeField]
		public ConfigurableJoint targetJoint1;

		// Token: 0x04000F7F RID: 3967
		[SerializeField]
		public ConfigurableJoint targetJoint2;

		// Token: 0x04000F80 RID: 3968
		[SerializeField]
		public string targetName = "";

		// Token: 0x04000F81 RID: 3969
		[Tooltip("Set to true if moves come from owner client, set to false if moves always come from server")]
		[SerializeField]
		public bool clientAuthority;

		// Token: 0x04000F82 RID: 3970
		[SerializeField]
		private bool syncTargetRotation = true;

		// Token: 0x04000F83 RID: 3971
		[Header("Settings")]
		[SerializeField]
		public Rigidbody targetRigidbody;

		// Token: 0x04000F84 RID: 3972
		[Header("Velocity")]
		[Tooltip("Syncs Velocity every SyncInterval")]
		[SerializeField]
		private bool syncVelocity = true;

		// Token: 0x04000F85 RID: 3973
		[Tooltip("Set velocity to 0 each frame (only works if syncVelocity is false")]
		[SerializeField]
		private bool clearVelocity;

		// Token: 0x04000F86 RID: 3974
		[Tooltip("Only Syncs Value if distance between previous and current is great than sensitivity")]
		[SerializeField]
		private float velocitySensitivity = 0.1f;

		// Token: 0x04000F87 RID: 3975
		[Header("Angular Velocity")]
		[Tooltip("Syncs AngularVelocity every SyncInterval")]
		[SerializeField]
		private bool syncAngularVelocity = true;

		// Token: 0x04000F88 RID: 3976
		[Tooltip("Set angularVelocity to 0 each frame (only works if syncAngularVelocity is false")]
		[SerializeField]
		private bool clearAngularVelocity;

		// Token: 0x04000F89 RID: 3977
		[Tooltip("Only Syncs Value if distance between previous and current is great than sensitivity")]
		[SerializeField]
		private float angularVelocitySensitivity = 0.1f;

		// Token: 0x04000F8A RID: 3978
		private readonly NetworkPlayerPart.ClientSyncState previousValue = new NetworkPlayerPart.ClientSyncState();

		// Token: 0x04000F8B RID: 3979
		[SyncVar(hook = "OnTargetJoint1RotationChanged")]
		private Quaternion targetJoint1Rotation;

		// Token: 0x04000F8C RID: 3980
		[SyncVar(hook = "OnTargetJoint2RotationChanged")]
		private Quaternion targetJoint2Rotation;

		// Token: 0x04000F8D RID: 3981
		[SyncVar(hook = "OnVelocityChanged")]
		private Vector3 velocity;

		// Token: 0x04000F8E RID: 3982
		[SyncVar(hook = "OnAngularVelocityChanged")]
		private Vector3 angularVelocity;

		// Token: 0x04000F8F RID: 3983
		[SyncVar(hook = "OnIsKinematicChanged")]
		private bool isKinematic;

		// Token: 0x04000F90 RID: 3984
		[SyncVar(hook = "OnUseGravityChanged")]
		private bool useGravity;

		// Token: 0x04000F91 RID: 3985
		[SyncVar(hook = "OnuDragChanged")]
		private float drag;

		// Token: 0x04000F92 RID: 3986
		[SyncVar(hook = "OnAngularDragChanged")]
		private float angularDrag;

		// Token: 0x04000F93 RID: 3987
		[Header("Settings")]
		public Transform targetTransform;

		// Token: 0x04000F94 RID: 3988
		[Header("Authority")]
		[Tooltip("Set to true if updates from server should be ignored by owner")]
		[SyncVar]
		public bool excludeOwnerUpdate = true;

		// Token: 0x04000F95 RID: 3989
		[Header("Synchronization")]
		[Tooltip("Set to true if position should be synchronized")]
		[SyncVar]
		public bool syncPosition = true;

		// Token: 0x04000F96 RID: 3990
		[Tooltip("Set to true if rotation should be synchronized")]
		[SyncVar]
		public bool syncRotation = true;

		// Token: 0x04000F97 RID: 3991
		[Tooltip("Set to true if scale should be synchronized")]
		[SyncVar]
		public bool syncScale = true;

		// Token: 0x04000F98 RID: 3992
		[Header("Interpolation")]
		[Tooltip("Set to true if position should be interpolated")]
		[SyncVar]
		public bool interpolatePosition = true;

		// Token: 0x04000F99 RID: 3993
		[Tooltip("Set to true if rotation should be interpolated")]
		[SyncVar]
		public bool interpolateRotation = true;

		// Token: 0x04000F9A RID: 3994
		[Tooltip("Set to true if scale should be interpolated")]
		[SyncVar]
		public bool interpolateScale = true;

		// Token: 0x04000F9B RID: 3995
		[Header("Sensitivity")]
		[Tooltip("Changes to the transform must exceed these values to be transmitted on the network.")]
		[SyncVar]
		public float localPositionSensitivity = 0.01f;

		// Token: 0x04000F9C RID: 3996
		[Tooltip("If rotation exceeds this angle, it will be transmitted on the network")]
		[SyncVar]
		public float localRotationSensitivity = 0.01f;

		// Token: 0x04000F9D RID: 3997
		[Tooltip("Changes to the transform must exceed these values to be transmitted on the network.")]
		[SyncVar]
		public float localScaleSensitivity = 0.01f;

		// Token: 0x04000F9E RID: 3998
		[Header("Diagnostics")]
		public Vector3 lastPosition;

		// Token: 0x04000F9F RID: 3999
		public Quaternion lastRotation;

		// Token: 0x04000FA0 RID: 4000
		public Vector3 lastScale;

		// Token: 0x04000FA1 RID: 4001
		public NetworkPlayerPart.DataPoint start;

		// Token: 0x04000FA2 RID: 4002
		public NetworkPlayerPart.DataPoint goal;

		// Token: 0x020002C0 RID: 704
		public class ClientSyncState
		{
			// Token: 0x04000FA3 RID: 4003
			public float nextSyncTime;

			// Token: 0x04000FA4 RID: 4004
			public Quaternion targetJoint1Rotation;

			// Token: 0x04000FA5 RID: 4005
			public Quaternion targetJoint2Rotation;

			// Token: 0x04000FA6 RID: 4006
			public Vector3 velocity;

			// Token: 0x04000FA7 RID: 4007
			public Vector3 angularVelocity;

			// Token: 0x04000FA8 RID: 4008
			public bool isKinematic;

			// Token: 0x04000FA9 RID: 4009
			public bool useGravity;

			// Token: 0x04000FAA RID: 4010
			public float drag;

			// Token: 0x04000FAB RID: 4011
			public float angularDrag;
		}

		// Token: 0x020002C1 RID: 705
		[Serializable]
		public struct DataPoint
		{
			// Token: 0x17000279 RID: 633
			// (get) Token: 0x06001576 RID: 5494 RVA: 0x0006B8D6 File Offset: 0x00069AD6
			public bool isValid
			{
				get
				{
					return this.timeStamp != 0f;
				}
			}

			// Token: 0x04000FAC RID: 4012
			public float timeStamp;

			// Token: 0x04000FAD RID: 4013
			public Vector3 localPosition;

			// Token: 0x04000FAE RID: 4014
			public Quaternion localRotation;

			// Token: 0x04000FAF RID: 4015
			public Vector3 localScale;

			// Token: 0x04000FB0 RID: 4016
			public float movementSpeed;
		}
	}
}
