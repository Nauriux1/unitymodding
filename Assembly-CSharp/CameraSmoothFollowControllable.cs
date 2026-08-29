using System;
using UnityEngine;
using Utils;

// Token: 0x0200003A RID: 58
public class CameraSmoothFollowControllable : CameraSmoothFollow
{
	// Token: 0x060001CB RID: 459 RVA: 0x0000AC61 File Offset: 0x00008E61
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0000AC6C File Offset: 0x00008E6C
	public override void SetCameraSettings(PlayerCameraSettings playerCameraSettings, bool preview = false, GameObject previewPrefab = null)
	{
		base.SetCameraSettings(playerCameraSettings, preview, previewPrefab);
		this.cameraTurnSpeedMultiplier = SettingsHelper.GetControllerSensitivity();
		this.invertY = SettingsHelper.GetInvertCameraY();
		if (this.invertY)
		{
			this.invertYMultiplier = -1f;
		}
		else
		{
			this.invertYMultiplier = 1f;
		}
		this.timeScaleAffectCameraTurnSpeed = SettingsHelper.GetTimeScaleAffactsCameraTurnSpeed();
		this.playerTurnType = SettingsHelper.GetPlayerTurnType();
		this.CalculateClampValues();
		this.SetStartXRotationOffset();
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0000ACDA File Offset: 0x00008EDA
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x060001CE RID: 462 RVA: 0x0000ACE4 File Offset: 0x00008EE4
	protected override void LateUpdate()
	{
		float num = this.rotationInputLeft + this.rotationInputRight;
		float num2 = this.timeScaleAffectCameraTurnSpeed ? Time.deltaTime : Time.unscaledDeltaTime;
		if (!Generic.FloatEquals(0f, num))
		{
			this.rotationOffset.y = this.rotationOffset.y + num * this.cameraTurnSpeedMultiplier * num2;
			if (this.playerInputManager != null)
			{
				this.playerInputManager.UpdateTargetRotation();
			}
		}
		if (!Generic.FloatEquals(0f, this.verticalTurnSpeed))
		{
			this.AddToVerticalOffsetRotation(this.verticalTurnSpeed * this.cameraTurnSpeedMultiplier * this.invertYMultiplier * num2);
		}
		base.LateUpdate();
	}

	// Token: 0x060001CF RID: 463 RVA: 0x0000AD84 File Offset: 0x00008F84
	public void SetStartRotationOffset()
	{
		this.rotationOffset = this.target.rotation.eulerAngles;
		this.SetStartXRotationOffset();
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x0000ADB0 File Offset: 0x00008FB0
	public void SetStartXRotationOffset()
	{
		this.rotationOffset.x = Quaternion.LookRotation(this.targetOffset - this.cameraOffset).eulerAngles.x;
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x0000ADEC File Offset: 0x00008FEC
	public void RecalculateRotationOffset()
	{
		Vector3 eulerAngles = this.target.rotation.eulerAngles;
		this.rotationOffset.y = eulerAngles.y;
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x0000AE20 File Offset: 0x00009020
	protected override Quaternion GetNewRotation()
	{
		if (this.isPreviewing)
		{
			return this.target.rotation * this.calculatedFullRotationOffset;
		}
		if (!this.playerInputManager.targetRotationInUse)
		{
			return this.target.rotation * this.calculatedYRotationOffset * Quaternion.Euler(new Vector3(this.rotationOffset.x, 0f, 0f));
		}
		return this.calculatedYRotationOffset * Quaternion.Euler(this.rotationOffset);
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x0000AEAA File Offset: 0x000090AA
	public void SetMoveCameraVerticalTurn(float speed)
	{
		this.verticalTurnSpeed = speed * -1f;
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x0000AEB9 File Offset: 0x000090B9
	public void SetRotationInputLeft(float speed)
	{
		this.rotationInputLeft = speed;
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0000AEC2 File Offset: 0x000090C2
	public void SetRotationInputRight(float speed)
	{
		this.rotationInputRight = speed;
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x0000AEB9 File Offset: 0x000090B9
	public void SetRotationInput(float speed)
	{
		this.rotationInputLeft = speed;
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0000AECB File Offset: 0x000090CB
	public void TurnCameraHorizontal(float turnValue)
	{
		if (this.timeScaleAffectCameraTurnSpeed)
		{
			turnValue *= Time.timeScale;
		}
		this.rotationOffset.y = this.rotationOffset.y + turnValue;
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x0000AEEE File Offset: 0x000090EE
	public void TurnCameraVertical(float turnValue)
	{
		if (this.timeScaleAffectCameraTurnSpeed)
		{
			turnValue *= Time.timeScale;
		}
		this.AddToVerticalOffsetRotation(turnValue * this.invertYMultiplier);
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0000AF0F File Offset: 0x0000910F
	private void CalculateClampValues()
	{
		this.lowClamp = -89f;
		this.highClamp = 89f;
	}

	// Token: 0x060001DA RID: 474 RVA: 0x0000AF27 File Offset: 0x00009127
	private void AddToVerticalOffsetRotation(float turnValue)
	{
		this.rotationOffset.x = this.rotationOffset.x + turnValue;
		this.rotationOffset.x = Mathf.Clamp(this.rotationOffset.x, this.lowClamp, this.highClamp);
	}

	// Token: 0x0400011C RID: 284
	private float verticalTurnSpeed;

	// Token: 0x0400011D RID: 285
	private float cameraTurnSpeedMultiplier = 150f;

	// Token: 0x0400011E RID: 286
	private bool invertY;

	// Token: 0x0400011F RID: 287
	private bool timeScaleAffectCameraTurnSpeed = true;

	// Token: 0x04000120 RID: 288
	private float invertYMultiplier = 1f;

	// Token: 0x04000121 RID: 289
	private float rotationInputLeft;

	// Token: 0x04000122 RID: 290
	private float rotationInputRight;

	// Token: 0x04000123 RID: 291
	public IPlayerInputManager playerInputManager;

	// Token: 0x04000124 RID: 292
	public float lowClamp = -90f;

	// Token: 0x04000125 RID: 293
	public float highClamp = 90f;

	// Token: 0x04000126 RID: 294
	private PlayerTurnType playerTurnType;

	// Token: 0x04000127 RID: 295
	public Vector3 rotationOffset;
}
