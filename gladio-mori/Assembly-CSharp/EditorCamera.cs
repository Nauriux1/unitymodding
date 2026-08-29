using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utils;

// Token: 0x0200012D RID: 301
public class EditorCamera : MonoBehaviour
{
	// Token: 0x06000959 RID: 2393 RVA: 0x0002C839 File Offset: 0x0002AA39
	private void Awake()
	{
		this.InitCamera();
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x0002C844 File Offset: 0x0002AA44
	private void InitCamera()
	{
		this.editorCamera = base.gameObject;
		this.editorCameraTarget = new GameObject("editorCameraTarget").transform;
		this.editorCameraTarget.transform.position = this.focusedEditorCameraTarget.transform.position;
		this.editorCamera.transform.position = this.editorCameraTarget.position;
		this.editorCamera.transform.Translate(new Vector3(0f, 0f, this.activeZoomLevel));
		this.userControls = SettingsHelper.GetUserControls();
		this.userControls.Generic.Enable();
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x0002C8F0 File Offset: 0x0002AAF0
	private void Update()
	{
		this.HandleCameraMove();
		if (Mouse.current.scroll.ReadValue().y != 0f)
		{
			this.activeZoomLevel += Mouse.current.scroll.ReadValue().y * this.GetCameraZoomMultiplier();
			this.activeZoomLevel = Mathf.Min((float)this.maxZoomLevel, this.activeZoomLevel);
			this.activeZoomLevel = Mathf.Max((float)this.minZoomLevel, this.activeZoomLevel);
		}
		if (this.userControls.Generic.Left_Click_Modifier_Or_Middle.WasPressedThisFrame())
		{
			if (!EventSystem.current.IsPointerOverGameObject())
			{
				this.rotatingCamera = true;
			}
		}
		else if (this.userControls.Generic.Left_Click_Modifier_Or_Middle.IsPressed())
		{
			if (this.rotatingCamera)
			{
				Vector2 vector = Mouse.current.position.ReadValueFromPreviousFrame() - Mouse.current.position.ReadValue();
				vector.y /= (float)Screen.height;
				vector.x /= (float)Screen.width;
				this.editorCamera.transform.position = this.editorCameraTarget.position;
				this.editorCamera.transform.Rotate(new Vector3(1f, 0f, 0f), vector.y * 180f);
				this.editorCamera.transform.Rotate(new Vector3(0f, 1f, 0f), -vector.x * 180f, Space.World);
				this.editorCamera.transform.Translate(new Vector3(0f, 0f, this.activeZoomLevel));
			}
		}
		else if (this.userControls.Generic.Left_Click_Modifier_Or_Middle.WasReleasedThisFrame())
		{
			this.rotatingCamera = false;
		}
		this.editorCamera.transform.position = this.editorCameraTarget.position;
		this.editorCamera.transform.Translate(new Vector3(0f, 0f, this.activeZoomLevel));
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0002CB25 File Offset: 0x0002AD25
	private float GetCameraZoomMultiplier()
	{
		return this.defaultCameraZoomMultiplier;
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0002CB30 File Offset: 0x0002AD30
	private bool HandleCameraMove()
	{
		if (Mouse.current.rightButton.isPressed)
		{
			Vector2 vector = Mouse.current.position.ReadValueFromPreviousFrame() - Mouse.current.position.ReadValue();
			vector.y /= (float)Screen.height;
			vector.x /= (float)Screen.width;
			if (vector.magnitude != 0f)
			{
				this.editorCameraTarget.transform.Translate(new Vector3(vector.x * this.cameraMoveMultiplier, vector.y * this.cameraMoveMultiplier), this.editorCamera.transform);
			}
			return true;
		}
		return false;
	}

	// Token: 0x04000689 RID: 1673
	public GameObject editorCamera;

	// Token: 0x0400068A RID: 1674
	public Transform focusedEditorCameraTarget;

	// Token: 0x0400068B RID: 1675
	public Transform editorCameraTarget;

	// Token: 0x0400068C RID: 1676
	public float activeZoomLevel = -1.5f;

	// Token: 0x0400068D RID: 1677
	public UserControls userControls;

	// Token: 0x0400068E RID: 1678
	private int minZoomLevel = -5;

	// Token: 0x0400068F RID: 1679
	private int maxZoomLevel;

	// Token: 0x04000690 RID: 1680
	private bool rotatingCamera;

	// Token: 0x04000691 RID: 1681
	private float defaultCameraZoomMultiplier = 0.001f;

	// Token: 0x04000692 RID: 1682
	private float cameraMoveMultiplier = 10f;
}
