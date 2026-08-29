using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Utils;

// Token: 0x02000128 RID: 296
public class MultiplayerTransform : NetworkTransformReliable
{
	// Token: 0x06000944 RID: 2372 RVA: 0x0002C512 File Offset: 0x0002A712
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x0002C51C File Offset: 0x0002A71C
	protected override bool Changed(TransformSnapshot current)
	{
		return !this.disableChanges && (base.QuantizedChanged(this.last.position, current.position, this.positionPrecision) || !Generic.IsQuaternionApproximate(this.last.rotation, current.rotation, 1E-08f) || base.QuantizedChanged(this.last.scale, current.scale, this.scalePrecision));
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x0002C591 File Offset: 0x0002A791
	protected override void Apply(TransformSnapshot interpolated, TransformSnapshot endGoal)
	{
		if (this.disableChanges)
		{
			return;
		}
		base.Apply(interpolated, endGoal);
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x0002C5A4 File Offset: 0x0002A7A4
	public void ResetPositionInterpolation()
	{
		if (!base.isClientOnly)
		{
			return;
		}
		MultiplayerTransform.RewriteHistoryForReset(this.clientSnapshots, NetworkClient.connection.remoteTimeStamp, NetworkTime.localTime, (double)(NetworkClient.sendInterval * this.sendIntervalMultiplier), base.GetPosition(), base.GetRotation(), base.GetScale());
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x0002C5F5 File Offset: 0x0002A7F5
	private static void RewriteHistoryForReset(SortedList<double, TransformSnapshot> snapshots, double remoteTimeStamp, double localTime, double sendInterval, Vector3 position, Quaternion rotation, Vector3 scale)
	{
		snapshots.Clear();
		SnapshotInterpolation.InsertIfNotExists<TransformSnapshot>(snapshots, NetworkClient.snapshotSettings.bufferLimit, new TransformSnapshot(remoteTimeStamp - sendInterval, localTime - sendInterval, position, rotation, scale));
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x04000675 RID: 1653
	public bool disableChanges;

	// Token: 0x04000676 RID: 1654
	public HandMultiplayer holdingHand;
}
