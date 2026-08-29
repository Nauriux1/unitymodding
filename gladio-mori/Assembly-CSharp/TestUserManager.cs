using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

// Token: 0x02000263 RID: 611
public class TestUserManager : MonoBehaviour
{
	// Token: 0x060011D3 RID: 4563 RVA: 0x0005B498 File Offset: 0x00059698
	private void Start()
	{
		InputUser.listenForUnpairedDeviceActivity++;
		InputUser.onUnpairedDeviceUsed += delegate(InputControl control, InputEventPtr eventPtr)
		{
			if (!(control is ButtonControl))
			{
				return;
			}
			TestControlsPlayer component = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab).GetComponent<TestControlsPlayer>();
			InputUser newUser = InputUser.PerformPairingWithDevice(control.device, default(InputUser), InputUserPairingOptions.None);
			component.ConnectToUser(newUser);
		};
	}

	// Token: 0x060011D4 RID: 4564 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x04000D6D RID: 3437
	public GameObject playerPrefab;
}
