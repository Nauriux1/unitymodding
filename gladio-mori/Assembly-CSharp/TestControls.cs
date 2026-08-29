using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

// Token: 0x0200025F RID: 607
public class TestControls : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
{
	// Token: 0x170001BC RID: 444
	// (get) Token: 0x060011B4 RID: 4532 RVA: 0x0005B170 File Offset: 0x00059370
	public InputActionAsset asset { get; }

	// Token: 0x060011B5 RID: 4533 RVA: 0x0005B178 File Offset: 0x00059378
	public TestControls()
	{
		this.asset = InputActionAsset.FromJson("{\r\n    \"name\": \"TestControls\",\r\n    \"maps\": [\r\n        {\r\n            \"name\": \"TestGameplay\",\r\n            \"id\": \"a3b8a2c7-ee0a-46b8-a806-5c2cd8f62f09\",\r\n            \"actions\": [\r\n                {\r\n                    \"name\": \"Move\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"6e1bfd60-e840-4b65-a42c-e2f725f870d8\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                }\r\n            ],\r\n            \"bindings\": [\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"777dc87a-641e-427b-904e-3d8743ef31e0\",\r\n                    \"path\": \"<Gamepad>/leftStick\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"2D Vector\",\r\n                    \"id\": \"9c543145-948d-43c4-a2f5-73aa76f5ee69\",\r\n                    \"path\": \"2DVector\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": true,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"4e88979f-c425-4f92-8a69-8368266cf0c3\",\r\n                    \"path\": \"<Keyboard>/w\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"6277146d-0808-4906-bb10-08799a28997e\",\r\n                    \"path\": \"<Keyboard>/s\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"left\",\r\n                    \"id\": \"bea4c8d9-1fd3-44e7-819b-841da8867abe\",\r\n                    \"path\": \"<Keyboard>/a\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"right\",\r\n                    \"id\": \"3b2bcaa1-7de9-4690-b313-bdce20bc1297\",\r\n                    \"path\": \"<Keyboard>/d\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                }\r\n            ]\r\n        }\r\n    ],\r\n    \"controlSchemes\": []\r\n}");
		this.m_TestGameplay = this.asset.FindActionMap("TestGameplay", true);
		this.m_TestGameplay_Move = this.m_TestGameplay.FindAction("Move", true);
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x0005B1C9 File Offset: 0x000593C9
	public void Dispose()
	{
		UnityEngine.Object.Destroy(this.asset);
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x060011B7 RID: 4535 RVA: 0x0005B1D6 File Offset: 0x000593D6
	// (set) Token: 0x060011B8 RID: 4536 RVA: 0x0005B1E3 File Offset: 0x000593E3
	public InputBinding? bindingMask
	{
		get
		{
			return this.asset.bindingMask;
		}
		set
		{
			this.asset.bindingMask = value;
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x060011B9 RID: 4537 RVA: 0x0005B1F1 File Offset: 0x000593F1
	// (set) Token: 0x060011BA RID: 4538 RVA: 0x0005B1FE File Offset: 0x000593FE
	public ReadOnlyArray<InputDevice>? devices
	{
		get
		{
			return this.asset.devices;
		}
		set
		{
			this.asset.devices = value;
		}
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x060011BB RID: 4539 RVA: 0x0005B20C File Offset: 0x0005940C
	public ReadOnlyArray<InputControlScheme> controlSchemes
	{
		get
		{
			return this.asset.controlSchemes;
		}
	}

	// Token: 0x060011BC RID: 4540 RVA: 0x0005B219 File Offset: 0x00059419
	public bool Contains(InputAction action)
	{
		return this.asset.Contains(action);
	}

	// Token: 0x060011BD RID: 4541 RVA: 0x0005B227 File Offset: 0x00059427
	public IEnumerator<InputAction> GetEnumerator()
	{
		return this.asset.GetEnumerator();
	}

	// Token: 0x060011BE RID: 4542 RVA: 0x0005B234 File Offset: 0x00059434
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x0005B23C File Offset: 0x0005943C
	public void Enable()
	{
		this.asset.Enable();
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x0005B249 File Offset: 0x00059449
	public void Disable()
	{
		this.asset.Disable();
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x060011C1 RID: 4545 RVA: 0x0005B256 File Offset: 0x00059456
	public IEnumerable<InputBinding> bindings
	{
		get
		{
			return this.asset.bindings;
		}
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x0005B263 File Offset: 0x00059463
	public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
	{
		return this.asset.FindAction(actionNameOrId, throwIfNotFound);
	}

	// Token: 0x060011C3 RID: 4547 RVA: 0x0005B272 File Offset: 0x00059472
	public int FindBinding(InputBinding bindingMask, out InputAction action)
	{
		return this.asset.FindBinding(bindingMask, out action);
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x060011C4 RID: 4548 RVA: 0x0005B281 File Offset: 0x00059481
	public TestControls.TestGameplayActions TestGameplay
	{
		get
		{
			return new TestControls.TestGameplayActions(this);
		}
	}

	// Token: 0x04000D66 RID: 3430
	private readonly InputActionMap m_TestGameplay;

	// Token: 0x04000D67 RID: 3431
	private TestControls.ITestGameplayActions m_TestGameplayActionsCallbackInterface;

	// Token: 0x04000D68 RID: 3432
	private readonly InputAction m_TestGameplay_Move;

	// Token: 0x02000260 RID: 608
	public struct TestGameplayActions
	{
		// Token: 0x060011C5 RID: 4549 RVA: 0x0005B289 File Offset: 0x00059489
		public TestGameplayActions(TestControls wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060011C6 RID: 4550 RVA: 0x0005B292 File Offset: 0x00059492
		public InputAction Move
		{
			get
			{
				return this.m_Wrapper.m_TestGameplay_Move;
			}
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0005B29F File Offset: 0x0005949F
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_TestGameplay;
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x0005B2AC File Offset: 0x000594AC
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x0005B2B9 File Offset: 0x000594B9
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060011CA RID: 4554 RVA: 0x0005B2C6 File Offset: 0x000594C6
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0005B2D3 File Offset: 0x000594D3
		public static implicit operator InputActionMap(TestControls.TestGameplayActions set)
		{
			return set.Get();
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0005B2DC File Offset: 0x000594DC
		public void SetCallbacks(TestControls.ITestGameplayActions instance)
		{
			if (this.m_Wrapper.m_TestGameplayActionsCallbackInterface != null)
			{
				this.Move.started -= this.m_Wrapper.m_TestGameplayActionsCallbackInterface.OnMove;
				this.Move.performed -= this.m_Wrapper.m_TestGameplayActionsCallbackInterface.OnMove;
				this.Move.canceled -= this.m_Wrapper.m_TestGameplayActionsCallbackInterface.OnMove;
			}
			this.m_Wrapper.m_TestGameplayActionsCallbackInterface = instance;
			if (instance != null)
			{
				this.Move.started += instance.OnMove;
				this.Move.performed += instance.OnMove;
				this.Move.canceled += instance.OnMove;
			}
		}

		// Token: 0x04000D69 RID: 3433
		private TestControls m_Wrapper;
	}

	// Token: 0x02000261 RID: 609
	public interface ITestGameplayActions
	{
		// Token: 0x060011CD RID: 4557
		void OnMove(InputAction.CallbackContext context);
	}
}
