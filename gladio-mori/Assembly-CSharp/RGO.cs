using System;
using System.Collections.Generic;
using MoveClasses;
using Newtonsoft.Json;
using ProtoBuf;
using UnityEngine;
using Utils;

// Token: 0x020000D3 RID: 211
[ProtoContract]
[Serializable]
public class RGO
{
	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000771 RID: 1905 RVA: 0x0002582D File Offset: 0x00023A2D
	// (set) Token: 0x06000772 RID: 1906 RVA: 0x00025835 File Offset: 0x00023A35
	[ProtoMember(1)]
	public string name { get; set; }

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000773 RID: 1907 RVA: 0x0002583E File Offset: 0x00023A3E
	// (set) Token: 0x06000774 RID: 1908 RVA: 0x00025846 File Offset: 0x00023A46
	[ProtoMember(2)]
	public string prefabName { get; set; }

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000775 RID: 1909 RVA: 0x0002584F File Offset: 0x00023A4F
	// (set) Token: 0x06000776 RID: 1910 RVA: 0x00025857 File Offset: 0x00023A57
	[JsonIgnore]
	public GameObject gameObject { get; set; }

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06000777 RID: 1911 RVA: 0x00025860 File Offset: 0x00023A60
	// (set) Token: 0x06000778 RID: 1912 RVA: 0x00025868 File Offset: 0x00023A68
	[ProtoMember(3)]
	public List<EquippedEquipment> equippedEquipment { get; set; } = new List<EquippedEquipment>();

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000779 RID: 1913 RVA: 0x00025871 File Offset: 0x00023A71
	// (set) Token: 0x0600077A RID: 1914 RVA: 0x00025879 File Offset: 0x00023A79
	[ProtoMember(4)]
	public List<RCGO> recordableChildGameObject { get; set; } = new List<RCGO>();

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x0600077B RID: 1915 RVA: 0x00025882 File Offset: 0x00023A82
	// (set) Token: 0x0600077C RID: 1916 RVA: 0x0002588A File Offset: 0x00023A8A
	[ProtoMember(5)]
	public List<RDP> recordableDamageablePart { get; set; } = new List<RDP>();

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x0600077D RID: 1917 RVA: 0x00025893 File Offset: 0x00023A93
	// (set) Token: 0x0600077E RID: 1918 RVA: 0x0002589B File Offset: 0x00023A9B
	[ProtoMember(6)]
	public int startTick { get; set; }

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x0600077F RID: 1919 RVA: 0x000258A4 File Offset: 0x00023AA4
	// (set) Token: 0x06000780 RID: 1920 RVA: 0x000258AC File Offset: 0x00023AAC
	[ProtoMember(7)]
	public DE deathEvent { get; set; }

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000781 RID: 1921 RVA: 0x000258B5 File Offset: 0x00023AB5
	// (set) Token: 0x06000782 RID: 1922 RVA: 0x000258BD File Offset: 0x00023ABD
	[ProtoMember(8)]
	public List<RFC> recordableFullCuts { get; set; } = new List<RFC>();

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000783 RID: 1923 RVA: 0x000258C6 File Offset: 0x00023AC6
	// (set) Token: 0x06000784 RID: 1924 RVA: 0x000258CE File Offset: 0x00023ACE
	[ProtoMember(9)]
	public List<RBH> recordableBluntHits { get; set; } = new List<RBH>();

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x06000785 RID: 1925 RVA: 0x000258D7 File Offset: 0x00023AD7
	// (set) Token: 0x06000786 RID: 1926 RVA: 0x000258DF File Offset: 0x00023ADF
	[ProtoMember(10)]
	public byte[] customTexture { get; set; }

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06000787 RID: 1927 RVA: 0x000258E8 File Offset: 0x00023AE8
	// (set) Token: 0x06000788 RID: 1928 RVA: 0x000258F0 File Offset: 0x00023AF0
	[ProtoMember(11)]
	public bool isLocalPlayer { get; set; }

	// Token: 0x06000789 RID: 1929 RVA: 0x000258FC File Offset: 0x00023AFC
	public void Activate()
	{
		this.gameObject.SetActive(true);
		if (this.cutActivationItem == null)
		{
			return;
		}
		if (this.cutActivationItem.CutItem != null && this.cutActivationItem.cutSections != null)
		{
			if (this.cutActivationItem.CutItem.newCuttableGameObject != null)
			{
				this.cutActivationItem.CutItem.newCuttableGameObject.transform.localPosition = default(Vector3);
				this.cutActivationItem.CutItem.newCuttableGameObject.transform.localRotation = default(Quaternion);
			}
			this.cutActivationItem.CutItem.RedoCuttableSections(this.cutActivationItem.cutSections);
		}
		foreach (CuttableMesh cuttableMesh in this.cutActivationItem.originalCuttableMeshs)
		{
			cuttableMesh.renderer.enabled = false;
		}
		foreach (CuttableMesh cuttableMesh2 in this.cutActivationItem.newCuttableMeshs)
		{
			cuttableMesh2.renderer.enabled = true;
		}
		foreach (WeaponDamageableArteryCut weaponDamageableArteryCut in this.cutActivationItem.arteryCuts)
		{
			weaponDamageableArteryCut.WeaponDamageablePart.TryToSetEffectPosition(weaponDamageableArteryCut.newParent.gameObject, weaponDamageableArteryCut.newPosition, weaponDamageableArteryCut.newRotation, new JointType?(weaponDamageableArteryCut.newBodypart), false);
		}
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x00025AC0 File Offset: 0x00023CC0
	public void Deactivate(bool fullDeactivation = false)
	{
		this.gameObject.SetActive(false);
		if (this.cutActivationItem == null)
		{
			return;
		}
		if (this.cutActivationItem.CutItem != null && this.cutActivationItem.cutSections != null)
		{
			this.cutActivationItem.CutItem.UndoCuttableSections(this.cutActivationItem.cutSections);
		}
		if (fullDeactivation)
		{
			foreach (CuttableMesh cuttableMesh in this.cutActivationItem.allCuttableMeshs)
			{
				cuttableMesh.renderer.enabled = true;
			}
		}
		foreach (CuttableMesh cuttableMesh2 in this.cutActivationItem.originalCuttableMeshs)
		{
			cuttableMesh2.renderer.enabled = true;
		}
		foreach (CuttableMesh cuttableMesh3 in this.cutActivationItem.newCuttableMeshs)
		{
			cuttableMesh3.renderer.enabled = false;
		}
		foreach (WeaponDamageableArteryCut weaponDamageableArteryCut in this.cutActivationItem.arteryCuts)
		{
			weaponDamageableArteryCut.WeaponDamageablePart.TryToSetEffectPosition(weaponDamageableArteryCut.oldParent.gameObject, weaponDamageableArteryCut.oldPosition, weaponDamageableArteryCut.oldRotation, weaponDamageableArteryCut.oldBodypart, true);
			if (!weaponDamageableArteryCut.oldBloodFlow)
			{
				weaponDamageableArteryCut.WeaponDamageablePart.StopDestroyVisuals();
			}
		}
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x00025C78 File Offset: 0x00023E78
	public void SetCustomTexture()
	{
		try
		{
			ReplayTexturesOverrideType replayTexturesOverrideType = SettingsHelper.GetReplayTexturesOverrideType();
			if (replayTexturesOverrideType != ReplayTexturesOverrideType.NoCustomTextures)
			{
				if (replayTexturesOverrideType == ReplayTexturesOverrideType.UseLocalPlayerTexture)
				{
					this.playerHealth.SetPlayerTexture(SettingsHelper.GetCustomPlayerTexture());
				}
				else if (!(this.playerHealth == null) && this.customTexture != null && this.customTexture.Length != 0)
				{
					Texture2D texture2D = new Texture2D(2, 2);
					texture2D.LoadImage(this.customTexture);
					this.playerHealth.SetPlayerTexture(texture2D);
				}
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x040004F2 RID: 1266
	[JsonIgnore]
	public List<WeaponDamageablePart> weaponDamageableParts;

	// Token: 0x040004F3 RID: 1267
	[JsonIgnore]
	public bool isPlayer;

	// Token: 0x040004F4 RID: 1268
	[JsonIgnore]
	public PlayerHealth playerHealth;

	// Token: 0x040004F5 RID: 1269
	public ReplayCutActivationItem cutActivationItem;
}
