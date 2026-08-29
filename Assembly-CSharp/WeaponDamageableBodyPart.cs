using System;
using System.Collections.Generic;
using Mirror;
using MoveClasses;
using UnityEngine;

// Token: 0x0200015F RID: 351
public class WeaponDamageableBodyPart : MonoBehaviour, IWeaponDamageable, ILegacy
{
	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06000B30 RID: 2864 RVA: 0x00036329 File Offset: 0x00034529
	// (set) Token: 0x06000B31 RID: 2865 RVA: 0x00036331 File Offset: 0x00034531
	public List<BladePaintable> bladePaintables { get; set; } = new List<BladePaintable>();

	// Token: 0x06000B32 RID: 2866 RVA: 0x0000777A File Offset: 0x0000597A
	public void Destory(DamageOrigin? damageOrigin = null, bool playEffects = true)
	{
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x0000C7CC File Offset: 0x0000A9CC
	public bool IsBone()
	{
		return false;
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x0000C7CC File Offset: 0x0000A9CC
	public bool IsOrgan()
	{
		return false;
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x0003633A File Offset: 0x0003453A
	private void Start()
	{
		if (NetworkManager.singleton != null && NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)
		{
			this.disableLocalLogic = true;
		}
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x0003635D File Offset: 0x0003455D
	private void OnCollisionEnter(Collision collision)
	{
		if (this.disableLocalLogic)
		{
			return;
		}
		if (SoundManager.singleton != null)
		{
			SoundManager.singleton.PlaySoundForCollision(collision, base.gameObject, new SoundMaterialType?(SoundMaterialType.Player));
		}
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x0003638C File Offset: 0x0003458C
	public List<CuttableGameObject> GetCuttableGameObjects()
	{
		return this.cuttableGameObjects;
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06000B38 RID: 2872 RVA: 0x00036394 File Offset: 0x00034594
	public Rigidbody Rigidbody
	{
		get
		{
			Rigidbody result;
			if ((result = this._rigidbody) == null)
			{
				result = (this._rigidbody = base.gameObject.GetComponent<Rigidbody>());
			}
			return result;
		}
	}

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06000B39 RID: 2873 RVA: 0x000363BF File Offset: 0x000345BF
	// (set) Token: 0x06000B3A RID: 2874 RVA: 0x000363C7 File Offset: 0x000345C7
	public bool legacyInitialized { get; set; }

	// Token: 0x06000B3B RID: 2875 RVA: 0x000363D0 File Offset: 0x000345D0
	public void SetLegacy(bool legacy)
	{
		this.InitLegacy();
		if (legacy)
		{
			this.Rigidbody.drag = this.legacyDrag;
			return;
		}
		this.Rigidbody.drag = this.normalDrag;
	}

	// Token: 0x06000B3C RID: 2876 RVA: 0x000363FE File Offset: 0x000345FE
	public void InitLegacy()
	{
		if (!this.legacyInitialized)
		{
			this.normalDrag = this.Rigidbody.drag;
			this.legacyInitialized = true;
		}
	}

	// Token: 0x06000B3D RID: 2877 RVA: 0x0002F596 File Offset: 0x0002D796
	public bool LegacyItemExists()
	{
		return base.enabled;
	}

	// Token: 0x040007C3 RID: 1987
	public JointType bodyPart;

	// Token: 0x040007C4 RID: 1988
	public PlayerHealth player;

	// Token: 0x040007C6 RID: 1990
	public List<CuttableGameObject> cuttableGameObjects;

	// Token: 0x040007C7 RID: 1991
	public bool disableLocalLogic;

	// Token: 0x040007C8 RID: 1992
	private Rigidbody _rigidbody;

	// Token: 0x040007CA RID: 1994
	private float legacyDrag;

	// Token: 0x040007CB RID: 1995
	private float normalDrag;

	// Token: 0x040007CC RID: 1996
	public List<WeaponDamageablePart> childWeaponDamageableParts;
}
