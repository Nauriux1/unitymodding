using System;
using System.Collections.Generic;
using MoveClasses;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class RigManager : MonoBehaviour
{
	// Token: 0x06000151 RID: 337 RVA: 0x00007868 File Offset: 0x00005A68
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x06000152 RID: 338 RVA: 0x0000777A File Offset: 0x0000597A
	private void Initialize()
	{
	}

	// Token: 0x06000153 RID: 339 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00007870 File Offset: 0x00005A70
	public void HandleRigAnimations()
	{
		if (this.doAnimation)
		{
			for (int i = 0; i < this.simpleRigs.Count; i++)
			{
				if (this.simpleRigs[i].TransformsHaveChanged())
				{
					this.UpdateSimpleRigAnimation(this.simpleRigs[i]);
					for (int j = 0; j < this.simpleRigs[i].animatedChildRigs.Count; j++)
					{
						this.UpdateSimpleRigAnimation(this.simpleRigs[i].animatedChildRigs[j]);
					}
					for (int k = 0; k < this.simpleRigs[i].targetChildRigs.Count; k++)
					{
						this.UpdateSimpleRigTargetPosition(this.simpleRigs[i].targetChildRigs[k]);
					}
				}
			}
		}
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00007948 File Offset: 0x00005B48
	private void UpdateSimpleRigAnimation(SimpleRig rig)
	{
		rig.CalculatePosition();
		foreach (JointMove newMove in rig.GetJointMoves())
		{
			MoveSetEditor.singleton.AddTempSingleMove(newMove);
		}
	}

	// Token: 0x06000156 RID: 342 RVA: 0x000079A8 File Offset: 0x00005BA8
	public void ToggleActive()
	{
		this.rigActive = !this.rigActive;
		base.gameObject.SetActive(this.rigActive);
		if (this.rigActive)
		{
			this.RecalculateTargetPosition();
		}
	}

	// Token: 0x06000157 RID: 343 RVA: 0x000079D8 File Offset: 0x00005BD8
	public void SetActive(bool setValue)
	{
		this.rigActive = setValue;
		base.gameObject.SetActive(this.rigActive);
		foreach (SimpleRig simpleRig in this.simpleRigs)
		{
			simpleRig.gameObject.SetActive(this.rigActive);
			if (simpleRig.hint != null)
			{
				simpleRig.hint.gameObject.SetActive(this.rigActive);
			}
		}
		if (this.rigActive)
		{
			this.RecalculateTargetPosition();
		}
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00007A80 File Offset: 0x00005C80
	public void RecalculateTargetPosition()
	{
		for (int i = 0; i < this.simpleRigs.Count; i++)
		{
			this.UpdateSimpleRigTargetPosition(this.simpleRigs[i]);
		}
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00007AB8 File Offset: 0x00005CB8
	private void UpdateSimpleRigTargetPosition(SimpleRig rig)
	{
		if (rig == this.twoHandedRig)
		{
			if (this.twoHandedRig.isActiveAndEnabled)
			{
				this.twoHandedRig.SetActive(false);
				this.twoHandedRig.SetActive(true);
				return;
			}
		}
		else
		{
			rig.CalculateTargetPosition();
			rig.TransformsHaveChanged();
		}
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00007B06 File Offset: 0x00005D06
	public void SetDoAnimation(bool value)
	{
		this.doAnimation = value;
	}

	// Token: 0x0400008C RID: 140
	private bool rigActive;

	// Token: 0x0400008D RID: 141
	private bool doAnimation;

	// Token: 0x0400008E RID: 142
	public List<SimpleRig> simpleRigs = new List<SimpleRig>();

	// Token: 0x0400008F RID: 143
	public TwoHandPositionRig twoHandedRig;
}
