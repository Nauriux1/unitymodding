using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

// Token: 0x020000A8 RID: 168
public class EnemyBlade
{
	// Token: 0x060005CB RID: 1483 RVA: 0x0001C300 File Offset: 0x0001A500
	public BladeDistanceItem CalculateClosestPointOnWeaponSections(Vector3 position1, Vector3 position2)
	{
		BladeDistanceItem bladeDistanceItem = default(BladeDistanceItem);
		bladeDistanceItem.distanceBetweenPoints = 999f;
		for (int i = 0; i < this.weaponSections.Count; i++)
		{
			WeaponSection weaponSection = this.weaponSections[i];
			Vector3 vector;
			Vector3 vector2;
			float num;
			float positionOnProtectedLine;
			if (Generic.ClosestPointsOnTwoLines(out vector, out vector2, out num, out positionOnProtectedLine, position1, position2 - position1, weaponSection.point0.position, weaponSection.point1.position - weaponSection.point0.position, true))
			{
				float num2 = Vector3.Distance(vector, vector2);
				if (num2 < bladeDistanceItem.distanceBetweenPoints)
				{
					bladeDistanceItem = new BladeDistanceItem
					{
						closestProtectedPoint = vector,
						closestWeaponPoint = vector2,
						distanceBetweenPoints = num2,
						positionOnProtectedLine = positionOnProtectedLine,
						vectorFromProtectedPointToWeaponPoint = vector2 - vector
					};
				}
			}
			else
			{
				float num3 = Vector3.Distance(position1, weaponSection.point0.position);
				if (num3 < bladeDistanceItem.distanceBetweenPoints)
				{
					bladeDistanceItem = new BladeDistanceItem
					{
						closestProtectedPoint = position1,
						closestWeaponPoint = weaponSection.point0.position,
						distanceBetweenPoints = num3,
						positionOnProtectedLine = 0f,
						vectorFromProtectedPointToWeaponPoint = weaponSection.point0.position - position1
					};
				}
			}
		}
		this.SetCurrentBladeDistanceItem(bladeDistanceItem);
		return bladeDistanceItem;
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x0001C455 File Offset: 0x0001A655
	public void SetCurrentBladeDistanceItem(BladeDistanceItem bladeDistanceItem)
	{
		this.currentBladeDistanceItem = bladeDistanceItem;
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x0001C45E File Offset: 0x0001A65E
	public float GetWeaponMaxDistance()
	{
		if (this.weapon != null)
		{
			return this.weapon.weaponMaxDistance;
		}
		return 0.9f;
	}

	// Token: 0x040003AD RID: 941
	public Weapon weapon;

	// Token: 0x040003AE RID: 942
	public List<WeaponSection> weaponSections;

	// Token: 0x040003AF RID: 943
	public BluntDamageDealerGameObject bluntDamageDealerGameObject;

	// Token: 0x040003B0 RID: 944
	public BladeDistanceItem currentBladeDistanceItem = new BladeDistanceItem
	{
		distanceBetweenPoints = 999f
	};
}
