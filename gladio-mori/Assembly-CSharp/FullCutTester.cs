using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class FullCutTester : MonoBehaviour
{
	// Token: 0x06000331 RID: 817 RVA: 0x00010E7C File Offset: 0x0000F07C
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.parent != null)
		{
			CuttableGameObject component = other.transform.parent.gameObject.GetComponent<CuttableGameObject>();
			if (component != null)
			{
				this.cuttableGameObjects.Add(component);
			}
		}
	}

	// Token: 0x06000332 RID: 818 RVA: 0x00010EC8 File Offset: 0x0000F0C8
	private void OnTriggerExit(Collider other)
	{
		if (other.transform.parent != null)
		{
			CuttableGameObject component = other.transform.parent.gameObject.GetComponent<CuttableGameObject>();
			if (component != null)
			{
				this.cuttableGameObjects.Remove(component);
			}
		}
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00010F14 File Offset: 0x0000F114
	private void OnTriggerStay(Collider other)
	{
		if (!this.doCut)
		{
			return;
		}
		if (other.transform.parent != null)
		{
			CuttableGameObject component = other.transform.parent.gameObject.GetComponent<CuttableGameObject>();
			if (component != null && this.cuttableToUse == null)
			{
				this.doCut = false;
				Vector3 inPoint = component.transform.worldToLocalMatrix.MultiplyPoint3x4(base.transform.position);
				Vector3 inNormal = component.transform.worldToLocalMatrix.MultiplyVector(base.transform.forward);
				Plane plane = new Plane(inNormal, inPoint);
				component.DoFullCut(plane, 0U);
			}
		}
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00010FC8 File Offset: 0x0000F1C8
	private void Update()
	{
		if (!this.doCut)
		{
			return;
		}
		if (this.cuttableToUse != null && this.doCut)
		{
			this.doCut = false;
			Vector3 inPoint = this.cuttableToUse.transform.worldToLocalMatrix.MultiplyPoint3x4(base.transform.position);
			Vector3 inNormal = this.cuttableToUse.transform.worldToLocalMatrix.MultiplyVector(base.transform.forward);
			Plane plane = new Plane(inNormal, inPoint);
			this.cuttableToUse.DoFullCut(plane, 0U);
		}
	}

	// Token: 0x0400023A RID: 570
	public bool doCut;

	// Token: 0x0400023B RID: 571
	public List<CuttableGameObject> cuttableGameObjects = new List<CuttableGameObject>();

	// Token: 0x0400023C RID: 572
	public CuttableGameObject cuttableToUse;
}
