using System;

// Token: 0x0200022F RID: 559
public enum ConnectionEndedType
{
	// Token: 0x04000C46 RID: 3142
	None,
	// Token: 0x04000C47 RID: 3143
	[LocalizedDescription("alert_server_connection_lost")]
	ConnectionLost,
	// Token: 0x04000C48 RID: 3144
	[LocalizedDescription("alert_server_version_conflict")]
	VersionConflict,
	// Token: 0x04000C49 RID: 3145
	[LocalizedDescription("alert_server_failed_to_connect")]
	FailedToConnect,
	// Token: 0x04000C4A RID: 3146
	[LocalizedDescription("alert_server_failed_to_create")]
	FailedToCreate
}
