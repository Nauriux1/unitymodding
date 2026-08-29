using System;
using UnityEngine;

// Token: 0x020000E8 RID: 232
[CreateAssetMenu(fileName = "PhysicsMaterialLegacyItem", menuName = "LegacyMode/PhysicsMaterial", order = 1)]
public class PhysicsMaterialLegacyItem : ScriptableObject
{
	// Token: 0x060007D5 RID: 2005 RVA: 0x00026B60 File Offset: 0x00024D60
	public void SetPhysicsMaterialValues(bool legacy)
	{
		PhysicMaterial physicMaterial = this.currentPhysicsMaterial;
		if (legacy)
		{
			physicMaterial = this.legacyPhysicsMaterial;
		}
		this.actualPhysicsMaterial.dynamicFriction = physicMaterial.dynamicFriction;
		this.actualPhysicsMaterial.staticFriction = physicMaterial.staticFriction;
		this.actualPhysicsMaterial.bounciness = physicMaterial.bounciness;
		this.actualPhysicsMaterial.frictionCombine = physicMaterial.frictionCombine;
		this.actualPhysicsMaterial.bounceCombine = physicMaterial.bounceCombine;
	}

	// Token: 0x04000551 RID: 1361
	public PhysicMaterial actualPhysicsMaterial;

	// Token: 0x04000552 RID: 1362
	public PhysicMaterial currentPhysicsMaterial;

	// Token: 0x04000553 RID: 1363
	public PhysicMaterial legacyPhysicsMaterial;
}
