using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

// Token: 0x0200000A RID: 10
public class MenuInputActionsMoveEditor : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
{
	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000049 RID: 73 RVA: 0x00002FD7 File Offset: 0x000011D7
	public InputActionAsset asset { get; }

	// Token: 0x0600004A RID: 74 RVA: 0x00002FE0 File Offset: 0x000011E0
	public MenuInputActionsMoveEditor()
	{
		this.asset = InputActionAsset.FromJson("{\n    \"name\": \"MenuInputActionsMoveEditor\",\n    \"maps\": [\n        {\n            \"name\": \"Player\",\n            \"id\": \"df70fa95-8a34-4494-b137-73ab6b9c7d37\",\n            \"actions\": [\n                {\n                    \"name\": \"Move\",\n                    \"type\": \"Value\",\n                    \"id\": \"351f2ccd-1f9f-44bf-9bec-d62ac5c5f408\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Look\",\n                    \"type\": \"Value\",\n                    \"id\": \"6b444451-8a00-4d00-a97e-f47457f736a8\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Fire\",\n                    \"type\": \"Button\",\n                    \"id\": \"6c2ab1b8-8984-453a-af3d-a3c78ae1679a\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"\",\n                    \"id\": \"978bfe49-cc26-4a3d-ab7b-7d7a29327403\",\n                    \"path\": \"<Gamepad>/leftStick\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"WASD\",\n                    \"id\": \"00ca640b-d935-4593-8157-c05846ea39b3\",\n                    \"path\": \"Dpad\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"8180e8bd-4097-4f4e-ab88-4523101a6ce9\",\n                    \"path\": \"<Keyboard>/upArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"1c5327b5-f71c-4f60-99c7-4e737386f1d1\",\n                    \"path\": \"<Keyboard>/downArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"2e46982e-44cc-431b-9f0b-c11910bf467a\",\n                    \"path\": \"<Keyboard>/leftArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"77bff152-3580-4b21-b6de-dcd0c7e41164\",\n                    \"path\": \"<Keyboard>/rightArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"1635d3fe-58b6-4ba9-a4e2-f4b964f6b5c8\",\n                    \"path\": \"<XRController>/{Primary2DAxis}\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"XR\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"3ea4d645-4504-4529-b061-ab81934c3752\",\n                    \"path\": \"<Joystick>/stick\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Joystick\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c1f7a91b-d0fd-4a62-997e-7fb9b69bf235\",\n                    \"path\": \"<Gamepad>/rightStick\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Look\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"8c8e490b-c610-4785-884f-f04217b23ca4\",\n                    \"path\": \"<Pointer>/delta\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse;Touch\",\n                    \"action\": \"Look\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"3e5f5442-8668-4b27-a940-df99bad7e831\",\n                    \"path\": \"<Joystick>/{Hatswitch}\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Joystick\",\n                    \"action\": \"Look\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"143bb1cd-cc10-4eca-a2f0-a3664166fe91\",\n                    \"path\": \"<Gamepad>/rightTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Fire\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"05f6913d-c316-48b2-a6bb-e225f14c7960\",\n                    \"path\": \"<Mouse>/leftButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Fire\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"886e731e-7071-4ae4-95c0-e61739dad6fd\",\n                    \"path\": \"<Touchscreen>/primaryTouch/tap\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Touch\",\n                    \"action\": \"Fire\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"ee3d0cd2-254e-47a7-a8cb-bc94d9658c54\",\n                    \"path\": \"<Joystick>/trigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Joystick\",\n                    \"action\": \"Fire\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"8255d333-5683-4943-a58a-ccb207ff1dce\",\n                    \"path\": \"<XRController>/{PrimaryAction}\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"XR\",\n                    \"action\": \"Fire\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        },\n        {\n            \"name\": \"UI\",\n            \"id\": \"272f6d14-89ba-496f-b7ff-215263d3219f\",\n            \"actions\": [\n                {\n                    \"name\": \"Navigate\",\n                    \"type\": \"Value\",\n                    \"id\": \"c95b2375-e6d9-4b88-9c4c-c5e76515df4b\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Submit\",\n                    \"type\": \"Button\",\n                    \"id\": \"7607c7b6-cd76-4816-beef-bd0341cfe950\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Cancel\",\n                    \"type\": \"Button\",\n                    \"id\": \"15cef263-9014-4fd5-94d9-4e4a6234a6ef\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Point\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"32b35790-4ed0-4e9a-aa41-69ac6d629449\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Click\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"3c7022bf-7922-4f7c-a998-c437916075ad\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ScrollWheel\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"0489e84a-4833-4c40-bfae-cea84b696689\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"MiddleClick\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"dad70c86-b58c-4b17-88ad-f5e53adf419e\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"RightClick\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"44b200b1-1557-4083-816c-b22cbdf77ddf\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"TrackedDevicePosition\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"24908448-c609-4bc3-a128-ea258674378a\",\n                    \"expectedControlType\": \"Vector3\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"TrackedDeviceOrientation\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"9caa3d8a-6b2f-4e8e-8bad-6ede561bd9be\",\n                    \"expectedControlType\": \"Quaternion\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"Gamepad\",\n                    \"id\": \"809f371f-c5e2-4e7a-83a1-d867598f40dd\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"14a5d6e8-4aaf-4119-a9ef-34b8c2c548bf\",\n                    \"path\": \"<Gamepad>/leftStick/up\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"9144cbe6-05e1-4687-a6d7-24f99d23dd81\",\n                    \"path\": \"<Gamepad>/rightStick/up\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"2db08d65-c5fb-421b-983f-c71163608d67\",\n                    \"path\": \"<Gamepad>/leftStick/down\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"58748904-2ea9-4a80-8579-b500e6a76df8\",\n                    \"path\": \"<Gamepad>/rightStick/down\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"8ba04515-75aa-45de-966d-393d9bbd1c14\",\n                    \"path\": \"<Gamepad>/leftStick/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"712e721c-bdfb-4b23-a86c-a0d9fcfea921\",\n                    \"path\": \"<Gamepad>/rightStick/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"fcd248ae-a788-4676-a12e-f4d81205600b\",\n                    \"path\": \"<Gamepad>/leftStick/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"1f04d9bc-c50b-41a1-bfcc-afb75475ec20\",\n                    \"path\": \"<Gamepad>/rightStick/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"fb8277d4-c5cd-4663-9dc7-ee3f0b506d90\",\n                    \"path\": \"<Gamepad>/dpad\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"Joystick\",\n                    \"id\": \"e25d9774-381c-4a61-b47c-7b6b299ad9f9\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"3db53b26-6601-41be-9887-63ac74e79d19\",\n                    \"path\": \"<Joystick>/stick/up\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Joystick\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"0cb3e13e-3d90-4178-8ae6-d9c5501d653f\",\n                    \"path\": \"<Joystick>/stick/down\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Joystick\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"0392d399-f6dd-4c82-8062-c1e9c0d34835\",\n                    \"path\": \"<Joystick>/stick/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Joystick\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"942a66d9-d42f-43d6-8d70-ecb4ba5363bc\",\n                    \"path\": \"<Joystick>/stick/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Joystick\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Keyboard\",\n                    \"id\": \"ff527021-f211-4c02-933e-5976594c46ed\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"eb480147-c587-4a33-85ed-eb0ab9942c43\",\n                    \"path\": \"<Keyboard>/upArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"85d264ad-e0a0-4565-b7ff-1a37edde51ac\",\n                    \"path\": \"<Keyboard>/downArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": tr[...string is too long...]");
		this.m_Player = this.asset.FindActionMap("Player", true);
		this.m_Player_Move = this.m_Player.FindAction("Move", true);
		this.m_Player_Look = this.m_Player.FindAction("Look", true);
		this.m_Player_Fire = this.m_Player.FindAction("Fire", true);
		this.m_UI = this.asset.FindActionMap("UI", true);
		this.m_UI_Navigate = this.m_UI.FindAction("Navigate", true);
		this.m_UI_Submit = this.m_UI.FindAction("Submit", true);
		this.m_UI_Cancel = this.m_UI.FindAction("Cancel", true);
		this.m_UI_Point = this.m_UI.FindAction("Point", true);
		this.m_UI_Click = this.m_UI.FindAction("Click", true);
		this.m_UI_ScrollWheel = this.m_UI.FindAction("ScrollWheel", true);
		this.m_UI_MiddleClick = this.m_UI.FindAction("MiddleClick", true);
		this.m_UI_RightClick = this.m_UI.FindAction("RightClick", true);
		this.m_UI_TrackedDevicePosition = this.m_UI.FindAction("TrackedDevicePosition", true);
		this.m_UI_TrackedDeviceOrientation = this.m_UI.FindAction("TrackedDeviceOrientation", true);
	}

	// Token: 0x0600004B RID: 75 RVA: 0x0000317F File Offset: 0x0000137F
	public void Dispose()
	{
		UnityEngine.Object.Destroy(this.asset);
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x0600004C RID: 76 RVA: 0x0000318C File Offset: 0x0000138C
	// (set) Token: 0x0600004D RID: 77 RVA: 0x00003199 File Offset: 0x00001399
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

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x0600004E RID: 78 RVA: 0x000031A7 File Offset: 0x000013A7
	// (set) Token: 0x0600004F RID: 79 RVA: 0x000031B4 File Offset: 0x000013B4
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

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000050 RID: 80 RVA: 0x000031C2 File Offset: 0x000013C2
	public ReadOnlyArray<InputControlScheme> controlSchemes
	{
		get
		{
			return this.asset.controlSchemes;
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000031CF File Offset: 0x000013CF
	public bool Contains(InputAction action)
	{
		return this.asset.Contains(action);
	}

	// Token: 0x06000052 RID: 82 RVA: 0x000031DD File Offset: 0x000013DD
	public IEnumerator<InputAction> GetEnumerator()
	{
		return this.asset.GetEnumerator();
	}

	// Token: 0x06000053 RID: 83 RVA: 0x000031EA File Offset: 0x000013EA
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x06000054 RID: 84 RVA: 0x000031F2 File Offset: 0x000013F2
	public void Enable()
	{
		this.asset.Enable();
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000031FF File Offset: 0x000013FF
	public void Disable()
	{
		this.asset.Disable();
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000056 RID: 86 RVA: 0x0000320C File Offset: 0x0000140C
	public IEnumerable<InputBinding> bindings
	{
		get
		{
			return this.asset.bindings;
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00003219 File Offset: 0x00001419
	public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
	{
		return this.asset.FindAction(actionNameOrId, throwIfNotFound);
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00003228 File Offset: 0x00001428
	public int FindBinding(InputBinding bindingMask, out InputAction action)
	{
		return this.asset.FindBinding(bindingMask, out action);
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000059 RID: 89 RVA: 0x00003237 File Offset: 0x00001437
	public MenuInputActionsMoveEditor.PlayerActions Player
	{
		get
		{
			return new MenuInputActionsMoveEditor.PlayerActions(this);
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x0600005A RID: 90 RVA: 0x0000323F File Offset: 0x0000143F
	public MenuInputActionsMoveEditor.UIActions UI
	{
		get
		{
			return new MenuInputActionsMoveEditor.UIActions(this);
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x0600005B RID: 91 RVA: 0x00003248 File Offset: 0x00001448
	public InputControlScheme KeyboardMouseScheme
	{
		get
		{
			if (this.m_KeyboardMouseSchemeIndex == -1)
			{
				this.m_KeyboardMouseSchemeIndex = this.asset.FindControlSchemeIndex("Keyboard&Mouse");
			}
			return this.asset.controlSchemes[this.m_KeyboardMouseSchemeIndex];
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x0600005C RID: 92 RVA: 0x00003290 File Offset: 0x00001490
	public InputControlScheme GamepadScheme
	{
		get
		{
			if (this.m_GamepadSchemeIndex == -1)
			{
				this.m_GamepadSchemeIndex = this.asset.FindControlSchemeIndex("Gamepad");
			}
			return this.asset.controlSchemes[this.m_GamepadSchemeIndex];
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x0600005D RID: 93 RVA: 0x000032D8 File Offset: 0x000014D8
	public InputControlScheme TouchScheme
	{
		get
		{
			if (this.m_TouchSchemeIndex == -1)
			{
				this.m_TouchSchemeIndex = this.asset.FindControlSchemeIndex("Touch");
			}
			return this.asset.controlSchemes[this.m_TouchSchemeIndex];
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x0600005E RID: 94 RVA: 0x00003320 File Offset: 0x00001520
	public InputControlScheme JoystickScheme
	{
		get
		{
			if (this.m_JoystickSchemeIndex == -1)
			{
				this.m_JoystickSchemeIndex = this.asset.FindControlSchemeIndex("Joystick");
			}
			return this.asset.controlSchemes[this.m_JoystickSchemeIndex];
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x0600005F RID: 95 RVA: 0x00003368 File Offset: 0x00001568
	public InputControlScheme XRScheme
	{
		get
		{
			if (this.m_XRSchemeIndex == -1)
			{
				this.m_XRSchemeIndex = this.asset.FindControlSchemeIndex("XR");
			}
			return this.asset.controlSchemes[this.m_XRSchemeIndex];
		}
	}

	// Token: 0x0400001E RID: 30
	private readonly InputActionMap m_Player;

	// Token: 0x0400001F RID: 31
	private MenuInputActionsMoveEditor.IPlayerActions m_PlayerActionsCallbackInterface;

	// Token: 0x04000020 RID: 32
	private readonly InputAction m_Player_Move;

	// Token: 0x04000021 RID: 33
	private readonly InputAction m_Player_Look;

	// Token: 0x04000022 RID: 34
	private readonly InputAction m_Player_Fire;

	// Token: 0x04000023 RID: 35
	private readonly InputActionMap m_UI;

	// Token: 0x04000024 RID: 36
	private MenuInputActionsMoveEditor.IUIActions m_UIActionsCallbackInterface;

	// Token: 0x04000025 RID: 37
	private readonly InputAction m_UI_Navigate;

	// Token: 0x04000026 RID: 38
	private readonly InputAction m_UI_Submit;

	// Token: 0x04000027 RID: 39
	private readonly InputAction m_UI_Cancel;

	// Token: 0x04000028 RID: 40
	private readonly InputAction m_UI_Point;

	// Token: 0x04000029 RID: 41
	private readonly InputAction m_UI_Click;

	// Token: 0x0400002A RID: 42
	private readonly InputAction m_UI_ScrollWheel;

	// Token: 0x0400002B RID: 43
	private readonly InputAction m_UI_MiddleClick;

	// Token: 0x0400002C RID: 44
	private readonly InputAction m_UI_RightClick;

	// Token: 0x0400002D RID: 45
	private readonly InputAction m_UI_TrackedDevicePosition;

	// Token: 0x0400002E RID: 46
	private readonly InputAction m_UI_TrackedDeviceOrientation;

	// Token: 0x0400002F RID: 47
	private int m_KeyboardMouseSchemeIndex = -1;

	// Token: 0x04000030 RID: 48
	private int m_GamepadSchemeIndex = -1;

	// Token: 0x04000031 RID: 49
	private int m_TouchSchemeIndex = -1;

	// Token: 0x04000032 RID: 50
	private int m_JoystickSchemeIndex = -1;

	// Token: 0x04000033 RID: 51
	private int m_XRSchemeIndex = -1;

	// Token: 0x0200000B RID: 11
	public struct PlayerActions
	{
		// Token: 0x06000060 RID: 96 RVA: 0x000033AD File Offset: 0x000015AD
		public PlayerActions(MenuInputActionsMoveEditor wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000061 RID: 97 RVA: 0x000033B6 File Offset: 0x000015B6
		public InputAction Move
		{
			get
			{
				return this.m_Wrapper.m_Player_Move;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000033C3 File Offset: 0x000015C3
		public InputAction Look
		{
			get
			{
				return this.m_Wrapper.m_Player_Look;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000033D0 File Offset: 0x000015D0
		public InputAction Fire
		{
			get
			{
				return this.m_Wrapper.m_Player_Fire;
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000033DD File Offset: 0x000015DD
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_Player;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000033EA File Offset: 0x000015EA
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000033F7 File Offset: 0x000015F7
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003404 File Offset: 0x00001604
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003411 File Offset: 0x00001611
		public static implicit operator InputActionMap(MenuInputActionsMoveEditor.PlayerActions set)
		{
			return set.Get();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000341C File Offset: 0x0000161C
		public void SetCallbacks(MenuInputActionsMoveEditor.IPlayerActions instance)
		{
			if (this.m_Wrapper.m_PlayerActionsCallbackInterface != null)
			{
				this.Move.started -= this.m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
				this.Move.performed -= this.m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
				this.Move.canceled -= this.m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
				this.Look.started -= this.m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
				this.Look.performed -= this.m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
				this.Look.canceled -= this.m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
				this.Fire.started -= this.m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
				this.Fire.performed -= this.m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
				this.Fire.canceled -= this.m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
			}
			this.m_Wrapper.m_PlayerActionsCallbackInterface = instance;
			if (instance != null)
			{
				this.Move.started += instance.OnMove;
				this.Move.performed += instance.OnMove;
				this.Move.canceled += instance.OnMove;
				this.Look.started += instance.OnLook;
				this.Look.performed += instance.OnLook;
				this.Look.canceled += instance.OnLook;
				this.Fire.started += instance.OnFire;
				this.Fire.performed += instance.OnFire;
				this.Fire.canceled += instance.OnFire;
			}
		}

		// Token: 0x04000034 RID: 52
		private MenuInputActionsMoveEditor m_Wrapper;
	}

	// Token: 0x0200000C RID: 12
	public struct UIActions
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00003655 File Offset: 0x00001855
		public UIActions(MenuInputActionsMoveEditor wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600006B RID: 107 RVA: 0x0000365E File Offset: 0x0000185E
		public InputAction Navigate
		{
			get
			{
				return this.m_Wrapper.m_UI_Navigate;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600006C RID: 108 RVA: 0x0000366B File Offset: 0x0000186B
		public InputAction Submit
		{
			get
			{
				return this.m_Wrapper.m_UI_Submit;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003678 File Offset: 0x00001878
		public InputAction Cancel
		{
			get
			{
				return this.m_Wrapper.m_UI_Cancel;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003685 File Offset: 0x00001885
		public InputAction Point
		{
			get
			{
				return this.m_Wrapper.m_UI_Point;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003692 File Offset: 0x00001892
		public InputAction Click
		{
			get
			{
				return this.m_Wrapper.m_UI_Click;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000070 RID: 112 RVA: 0x0000369F File Offset: 0x0000189F
		public InputAction ScrollWheel
		{
			get
			{
				return this.m_Wrapper.m_UI_ScrollWheel;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000036AC File Offset: 0x000018AC
		public InputAction MiddleClick
		{
			get
			{
				return this.m_Wrapper.m_UI_MiddleClick;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000072 RID: 114 RVA: 0x000036B9 File Offset: 0x000018B9
		public InputAction RightClick
		{
			get
			{
				return this.m_Wrapper.m_UI_RightClick;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000073 RID: 115 RVA: 0x000036C6 File Offset: 0x000018C6
		public InputAction TrackedDevicePosition
		{
			get
			{
				return this.m_Wrapper.m_UI_TrackedDevicePosition;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000036D3 File Offset: 0x000018D3
		public InputAction TrackedDeviceOrientation
		{
			get
			{
				return this.m_Wrapper.m_UI_TrackedDeviceOrientation;
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000036E0 File Offset: 0x000018E0
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_UI;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000036ED File Offset: 0x000018ED
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000036FA File Offset: 0x000018FA
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003707 File Offset: 0x00001907
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003714 File Offset: 0x00001914
		public static implicit operator InputActionMap(MenuInputActionsMoveEditor.UIActions set)
		{
			return set.Get();
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003720 File Offset: 0x00001920
		public void SetCallbacks(MenuInputActionsMoveEditor.IUIActions instance)
		{
			if (this.m_Wrapper.m_UIActionsCallbackInterface != null)
			{
				this.Navigate.started -= this.m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
				this.Navigate.performed -= this.m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
				this.Navigate.canceled -= this.m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
				this.Submit.started -= this.m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
				this.Submit.performed -= this.m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
				this.Submit.canceled -= this.m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
				this.Cancel.started -= this.m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
				this.Cancel.performed -= this.m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
				this.Cancel.canceled -= this.m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
				this.Point.started -= this.m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
				this.Point.performed -= this.m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
				this.Point.canceled -= this.m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
				this.Click.started -= this.m_Wrapper.m_UIActionsCallbackInterface.OnClick;
				this.Click.performed -= this.m_Wrapper.m_UIActionsCallbackInterface.OnClick;
				this.Click.canceled -= this.m_Wrapper.m_UIActionsCallbackInterface.OnClick;
				this.ScrollWheel.started -= this.m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
				this.ScrollWheel.performed -= this.m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
				this.ScrollWheel.canceled -= this.m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
				this.MiddleClick.started -= this.m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
				this.MiddleClick.performed -= this.m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
				this.MiddleClick.canceled -= this.m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
				this.RightClick.started -= this.m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
				this.RightClick.performed -= this.m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
				this.RightClick.canceled -= this.m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
				this.TrackedDevicePosition.started -= this.m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
				this.TrackedDevicePosition.performed -= this.m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
				this.TrackedDevicePosition.canceled -= this.m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
				this.TrackedDeviceOrientation.started -= this.m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
				this.TrackedDeviceOrientation.performed -= this.m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
				this.TrackedDeviceOrientation.canceled -= this.m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
			}
			this.m_Wrapper.m_UIActionsCallbackInterface = instance;
			if (instance != null)
			{
				this.Navigate.started += instance.OnNavigate;
				this.Navigate.performed += instance.OnNavigate;
				this.Navigate.canceled += instance.OnNavigate;
				this.Submit.started += instance.OnSubmit;
				this.Submit.performed += instance.OnSubmit;
				this.Submit.canceled += instance.OnSubmit;
				this.Cancel.started += instance.OnCancel;
				this.Cancel.performed += instance.OnCancel;
				this.Cancel.canceled += instance.OnCancel;
				this.Point.started += instance.OnPoint;
				this.Point.performed += instance.OnPoint;
				this.Point.canceled += instance.OnPoint;
				this.Click.started += instance.OnClick;
				this.Click.performed += instance.OnClick;
				this.Click.canceled += instance.OnClick;
				this.ScrollWheel.started += instance.OnScrollWheel;
				this.ScrollWheel.performed += instance.OnScrollWheel;
				this.ScrollWheel.canceled += instance.OnScrollWheel;
				this.MiddleClick.started += instance.OnMiddleClick;
				this.MiddleClick.performed += instance.OnMiddleClick;
				this.MiddleClick.canceled += instance.OnMiddleClick;
				this.RightClick.started += instance.OnRightClick;
				this.RightClick.performed += instance.OnRightClick;
				this.RightClick.canceled += instance.OnRightClick;
				this.TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
				this.TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
				this.TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
				this.TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
				this.TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
				this.TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
			}
		}

		// Token: 0x04000035 RID: 53
		private MenuInputActionsMoveEditor m_Wrapper;
	}

	// Token: 0x0200000D RID: 13
	public interface IPlayerActions
	{
		// Token: 0x0600007B RID: 123
		void OnMove(InputAction.CallbackContext context);

		// Token: 0x0600007C RID: 124
		void OnLook(InputAction.CallbackContext context);

		// Token: 0x0600007D RID: 125
		void OnFire(InputAction.CallbackContext context);
	}

	// Token: 0x0200000E RID: 14
	public interface IUIActions
	{
		// Token: 0x0600007E RID: 126
		void OnNavigate(InputAction.CallbackContext context);

		// Token: 0x0600007F RID: 127
		void OnSubmit(InputAction.CallbackContext context);

		// Token: 0x06000080 RID: 128
		void OnCancel(InputAction.CallbackContext context);

		// Token: 0x06000081 RID: 129
		void OnPoint(InputAction.CallbackContext context);

		// Token: 0x06000082 RID: 130
		void OnClick(InputAction.CallbackContext context);

		// Token: 0x06000083 RID: 131
		void OnScrollWheel(InputAction.CallbackContext context);

		// Token: 0x06000084 RID: 132
		void OnMiddleClick(InputAction.CallbackContext context);

		// Token: 0x06000085 RID: 133
		void OnRightClick(InputAction.CallbackContext context);

		// Token: 0x06000086 RID: 134
		void OnTrackedDevicePosition(InputAction.CallbackContext context);

		// Token: 0x06000087 RID: 135
		void OnTrackedDeviceOrientation(InputAction.CallbackContext context);
	}
}
