using System;
using UnityEngine;

// Token: 0x0200015E RID: 350
public class RotatePlayer : MonoBehaviour
{
	// Token: 0x06000B28 RID: 2856 RVA: 0x000361AC File Offset: 0x000343AC
	private void Start()
	{
		this.rotationSpeed = 100f;
		this.rb = base.GetComponent<Rigidbody>();
		this.player = (PlayerHealth)base.GetComponentsInParent(typeof(PlayerHealth))[0];
		this.playerNum = this.player.playerNum;
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x00036200 File Offset: 0x00034400
	private void FixedUpdate()
	{
		float num = this.rotationInputLeft + this.rotationInputRight;
		if (this.useTargetRotation)
		{
			float num2 = this.NormalizeAngle(this.rb.rotation.eulerAngles.y) - this.NormalizeAngle(this.targetRotation);
			if (Mathf.Abs(num2) > 1f)
			{
				if (this.NormalizeAngle(num2) < 180f)
				{
					num = -1f;
				}
				else
				{
					num = 1f;
				}
			}
		}
		if (Math.Abs(num) > 0.1f)
		{
			float y = num * this.rotationSpeed;
			Quaternion rhs = Quaternion.Euler(new Vector3(0f, y, 0f) * Time.deltaTime);
			this.rb.MoveRotation(this.rb.rotation * rhs);
		}
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x000362CC File Offset: 0x000344CC
	private float NormalizeAngle(float angle)
	{
		float num = angle % 360f;
		if (num < 0f)
		{
			num += 360f;
		}
		return num;
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x000362F2 File Offset: 0x000344F2
	public void SetRotationInputLeft(float speed)
	{
		this.rotationInputLeft = speed;
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x000362FB File Offset: 0x000344FB
	public void SetRotationInputRight(float speed)
	{
		this.rotationInputRight = speed;
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x000362F2 File Offset: 0x000344F2
	public void SetRotationInput(float speed)
	{
		this.rotationInputLeft = speed;
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x00036304 File Offset: 0x00034504
	public void SetUseTargetRotation(bool value)
	{
		if (value != this.useTargetRotation)
		{
			this.useTargetRotation = value;
		}
	}

	// Token: 0x040007BB RID: 1979
	private Rigidbody rb;

	// Token: 0x040007BC RID: 1980
	private PlayerHealth player;

	// Token: 0x040007BD RID: 1981
	private int playerNum;

	// Token: 0x040007BE RID: 1982
	public float rotationSpeed = 100f;

	// Token: 0x040007BF RID: 1983
	private float rotationInputLeft;

	// Token: 0x040007C0 RID: 1984
	private float rotationInputRight;

	// Token: 0x040007C1 RID: 1985
	public bool useTargetRotation;

	// Token: 0x040007C2 RID: 1986
	public float targetRotation;
}
