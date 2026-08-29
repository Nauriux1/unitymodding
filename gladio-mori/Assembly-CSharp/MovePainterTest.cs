using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x0200025D RID: 605
public class MovePainterTest : MonoBehaviour
{
	// Token: 0x060011AA RID: 4522 RVA: 0x0005A84C File Offset: 0x00058A4C
	private void Start()
	{
		Application.targetFrameRate = -1;
		QualitySettings.vSyncCount = 0;
		if (base.transform.childCount > 0)
		{
			base.transform.GetChild(0).gameObject.SetActive(true);
		}
		Rigidbody componentInChildren = base.gameObject.GetComponentInChildren<Rigidbody>();
		if (componentInChildren != null)
		{
			componentInChildren.isKinematic = true;
		}
	}

	// Token: 0x060011AB RID: 4523 RVA: 0x0005A8A8 File Offset: 0x00058AA8
	private void Update()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(true);
		}
		if (this.noMovement)
		{
			return;
		}
		if (Keyboard.current.spaceKey.wasPressedThisFrame)
		{
			this.move = false;
			base.transform.Translate(this.moveVector * 0.3f, Space.World);
		}
		if (Keyboard.current.backspaceKey.wasPressedThisFrame)
		{
			this.move = false;
			base.transform.Translate((this.moveVector + Vector3.up).normalized * 0.5f, Space.World);
		}
		if (Keyboard.current.xKey.wasPressedThisFrame)
		{
			this.move = false;
			Vector3 translation = new Vector3(-0.1f, -0.07f, 0f);
			base.transform.Translate(translation, Space.World);
			this.moveVector = new Vector3(0f, -1f, 0f);
		}
		if (Keyboard.current.dKey.isPressed)
		{
			this.move = false;
			base.transform.Translate(Vector3.right * this.speed, Space.World);
			this.moveVector = Vector3.right;
		}
		if (Keyboard.current.aKey.isPressed)
		{
			this.move = false;
			base.transform.Translate(Vector3.right * -this.speed, Space.World);
			this.moveVector = Vector3.right * -1f;
		}
		if (Keyboard.current.wKey.isPressed)
		{
			this.move = false;
			base.transform.Translate(Vector3.up * this.speed, Space.World);
			this.moveVector = Vector3.up;
		}
		if (Keyboard.current.sKey.isPressed)
		{
			this.move = false;
			base.transform.Translate(Vector3.up * -this.speed, Space.World);
			this.moveVector = Vector3.up * -1f;
		}
		if (Keyboard.current.rKey.isPressed)
		{
			this.move = false;
			base.transform.Translate(Vector3.forward * this.speed, Space.World);
			this.moveVector = Vector3.forward;
		}
		if (Keyboard.current.fKey.isPressed)
		{
			this.move = false;
			base.transform.Translate(Vector3.forward * -this.speed, Space.World);
			this.moveVector = Vector3.forward * -1f;
		}
		if (Keyboard.current.gKey.wasPressedThisFrame)
		{
			foreach (object obj in base.transform.transform)
			{
				((Transform)obj).Rotate(new Vector3(0f, 0f, 90f), Space.World);
			}
		}
	}

	// Token: 0x04000D4C RID: 3404
	private Vector3 moveVector = Vector3.right;

	// Token: 0x04000D4D RID: 3405
	public bool move;

	// Token: 0x04000D4E RID: 3406
	public bool noMovement;

	// Token: 0x04000D4F RID: 3407
	public float speed = 0.002f;
}
