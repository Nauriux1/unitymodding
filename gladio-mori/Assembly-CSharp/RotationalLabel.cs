using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000209 RID: 521
public class RotationalLabel : MonoBehaviour
{
	// Token: 0x06000FFF RID: 4095 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06001001 RID: 4097 RVA: 0x00053774 File Offset: 0x00051974
	public void AddToValue(double value)
	{
		float num;
		float.TryParse(this.inputField.text, out num);
		this.inputField.text = ((double)num + value).ToString();
	}

	// Token: 0x04000B74 RID: 2932
	public InputField inputField;
}
