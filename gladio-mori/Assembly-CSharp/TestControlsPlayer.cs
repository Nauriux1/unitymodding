using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

// Token: 0x02000262 RID: 610
public class TestControlsPlayer : MonoBehaviour
{
	// Token: 0x060011CE RID: 4558 RVA: 0x0005B3B4 File Offset: 0x000595B4
	private void Awake()
	{
		this.testControls = new TestControls();
		this.testControls.TestGameplay.Move.performed += this.Move_performed;
		this.testControls.TestGameplay.Move.canceled += this.Move_performed;
	}

	// Token: 0x060011CF RID: 4559 RVA: 0x0005B414 File Offset: 0x00059614
	public void ConnectToUser(InputUser newUser)
	{
		this.inputUser = newUser;
		this.inputUser.AssociateActionsWithUser(this.testControls);
		this.testControls.Enable();
	}

	// Token: 0x060011D0 RID: 4560 RVA: 0x0005B439 File Offset: 0x00059639
	private void Move_performed(InputAction.CallbackContext obj)
	{
		if (obj.canceled)
		{
			this.moveValue = null;
			return;
		}
		this.moveValue = new Vector2?(obj.ReadValue<Vector2>());
	}

	// Token: 0x060011D1 RID: 4561 RVA: 0x0005B463 File Offset: 0x00059663
	private void Update()
	{
		if (this.moveValue != null)
		{
			base.transform.Translate(this.moveValue.Value * Time.deltaTime, Space.World);
		}
	}

	// Token: 0x04000D6A RID: 3434
	public TestControls testControls;

	// Token: 0x04000D6B RID: 3435
	private Vector2? moveValue;

	// Token: 0x04000D6C RID: 3436
	public InputUser inputUser;
}
