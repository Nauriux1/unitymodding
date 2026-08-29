using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Utils
{
	// Token: 0x02000290 RID: 656
	[Serializable]
	public class PlayerCameraSettings
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x00063C14 File Offset: 0x00061E14
		// (set) Token: 0x06001347 RID: 4935 RVA: 0x00063C2D File Offset: 0x00061E2D
		[JsonIgnore]
		public Vector3 cameraPositionOffset
		{
			get
			{
				return new Vector3(this.cameraPositionOffsetX, this.cameraPositionOffsetY, this.cameraPositionOffsetZ);
			}
			set
			{
				this.cameraPositionOffsetX = value.x;
				this.cameraPositionOffsetY = value.y;
				this.cameraPositionOffsetZ = value.z;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06001348 RID: 4936 RVA: 0x00063C53 File Offset: 0x00061E53
		// (set) Token: 0x06001349 RID: 4937 RVA: 0x00063C6C File Offset: 0x00061E6C
		[JsonIgnore]
		public Vector3 cameraTargetOffset
		{
			get
			{
				return new Vector3(this.cameraTargetOffsetX, this.cameraTargetOffsetY, this.cameraTargetOffsetZ);
			}
			set
			{
				this.cameraTargetOffsetX = value.x;
				this.cameraTargetOffsetY = value.y;
				this.cameraTargetOffsetZ = value.z;
			}
		}

		// Token: 0x04000E46 RID: 3654
		public int cameraFov = 60;

		// Token: 0x04000E47 RID: 3655
		[JsonProperty]
		private float cameraPositionOffsetX;

		// Token: 0x04000E48 RID: 3656
		[JsonProperty]
		private float cameraPositionOffsetY = 1.8f;

		// Token: 0x04000E49 RID: 3657
		[JsonProperty]
		private float cameraPositionOffsetZ = -1.8f;

		// Token: 0x04000E4A RID: 3658
		[JsonProperty]
		private float cameraTargetOffsetX;

		// Token: 0x04000E4B RID: 3659
		[JsonProperty]
		private float cameraTargetOffsetY = 0.6f;

		// Token: 0x04000E4C RID: 3660
		[JsonProperty]
		private float cameraTargetOffsetZ;
	}
}
