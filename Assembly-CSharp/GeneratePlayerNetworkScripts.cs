using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using MoveClasses;
using UnityEngine;

// Token: 0x0200019B RID: 411
public class GeneratePlayerNetworkScripts : MonoBehaviour
{
	// Token: 0x06000CD7 RID: 3287 RVA: 0x00040C34 File Offset: 0x0003EE34
	private void Start()
	{
		this.count = 0;
		this.transformCount = 0;
		this.mainObject = base.gameObject;
	}

	// Token: 0x06000CD8 RID: 3288 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x00040C50 File Offset: 0x0003EE50
	public void DOSHIT()
	{
		this.count = 0;
		this.transformCount = 0;
		this.damageableBodyPartCount = 0;
		this.mainObject = base.gameObject;
		this.DeleteScripts();
		this.DeleteAll(this.mainObject);
		this.mainObject.AddComponent<NetworkIdentity>();
		this.playerHealth = this.mainObject.GetComponent<PlayerHealth>();
		this.playerHealth.onlyPhysicalByDefault = true;
		this.GenerateNetworkScriptsForPlayer(this.mainObject);
		this.AddWeaponDamageableScripts();
		this.GenerateHands();
		Debug.Log("Transforms:" + this.transformCount.ToString());
		this.GenerateAndLinkPlayerHealthMultiplayer();
		this.AddCuttableScripts();
		MultiplayerPlayerSetup multiplayerPlayerSetup = this.mainObject.GetComponent<MultiplayerPlayerSetup>();
		if (multiplayerPlayerSetup == null)
		{
			multiplayerPlayerSetup = this.mainObject.AddComponent<MultiplayerPlayerSetup>();
		}
		multiplayerPlayerSetup.FillList();
	}

	// Token: 0x06000CDA RID: 3290 RVA: 0x00040D1E File Offset: 0x0003EF1E
	public void DeleteMultiplayerShit()
	{
		this.mainObject = base.gameObject;
		this.DeleteScripts();
		this.DeleteAll(this.mainObject);
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x00040D40 File Offset: 0x0003EF40
	private void DeleteScripts()
	{
		foreach (WeaponDamageablePartMultiplayer obj in UnityEngine.Object.FindObjectsOfType<WeaponDamageablePartMultiplayer>().ToList<WeaponDamageablePartMultiplayer>())
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
		foreach (WeaponDamageablePartMultiplayerHandler obj2 in UnityEngine.Object.FindObjectsOfType<WeaponDamageablePartMultiplayerHandler>().ToList<WeaponDamageablePartMultiplayerHandler>())
		{
			UnityEngine.Object.DestroyImmediate(obj2);
		}
		foreach (CuttableMultiplayerHandler obj3 in UnityEngine.Object.FindObjectsOfType<CuttableMultiplayerHandler>().ToList<CuttableMultiplayerHandler>())
		{
			UnityEngine.Object.DestroyImmediate(obj3);
		}
		foreach (MultiplayerPlayerSetup obj4 in UnityEngine.Object.FindObjectsOfType<MultiplayerPlayerSetup>().ToList<MultiplayerPlayerSetup>())
		{
			UnityEngine.Object.DestroyImmediate(obj4);
		}
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x00040E60 File Offset: 0x0003F060
	private void AddWeaponDamageableScripts()
	{
		WeaponDamageablePartMultiplayerHandler weaponDamageablePartMultiplayerHandler = this.mainObject.AddComponent<WeaponDamageablePartMultiplayerHandler>();
		weaponDamageablePartMultiplayerHandler.weaponDamageableParts = new List<WeaponDamageablePart>();
		foreach (WeaponDamageablePart weaponDamageablePart in UnityEngine.Object.FindObjectsOfType<WeaponDamageablePart>().ToList<WeaponDamageablePart>())
		{
			weaponDamageablePart.weaponDamageablePartMultiplayerHandler = weaponDamageablePartMultiplayerHandler;
			weaponDamageablePartMultiplayerHandler.weaponDamageableParts.Add(weaponDamageablePart);
			this.damageableBodyPartCount++;
		}
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x00040EE8 File Offset: 0x0003F0E8
	private void AddCuttableScripts()
	{
		CuttableMultiplayerHandler cuttableMultiplayerHandler = this.mainObject.AddComponent<CuttableMultiplayerHandler>();
		cuttableMultiplayerHandler.cuttableGameObjects = new CuttableGameObject[Enum.GetNames(typeof(JointType)).Length];
		foreach (CuttableGameObject cuttableGameObject in from x in UnityEngine.Object.FindObjectsOfType<CuttableGameObject>().ToList<CuttableGameObject>()
		orderby (int)x.bodyPart
		select x)
		{
			cuttableGameObject.cuttableMultiplayerHandler = cuttableMultiplayerHandler;
			cuttableMultiplayerHandler.cuttableGameObjects[(int)cuttableGameObject.bodyPart] = cuttableGameObject;
		}
		this.playerHealth.GetComponent<PlayerHealthMultiplayer>().cuttableMultiplayerHandler = cuttableMultiplayerHandler;
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x00040FA4 File Offset: 0x0003F1A4
	public void GenerateNetworkScriptsForPlayer(GameObject currentGameObject)
	{
		foreach (object obj in currentGameObject.transform)
		{
			Transform transform = (Transform)obj;
			if (!(transform.name == "TargetBoxer") && (!(currentGameObject.name == "ActualBoxer") || !(transform.name != "Armature")))
			{
				this.GenerateScripts(transform.gameObject);
				this.GenerateNetworkScriptsForPlayer(transform.gameObject);
				this.count++;
				Debug.Log(this.count);
			}
		}
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x00041064 File Offset: 0x0003F264
	private void GenerateScripts(GameObject currentGameObject)
	{
		if (currentGameObject.GetComponent<Rigidbody>() != null && currentGameObject.name != "PlayerModelPhysics" && currentGameObject.activeInHierarchy)
		{
			Debug.Log(currentGameObject.name);
			MultiplayerTransform multiplayerTransform = this.mainObject.AddComponent<MultiplayerTransform>();
			multiplayerTransform.target = currentGameObject.transform;
			multiplayerTransform.syncInterval = 0f;
			multiplayerTransform.positionPrecision = 0.001f;
			multiplayerTransform.rotationSensitivity = 1E-06f;
			multiplayerTransform.interpolatePosition = true;
			multiplayerTransform.syncScale = false;
			multiplayerTransform.syncPosition = false;
			multiplayerTransform.onlySyncOnChange = true;
			multiplayerTransform.compressRotation = false;
			multiplayerTransform.onlySyncOnChangeCorrectionMultiplier = 1.5f;
			this.transformCount++;
			if (currentGameObject.name == "HIP")
			{
				Debug.Log("Hip found");
				multiplayerTransform.syncPosition = true;
				multiplayerTransform.positionPrecision = 1E-06f;
				multiplayerTransform.coordinateSpace = CoordinateSpace.World;
				this.existingPlayerName = GameObject.Find("PlayerName(Clone)");
				if (this.existingPlayerName == null)
				{
					this.existingPlayerName = UnityEngine.Object.Instantiate<GameObject>(this.playerNamePrefab, currentGameObject.transform);
				}
				if (this.playerHealth != null)
				{
					this.playerHealth.playerNameTextMesh = this.existingPlayerName.GetComponent<TextMesh>();
				}
			}
		}
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x000411B0 File Offset: 0x0003F3B0
	private void GenerateAndLinkPlayerHealthMultiplayer()
	{
		PlayerHealthMultiplayer playerHealthMultiplayer = this.playerHealth.gameObject.GetComponent<PlayerHealthMultiplayer>();
		if (playerHealthMultiplayer == null)
		{
			playerHealthMultiplayer = this.playerHealth.gameObject.AddComponent<PlayerHealthMultiplayer>();
			this.playerHealth.playerHealthMultiplayer = playerHealthMultiplayer;
			playerHealthMultiplayer.playerHealth = this.playerHealth;
		}
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x00041200 File Offset: 0x0003F400
	private void GenerateHands()
	{
		List<HandMultiplayer> list = new List<HandMultiplayer>();
		foreach (Hand hand in UnityEngine.Object.FindObjectsOfType<Hand>())
		{
			HandMultiplayer handMultiplayer = this.mainObject.AddComponent<HandMultiplayer>();
			MultiplayerTransform multiplayerTransform = this.mainObject.AddComponent<MultiplayerTransform>();
			multiplayerTransform.syncInterval = 0f;
			multiplayerTransform.positionPrecision = 0.001f;
			multiplayerTransform.rotationSensitivity = 1E-06f;
			multiplayerTransform.interpolatePosition = true;
			multiplayerTransform.onlySyncOnChange = true;
			multiplayerTransform.compressRotation = false;
			multiplayerTransform.onlySyncOnChangeCorrectionMultiplier = 1.5f;
			multiplayerTransform.syncScale = false;
			hand.handMultiplayer = handMultiplayer;
			handMultiplayer.hand = hand;
			handMultiplayer.itemTransform = multiplayerTransform;
			multiplayerTransform.target = hand.placeholderGameObject.transform;
			list.Add(handMultiplayer);
			this.transformCount++;
		}
		if (list.Count == 2)
		{
			list[0].otherHandMultiplayer = list[1];
			list[1].otherHandMultiplayer = list[0];
			return;
		}
		throw new Exception("Wrong amount of hands");
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x00041318 File Offset: 0x0003F518
	private void DeleteAll(GameObject currentGameObject)
	{
		foreach (object obj in currentGameObject.transform)
		{
			Transform transform = (Transform)obj;
			if (!(transform.name == "PlayerModelAnimation") && (!(currentGameObject.name == "ActualBoxer") || !(transform.name != "Armature")))
			{
				this.DeleteAll(transform.gameObject);
			}
		}
		this.DeleteAllComponents(currentGameObject);
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x000413B4 File Offset: 0x0003F5B4
	private void DeleteAllComponents(GameObject currentGameObject)
	{
		foreach (HandMultiplayer obj in currentGameObject.GetComponents<HandMultiplayer>().ToList<HandMultiplayer>())
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
		foreach (NetworkTransformReliable obj2 in currentGameObject.GetComponents<NetworkTransformReliable>().ToList<NetworkTransformReliable>())
		{
			UnityEngine.Object.DestroyImmediate(obj2);
		}
		foreach (NetworkTransformReliable obj3 in currentGameObject.GetComponents<NetworkTransformReliable>().ToList<NetworkTransformReliable>())
		{
			UnityEngine.Object.DestroyImmediate(obj3);
		}
		foreach (NetworkIdentity obj4 in currentGameObject.GetComponents<NetworkIdentity>().ToList<NetworkIdentity>())
		{
			UnityEngine.Object.DestroyImmediate(obj4);
		}
		foreach (PlayerHealthMultiplayer obj5 in currentGameObject.GetComponents<PlayerHealthMultiplayer>().ToList<PlayerHealthMultiplayer>())
		{
			UnityEngine.Object.DestroyImmediate(obj5);
		}
	}

	// Token: 0x04000931 RID: 2353
	private GameObject mainObject;

	// Token: 0x04000932 RID: 2354
	public int count;

	// Token: 0x04000933 RID: 2355
	public int transformCount;

	// Token: 0x04000934 RID: 2356
	public GameObject playerNamePrefab;

	// Token: 0x04000935 RID: 2357
	public GameObject existingPlayerName;

	// Token: 0x04000936 RID: 2358
	public PlayerHealth playerHealth;

	// Token: 0x04000937 RID: 2359
	public int damageableBodyPartCount;
}
