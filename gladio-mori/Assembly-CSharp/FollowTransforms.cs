using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200011F RID: 287
public class FollowTransforms : MonoBehaviour
{
	// Token: 0x060008E8 RID: 2280 RVA: 0x0002B4EC File Offset: 0x000296EC
	private void Start()
	{
		if (this.enabledCheckbox != null)
		{
			this.enabledCheckbox.isOn = false;
			this.enabledCheckbox.onValueChanged.AddListener(delegate(bool <p0>)
			{
				this.ToggleVisibility();
			});
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x0002B53C File Offset: 0x0002973C
	private void LateUpdate()
	{
		Vector3 position = new Vector3(this.xTargetTransform.position.x, 0f, this.zTargetTransform.position.z);
		float? num = null;
		foreach (Transform transform in this.yTargetTransforms)
		{
			if (num == null || transform.position.y < num.Value)
			{
				num = new float?(transform.position.y);
			}
		}
		if (num != null)
		{
			position.y = num.Value;
		}
		position.y += this.offsets.y;
		base.transform.position = position;
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x0002B624 File Offset: 0x00029824
	private void ToggleVisibility()
	{
		base.gameObject.SetActive(this.enabledCheckbox.isOn);
	}

	// Token: 0x04000632 RID: 1586
	public Transform xTargetTransform;

	// Token: 0x04000633 RID: 1587
	public Transform zTargetTransform;

	// Token: 0x04000634 RID: 1588
	public List<Transform> yTargetTransforms;

	// Token: 0x04000635 RID: 1589
	public Vector3 offsets;

	// Token: 0x04000636 RID: 1590
	public Toggle enabledCheckbox;
}
