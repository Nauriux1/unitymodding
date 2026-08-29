using System;
using Mirror;

// Token: 0x02000053 RID: 83
public interface IInputManager
{
	// Token: 0x0600023F RID: 575
	void SetupBasicCameraControls();

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000240 RID: 576
	// (set) Token: 0x06000241 RID: 577
	MultiplayerRoomPlayer multiplayerRoomPlayer { get; set; }
}
