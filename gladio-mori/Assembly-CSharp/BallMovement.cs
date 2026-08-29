using System;
using UnityEngine;

// Token: 0x0200002A RID: 42
public class BallMovement : MonoBehaviour
{
	// Token: 0x06000180 RID: 384 RVA: 0x00008C6C File Offset: 0x00006E6C
	private void Awake()
	{
		if (IGameSettingsManager.singleton != null)
		{
			this.rollingFeet = IGameSettingsManager.singleton.GetRollingFeet();
		}
	}

	// Token: 0x06000181 RID: 385 RVA: 0x00008C88 File Offset: 0x00006E88
	private void Start()
	{
		if (this.userCamera == null)
		{
			this.userCamera = Camera.main;
		}
		this.joint = base.GetComponent<ConfigurableJoint>();
		this.rb = base.GetComponent<Rigidbody>();
		this.player = (PlayerHealth)base.GetComponentsInParent(typeof(PlayerHealth))[0];
		if (this.joint != null)
		{
			this.joint.angularYMotion = ConfigurableJointMotion.Locked;
			this.joint.angularXMotion = ConfigurableJointMotion.Locked;
			this.joint.angularZMotion = ConfigurableJointMotion.Locked;
		}
	}

	// Token: 0x06000182 RID: 386 RVA: 0x00008D18 File Offset: 0x00006F18
	private void Update()
	{
		if (!this.rollingFeet)
		{
			return;
		}
		if (this.verticalSpeed != 0f || this.horizontalSpeed != 0f)
		{
			this.joint.angularYMotion = ConfigurableJointMotion.Free;
			this.joint.angularXMotion = ConfigurableJointMotion.Free;
			this.joint.angularZMotion = ConfigurableJointMotion.Free;
			Vector3 vector = this.player.cameraPoint.transform.TransformDirection(Vector3.forward);
			vector.y = 0f;
			vector = vector.normalized;
			Vector3 a = new Vector3(vector.z, 0f, -vector.x);
			float d = this.horizontalSpeed;
			float d2 = this.verticalSpeed;
			Vector3 vector2 = d * a + d2 * vector;
			vector2 = Quaternion.Inverse(base.transform.rotation) * vector2;
			vector2 = new Vector3(vector2.x, vector2.y, vector2.z);
			vector2 = base.transform.rotation * vector2;
			this.rb.AddForce(vector2.normalized * this.speed * Time.deltaTime);
			return;
		}
		this.joint.angularYMotion = ConfigurableJointMotion.Locked;
		this.joint.angularXMotion = ConfigurableJointMotion.Locked;
		this.joint.angularZMotion = ConfigurableJointMotion.Locked;
	}

	// Token: 0x06000183 RID: 387 RVA: 0x00008E65 File Offset: 0x00007065
	public void SetVerticalSpeed(float speed)
	{
		this.verticalSpeed = speed;
	}

	// Token: 0x06000184 RID: 388 RVA: 0x00008E6E File Offset: 0x0000706E
	public void SetHorizontalSpeed(float speed)
	{
		this.horizontalSpeed = speed;
	}

	// Token: 0x06000185 RID: 389 RVA: 0x00008E77 File Offset: 0x00007077
	public void SetCamera(Camera newCamera)
	{
		this.userCamera = newCamera;
	}

	// Token: 0x040000B4 RID: 180
	private PlayerHealth player;

	// Token: 0x040000B5 RID: 181
	private Rigidbody rb;

	// Token: 0x040000B6 RID: 182
	private float speed = 30000f;

	// Token: 0x040000B7 RID: 183
	private ConfigurableJoint joint;

	// Token: 0x040000B8 RID: 184
	public Camera userCamera;

	// Token: 0x040000B9 RID: 185
	private bool rollingFeet;

	// Token: 0x040000BA RID: 186
	private float verticalSpeed;

	// Token: 0x040000BB RID: 187
	private float horizontalSpeed;
}
