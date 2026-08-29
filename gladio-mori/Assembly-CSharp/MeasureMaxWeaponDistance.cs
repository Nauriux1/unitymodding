using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200018D RID: 397
public class MeasureMaxWeaponDistance : MonoBehaviour
{
	// Token: 0x06000C6D RID: 3181 RVA: 0x0003C6E1 File Offset: 0x0003A8E1
	private void Start()
	{
		if (MeasureMaxWeaponDistance.singleton != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		MeasureMaxWeaponDistance.singleton = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x0003C71E File Offset: 0x0003A91E
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		base.Invoke("FindParts", 1f);
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x0003C730 File Offset: 0x0003A930
	private void FindParts()
	{
		this.maxDistance = 0f;
		this.minDistance = 100f;
		this.playerHealth = null;
		this.distanceTransforms.Clear();
		PlayerInputManager playerInputManager = UnityEngine.Object.FindObjectOfType<PlayerInputManager>();
		if (playerInputManager != null)
		{
			PlayerAnimator playerAnimator = playerInputManager.playerAnimator;
			if (playerAnimator != null)
			{
				this.playerHealth = playerAnimator.player;
				Weapon weapon = null;
				if (this.playerHealth.leftHand.currentlyGrabbedItem != null)
				{
					weapon = this.playerHealth.leftHand.currentlyGrabbedItem.GetWeapon();
					if (weapon != null)
					{
						this.AddWeaponToDistancePoints(weapon);
					}
				}
				if (this.playerHealth.rightHand.currentlyGrabbedItem != null)
				{
					Weapon weapon2 = this.playerHealth.rightHand.currentlyGrabbedItem.GetWeapon();
					if (weapon2 != null && weapon2 != weapon)
					{
						this.AddWeaponToDistancePoints(weapon2);
					}
				}
			}
		}
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x0003C814 File Offset: 0x0003AA14
	private void AddWeaponToDistancePoints(Weapon weapon)
	{
		foreach (WeaponSection weaponSection in weapon.GetWeaponSections())
		{
			this.distanceTransforms.Add(weaponSection.point0);
			this.distanceTransforms.Add(weaponSection.point1);
		}
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x0003C884 File Offset: 0x0003AA84
	private void Update()
	{
		if (this.clearMaxDistance)
		{
			this.clearMaxDistance = false;
			this.maxDistance = 0f;
			this.minDistance = 100f;
		}
		this.MeasureDistance();
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x0003C8B4 File Offset: 0x0003AAB4
	private void MeasureDistance()
	{
		this.currentMaxDistance = 0f;
		this.currentMinDistance = 100f;
		if (this.playerHealth != null)
		{
			Transform transform = this.playerHealth.cameraPositionPoint.transform;
			foreach (Transform transform2 in this.distanceTransforms)
			{
				float num = Vector3.Distance(new Vector3(transform2.position.x, 0f, transform2.position.z), new Vector3(transform.position.x, 0f, transform.position.z));
				if (num > this.maxDistance)
				{
					this.maxDistance = num;
				}
				if (num < this.minDistance)
				{
					this.minDistance = num;
				}
				if (num > this.currentMaxDistance)
				{
					this.currentMaxDistance = num;
				}
				if (num < this.currentMinDistance)
				{
					this.currentMinDistance = num;
				}
			}
		}
	}

	// Token: 0x040008D4 RID: 2260
	public float maxDistance;

	// Token: 0x040008D5 RID: 2261
	public float minDistance;

	// Token: 0x040008D6 RID: 2262
	public float currentMaxDistance;

	// Token: 0x040008D7 RID: 2263
	public float currentMinDistance;

	// Token: 0x040008D8 RID: 2264
	public bool clearMaxDistance;

	// Token: 0x040008D9 RID: 2265
	public PlayerHealth playerHealth;

	// Token: 0x040008DA RID: 2266
	public List<Transform> distanceTransforms = new List<Transform>();

	// Token: 0x040008DB RID: 2267
	public static MeasureMaxWeaponDistance singleton;
}
