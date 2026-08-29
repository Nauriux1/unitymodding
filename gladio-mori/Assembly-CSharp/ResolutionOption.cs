using System;

// Token: 0x020000B8 RID: 184
public class ResolutionOption
{
	// Token: 0x06000664 RID: 1636 RVA: 0x00020519 File Offset: 0x0001E719
	public override string ToString()
	{
		return string.Format("{0} x {1}", this.width, this.height);
	}

	// Token: 0x04000452 RID: 1106
	public int width;

	// Token: 0x04000453 RID: 1107
	public int height;

	// Token: 0x04000454 RID: 1108
	public int refreshRate;
}
