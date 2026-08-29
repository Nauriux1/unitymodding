using System;
using MoveClasses;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class Bodypart : MonoBehaviour
{
	// Token: 0x060009EA RID: 2538 RVA: 0x0002EBC3 File Offset: 0x0002CDC3
	public virtual void Start()
	{
		this.startRotation = base.transform.localRotation;
	}

	// Token: 0x040006E8 RID: 1768
	public Rigidbody bodypartRigidbody;

	// Token: 0x040006E9 RID: 1769
	public Transform spawnPosition;

	// Token: 0x040006EA RID: 1770
	public Quaternion startRotation;

	// Token: 0x040006EB RID: 1771
	public EquipmentPosition equipmentPosition;
}
