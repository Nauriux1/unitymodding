using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A1 RID: 417
public class JointStrengthToolListItem : MonoBehaviour
{
	// Token: 0x06000D00 RID: 3328 RVA: 0x00042024 File Offset: 0x00040224
	private void Start()
	{
		this.strengthInputField.onEndEdit.AddListener(delegate(string <p0>)
		{
			this.StrengthChanged();
		});
		this.damperInputField.onEndEdit.AddListener(delegate(string <p0>)
		{
			this.DamperChanged();
		});
		this.maximumForceMultiplierInputField.onEndEdit.AddListener(delegate(string <p0>)
		{
			this.MaximumForceChanged();
		});
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x00042088 File Offset: 0x00040288
	public void UpdateUI()
	{
		this.titleText.text = this.jointStrengthToolItem.jointName.Replace("_LEFT", "");
		this.strengthInputField.text = this.jointStrengthToolItem.maxStrength.ToString();
		this.damperInputField.text = this.jointStrengthToolItem.maxDamper.ToString();
		this.damperPercentageText.text = this.jointStrengthToolItem.damperPercentage.ToString();
		this.maximumForceMultiplierInputField.text = this.jointStrengthToolItem.jointMaximumForceMultiplier.ToString();
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x00042129 File Offset: 0x00040329
	public void StrengthChanged()
	{
		this.jointStrengthToolItem.maxStrength = Convert.ToSingle(this.strengthInputField.text);
		this.UpdateUI();
	}

	// Token: 0x06000D03 RID: 3331 RVA: 0x0004214C File Offset: 0x0004034C
	public void DamperChanged()
	{
		this.jointStrengthToolItem.maxDamper = Convert.ToSingle(this.damperInputField.text);
		this.UpdateUI();
	}

	// Token: 0x06000D04 RID: 3332 RVA: 0x0004216F File Offset: 0x0004036F
	public void MaximumForceChanged()
	{
		this.jointStrengthToolItem.jointMaximumForceMultiplier = Convert.ToSingle(this.maximumForceMultiplierInputField.text);
		this.UpdateUI();
	}

	// Token: 0x0400094D RID: 2381
	public Text titleText;

	// Token: 0x0400094E RID: 2382
	public InputField strengthInputField;

	// Token: 0x0400094F RID: 2383
	public InputField damperInputField;

	// Token: 0x04000950 RID: 2384
	public Text damperPercentageText;

	// Token: 0x04000951 RID: 2385
	public JointStrengthToolItem jointStrengthToolItem;

	// Token: 0x04000952 RID: 2386
	public InputField maximumForceMultiplierInputField;
}
