using System;
using System.Text.RegularExpressions;
using Unity.Collections;
using Unity.Jobs;

// Token: 0x02000245 RID: 581
public struct StringFilterJob : IJob
{
	// Token: 0x060010E9 RID: 4329 RVA: 0x000575B4 File Offset: 0x000557B4
	public StringFilterJob(FixedString512Bytes text, FixedString4096Bytes regex, NativeArray<FixedString512Bytes> result)
	{
		this._text = text;
		this._result = result;
		this._regex = regex;
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x000575CB File Offset: 0x000557CB
	public void Execute()
	{
		this._result[0] = Regex.Replace(this._text.ToString(), this._regex.ToString(), "****");
	}

	// Token: 0x04000C94 RID: 3220
	public FixedString512Bytes _text;

	// Token: 0x04000C95 RID: 3221
	public FixedString4096Bytes _regex;

	// Token: 0x04000C96 RID: 3222
	private NativeArray<FixedString512Bytes> _result;
}
