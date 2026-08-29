using System;
using Mirror;

namespace Dissonance.Integrations.MirrorIgnorance
{
	// Token: 0x0200030D RID: 781
	public readonly struct MirrorConn : IEquatable<MirrorConn>
	{
		// Token: 0x0600176C RID: 5996 RVA: 0x00076A9E File Offset: 0x00074C9E
		public MirrorConn(NetworkConnection connection)
		{
			this = default(MirrorConn);
			this.Connection = connection;
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x00076AAE File Offset: 0x00074CAE
		public override int GetHashCode()
		{
			return this.Connection.GetHashCode();
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x00076ABB File Offset: 0x00074CBB
		public override string ToString()
		{
			return this.Connection.ToString();
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x00076AC8 File Offset: 0x00074CC8
		public override bool Equals(object obj)
		{
			return obj != null && obj is MirrorConn && this.Equals((MirrorConn)obj);
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x00076AE5 File Offset: 0x00074CE5
		public bool Equals(MirrorConn other)
		{
			if (this.Connection == null)
			{
				return other.Connection == null;
			}
			return this.Connection.Equals(other.Connection);
		}

		// Token: 0x04001159 RID: 4441
		public readonly NetworkConnection Connection;
	}
}
