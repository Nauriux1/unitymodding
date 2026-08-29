using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utils;

// Token: 0x0200019D RID: 413
public class JointStrengthTool : MonoBehaviour
{
	// Token: 0x06000CE8 RID: 3304 RVA: 0x00041530 File Offset: 0x0003F730
	private void Start()
	{
		if (JointStrengthTool.singleton == null)
		{
			this.InitializeJointStrengthTool();
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x00041551 File Offset: 0x0003F751
	private void Update()
	{
		if (Keyboard.current.f10Key.wasPressedThisFrame)
		{
			this.ToggleToolVisible();
		}
		if (this.updateJointStrengths)
		{
			this.updateJointStrengths = false;
			this.UpdateJointStrengths();
		}
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x00041580 File Offset: 0x0003F780
	private void InitializeJointStrengthTool()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		JointStrengthTool.singleton = this;
		IEnumerable<JointStrength> source = this.playerPrefab.GetComponentsInChildren<JointStrength>().ToList<JointStrength>();
		JointStrengthToolItem jointStrengthToolItem = new JointStrengthToolItem
		{
			jointName = "BASE",
			maxStrength = 0f,
			maxDamper = 0f,
			jointMaximumForceMultiplier = 0f
		};
		this.jointStrengthToolItems.Add(jointStrengthToolItem);
		JointStrengthToolListItem component = UnityEngine.Object.Instantiate<GameObject>(this.jointStrengthToolListItemPrefab, this.listHolder).GetComponent<JointStrengthToolListItem>();
		jointStrengthToolItem.jointStrengthToolListItem = component;
		component.jointStrengthToolItem = jointStrengthToolItem;
		component.UpdateUI();
		JointStrengthToolItem jointStrengthToolItem2 = new JointStrengthToolItem
		{
			jointName = "MAGIC_HIP",
			maxStrength = 10000f,
			maxDamper = 2700f
		};
		ConfigurableJoint configurableJoint = (from x in this.playerPrefab.GetComponentsInChildren<ConfigurableJoint>()
		where x.connectedBody.gameObject.name == "HipSphere"
		select x).FirstOrDefault<ConfigurableJoint>();
		if (configurableJoint != null)
		{
			jointStrengthToolItem2.maxStrength = configurableJoint.angularXDrive.positionSpring;
			jointStrengthToolItem2.maxDamper = configurableJoint.angularXDrive.positionDamper;
		}
		this.jointStrengthToolItems.Add(jointStrengthToolItem2);
		JointStrengthToolListItem component2 = UnityEngine.Object.Instantiate<GameObject>(this.jointStrengthToolListItemPrefab, this.listHolder).GetComponent<JointStrengthToolListItem>();
		jointStrengthToolItem2.jointStrengthToolListItem = component2;
		component2.jointStrengthToolItem = jointStrengthToolItem2;
		component2.UpdateUI();
		foreach (JointStrength jointStrength in from x in source
		orderby x.jointName
		select x)
		{
			JointStrengthToolItem jointStrengthToolItem3 = new JointStrengthToolItem
			{
				jointName = jointStrength.jointName,
				maxStrength = jointStrength.joint.angularXDrive.positionSpring,
				maxDamper = jointStrength.joint.angularXDrive.positionDamper
			};
			if (!jointStrengthToolItem3.jointName.Contains("_RIGHT"))
			{
				this.jointStrengthToolItems.Add(jointStrengthToolItem3);
				JointStrengthToolListItem component3 = UnityEngine.Object.Instantiate<GameObject>(this.jointStrengthToolListItemPrefab, this.listHolder).GetComponent<JointStrengthToolListItem>();
				jointStrengthToolItem3.jointStrengthToolListItem = component3;
				component3.jointStrengthToolItem = jointStrengthToolItem3;
				component3.UpdateUI();
			}
		}
		this.Load();
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x000417E8 File Offset: 0x0003F9E8
	public void UpdateJointStrengths()
	{
		List<JointStrength> source = UnityEngine.Object.FindObjectsOfType<JointStrength>().ToList<JointStrength>();
		JointStrengthToolItem jointStrengthToolItem2 = (from x in this.jointStrengthToolItems
		where x.jointName == "BASE"
		select x).First<JointStrengthToolItem>();
		using (List<JointStrengthToolItem>.Enumerator enumerator = this.jointStrengthToolItems.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				JointStrengthToolItem jointStrengthToolItem = enumerator.Current;
				float maximumForce = float.MaxValue;
				if (jointStrengthToolItem2.jointMaximumForceMultiplier > 0f)
				{
					maximumForce = jointStrengthToolItem.maxStrength * jointStrengthToolItem2.jointMaximumForceMultiplier;
				}
				if (jointStrengthToolItem.jointMaximumForceMultiplier > 0f)
				{
					maximumForce = jointStrengthToolItem.maxStrength * jointStrengthToolItem.jointMaximumForceMultiplier;
				}
				if (jointStrengthToolItem.jointName == "MAGIC_HIP")
				{
					using (List<ConfigurableJoint>.Enumerator enumerator2 = UnityEngine.Object.FindObjectsOfType<ConfigurableJoint>().ToList<ConfigurableJoint>().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ConfigurableJoint configurableJoint = enumerator2.Current;
							if (configurableJoint.connectedBody.gameObject.name == "HipSphere")
							{
								configurableJoint.angularXDrive = new JointDrive
								{
									positionDamper = jointStrengthToolItem.maxDamper,
									positionSpring = jointStrengthToolItem.maxStrength,
									maximumForce = maximumForce
								};
								configurableJoint.angularYZDrive = new JointDrive
								{
									positionDamper = jointStrengthToolItem.maxDamper,
									positionSpring = jointStrengthToolItem.maxStrength,
									maximumForce = maximumForce
								};
							}
						}
						continue;
					}
				}
				List<JointStrength> list = (from x in source
				where x.jointName == jointStrengthToolItem.jointName
				select x).ToList<JointStrength>();
				if (jointStrengthToolItem.jointName.Contains("_LEFT"))
				{
					list.AddRange(from x in source
					where x.jointName == jointStrengthToolItem.jointName.Replace("_LEFT", "_RIGHT")
					select x);
				}
				foreach (JointStrength jointStrength in list)
				{
					jointStrength.totalMaxPower = jointStrengthToolItem.maxStrength;
					jointStrength.totalMaxDamper = jointStrengthToolItem.maxDamper;
					jointStrength.SetStrengthPercent(100f);
					jointStrength.joint.angularXDrive = new JointDrive
					{
						positionDamper = jointStrengthToolItem.maxDamper,
						positionSpring = jointStrengthToolItem.maxStrength,
						maximumForce = maximumForce
					};
					jointStrength.joint.angularYZDrive = new JointDrive
					{
						positionDamper = jointStrengthToolItem.maxDamper,
						positionSpring = jointStrengthToolItem.maxStrength,
						maximumForce = maximumForce
					};
				}
			}
		}
	}

	// Token: 0x06000CEC RID: 3308 RVA: 0x00041B2C File Offset: 0x0003FD2C
	private void ToggleToolVisible()
	{
		if (this.jointStrengthToolCanvas.gameObject.activeInHierarchy)
		{
			this.jointStrengthToolCanvas.gameObject.SetActive(false);
			GeneralManager.singleton.UpdateCursorState();
			return;
		}
		this.jointStrengthToolCanvas.gameObject.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
	}

	// Token: 0x06000CED RID: 3309 RVA: 0x00041B80 File Offset: 0x0003FD80
	public void Save()
	{
		string path = SettingsHelper.GetUserSavePath() + "/JointStrengthTool.json";
		string contents = JsonConvert.SerializeObject(this.jointStrengthToolItems);
		File.WriteAllText(path, contents);
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x00041BB0 File Offset: 0x0003FDB0
	public void Load()
	{
		string filePath = SettingsHelper.GetUserSavePath() + "/JointStrengthTool.json";
		try
		{
			string value = Generic.LoadJsonFromFile(filePath);
			if (!string.IsNullOrEmpty(value))
			{
				List<JointStrengthToolItem> list = JsonConvert.DeserializeObject<List<JointStrengthToolItem>>(value);
				foreach (JointStrengthToolItem jointStrengthToolItem in this.jointStrengthToolItems)
				{
					foreach (JointStrengthToolItem jointStrengthToolItem2 in list)
					{
						if (jointStrengthToolItem.jointName == jointStrengthToolItem2.jointName)
						{
							jointStrengthToolItem.maxStrength = jointStrengthToolItem2.maxStrength;
							jointStrengthToolItem.maxDamper = jointStrengthToolItem2.maxDamper;
							jointStrengthToolItem.jointMaximumForceMultiplier = jointStrengthToolItem2.jointMaximumForceMultiplier;
							jointStrengthToolItem.jointStrengthToolListItem.UpdateUI();
							break;
						}
					}
				}
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x00041CC8 File Offset: 0x0003FEC8
	public void ResetStrengths()
	{
		IEnumerable<JointStrength> source = this.playerPrefab.GetComponentsInChildren<JointStrength>().ToList<JointStrength>();
		List<JointStrengthToolItem> list = new List<JointStrengthToolItem>();
		foreach (JointStrength jointStrength in from x in source
		orderby x.jointName
		select x)
		{
			JointStrengthToolItem jointStrengthToolItem = new JointStrengthToolItem
			{
				jointName = jointStrength.jointName,
				maxStrength = jointStrength.joint.angularXDrive.positionSpring,
				maxDamper = jointStrength.joint.angularXDrive.positionDamper
			};
			if (!jointStrengthToolItem.jointName.Contains("_RIGHT"))
			{
				list.Add(jointStrengthToolItem);
			}
		}
		JointStrengthToolItem item = new JointStrengthToolItem
		{
			jointName = "BASE",
			maxStrength = 0f,
			maxDamper = 0f
		};
		list.Add(item);
		JointStrengthToolItem jointStrengthToolItem2 = new JointStrengthToolItem
		{
			jointName = "MAGIC_HIP",
			maxStrength = 10000f,
			maxDamper = 2700f
		};
		ConfigurableJoint configurableJoint = (from x in this.playerPrefab.GetComponentsInChildren<ConfigurableJoint>()
		where x.connectedBody.gameObject.name == "HipSphere"
		select x).FirstOrDefault<ConfigurableJoint>();
		if (configurableJoint != null)
		{
			jointStrengthToolItem2.maxStrength = configurableJoint.angularXDrive.positionSpring;
			jointStrengthToolItem2.maxDamper = configurableJoint.angularXDrive.positionDamper;
		}
		list.Add(jointStrengthToolItem2);
		foreach (JointStrengthToolItem jointStrengthToolItem3 in this.jointStrengthToolItems)
		{
			foreach (JointStrengthToolItem jointStrengthToolItem4 in list)
			{
				if (jointStrengthToolItem3.jointName == jointStrengthToolItem4.jointName)
				{
					jointStrengthToolItem3.maxStrength = jointStrengthToolItem4.maxStrength;
					jointStrengthToolItem3.maxDamper = jointStrengthToolItem4.maxDamper;
					jointStrengthToolItem3.jointMaximumForceMultiplier = 0f;
					jointStrengthToolItem3.jointStrengthToolListItem.UpdateUI();
					break;
				}
			}
		}
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x00041F38 File Offset: 0x00040138
	private void OnEnable()
	{
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x00041F4B File Offset: 0x0004014B
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x00041F5E File Offset: 0x0004015E
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (!scene.name.ToLower().Contains("mainmenu"))
		{
			this.updateJointStrengths = true;
		}
	}

	// Token: 0x0400093A RID: 2362
	public static JointStrengthTool singleton;

	// Token: 0x0400093B RID: 2363
	public GameObject playerPrefab;

	// Token: 0x0400093C RID: 2364
	public GameObject jointStrengthToolListItemPrefab;

	// Token: 0x0400093D RID: 2365
	public List<JointStrengthToolItem> jointStrengthToolItems = new List<JointStrengthToolItem>();

	// Token: 0x0400093E RID: 2366
	public Transform listHolder;

	// Token: 0x0400093F RID: 2367
	public Canvas jointStrengthToolCanvas;

	// Token: 0x04000940 RID: 2368
	private bool updateJointStrengths;
}
