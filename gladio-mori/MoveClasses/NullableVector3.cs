using System;
using Newtonsoft.Json;
using ProtoBuf;
using UnityEngine;

namespace MoveClasses
{
	// Token: 0x020002AC RID: 684
	[ProtoContract]
	[Serializable]
	public class NullableVector3
	{
		// Token: 0x06001407 RID: 5127 RVA: 0x00065840 File Offset: 0x00063A40
		[JsonConstructor]
		public NullableVector3(float? X = null, float? Y = null, float? Z = null)
		{
			this.x = X;
			this.y = Y;
			this.z = Z;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0006585D File Offset: 0x00063A5D
		public NullableVector3(Vector3 values)
		{
			this.x = new float?(values.x);
			this.y = new float?(values.y);
			this.z = new float?(values.z);
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x00065898 File Offset: 0x00063A98
		public void SetValues(float? X = null, float? Y = null, float? Z = null)
		{
			this.x = X;
			this.y = Y;
			this.z = Z;
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x000658AF File Offset: 0x00063AAF
		public void SetValues(Vector3 values)
		{
			this.x = new float?(values.x);
			this.y = new float?(values.y);
			this.z = new float?(values.z);
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x000658E4 File Offset: 0x00063AE4
		// (set) Token: 0x0600140C RID: 5132 RVA: 0x000658EC File Offset: 0x00063AEC
		[ProtoMember(1)]
		public float? x
		{
			get
			{
				return this._x;
			}
			set
			{
				if (value != null && value.Value > 180f)
				{
					this._x = new float?(value.Value - 360f);
					return;
				}
				if (value != null && value.Value < -180f)
				{
					this._x = new float?(value.Value + 360f);
					return;
				}
				this._x = value;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x0600140D RID: 5133 RVA: 0x00065960 File Offset: 0x00063B60
		// (set) Token: 0x0600140E RID: 5134 RVA: 0x00065968 File Offset: 0x00063B68
		[ProtoMember(2)]
		public float? y
		{
			get
			{
				return this._y;
			}
			set
			{
				if (value != null && value.Value > 180f)
				{
					this._y = new float?(value.Value - 360f);
					return;
				}
				if (value != null && value.Value < -180f)
				{
					this._y = new float?(value.Value + 360f);
					return;
				}
				this._y = value;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x000659DC File Offset: 0x00063BDC
		// (set) Token: 0x06001410 RID: 5136 RVA: 0x000659E4 File Offset: 0x00063BE4
		[ProtoMember(3)]
		public float? z
		{
			get
			{
				return this._z;
			}
			set
			{
				if (value != null && value.Value > 180f)
				{
					this._z = new float?(value.Value - 360f);
					return;
				}
				if (value != null && value.Value < -180f)
				{
					this._z = new float?(value.Value + 360f);
					return;
				}
				this._z = value;
			}
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x00065A58 File Offset: 0x00063C58
		public Vector3 ConvertToVector3()
		{
			Vector3 result = new Vector3(0f, 0f, 0f);
			if (this.x != null)
			{
				result.x = this.x.Value;
			}
			if (this.y != null)
			{
				result.y = this.y.Value;
			}
			if (this.z != null)
			{
				result.z = this.z.Value;
			}
			return result;
		}

		// Token: 0x04000ED3 RID: 3795
		private float? _x;

		// Token: 0x04000ED4 RID: 3796
		private float? _y;

		// Token: 0x04000ED5 RID: 3797
		private float? _z;
	}
}
