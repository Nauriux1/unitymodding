using System;
using System.Collections.Generic;
using MoveClasses;
using UnityEngine;

// Token: 0x0200006D RID: 109
[Serializable]
public class CuttableSection
{
	// Token: 0x04000205 RID: 517
	public ConfigurableJoint joint;

	// Token: 0x04000206 RID: 518
	public Hand hand;

	// Token: 0x04000207 RID: 519
	public Transform gameObjectTransform;

	// Token: 0x04000208 RID: 520
	public bool isEquipment;

	// Token: 0x04000209 RID: 521
	public CuttableGameObject cuttableGameObject;

	// Token: 0x0400020A RID: 522
	public WeaponDamageablePart artery;

	// Token: 0x0400020B RID: 523
	public bool instantKill;

	// Token: 0x0400020C RID: 524
	public DeathReason deathReason = DeathReason.Spine;

	// Token: 0x0400020D RID: 525
	public bool parentSection;

	// Token: 0x0400020E RID: 526
	public Vector3 position;

	// Token: 0x0400020F RID: 527
	public List<ConfigurableJointScript> configurableJointScripts;
}
