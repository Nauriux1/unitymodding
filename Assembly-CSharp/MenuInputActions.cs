using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

// Token: 0x02000005 RID: 5
public class MenuInputActions : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600000A RID: 10 RVA: 0x00002191 File Offset: 0x00000391
	public InputActionAsset asset { get; }

	// Token: 0x0600000B RID: 11 RVA: 0x0000219C File Offset: 0x0000039C
	public MenuInputActions()
	{
		this.asset = InputActionAsset.FromJson("{\r\n    \"name\": \"MenuInputActions\",\r\n    \"maps\": [\r\n        {\r\n            \"name\": \"Player\",\r\n            \"id\": \"df70fa95-8a34-4494-b137-73ab6b9c7d37\",\r\n            \"actions\": [\r\n                {\r\n                    \"name\": \"Move\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"351f2ccd-1f9f-44bf-9bec-d62ac5c5f408\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"Look\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"6b444451-8a00-4d00-a97e-f47457f736a8\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"Fire\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"6c2ab1b8-8984-453a-af3d-a3c78ae1679a\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                }\r\n            ],\r\n            \"bindings\": [\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"978bfe49-cc26-4a3d-ab7b-7d7a29327403\",\r\n                    \"path\": \"<Gamepad>/leftStick\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Gamepad\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"WASD\",\r\n                    \"id\": \"00ca640b-d935-4593-8157-c05846ea39b3\",\r\n                    \"path\": \"Dpad\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": true,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"e2062cb9-1b15-46a2-838c-2f8d72a0bdd9\",\r\n                    \"path\": \"<Keyboard>/w\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard&Mouse\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"8180e8bd-4097-4f4e-ab88-4523101a6ce9\",\r\n                    \"path\": \"<Keyboard>/upArrow\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard&Mouse\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"320bffee-a40b-4347-ac70-c210eb8bc73a\",\r\n                    \"path\": \"<Keyboard>/s\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard&Mouse\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"1c5327b5-f71c-4f60-99c7-4e737386f1d1\",\r\n                    \"path\": \"<Keyboard>/downArrow\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard&Mouse\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"left\",\r\n                    \"id\": \"d2581a9b-1d11-4566-b27d-b92aff5fabbc\",\r\n                    \"path\": \"<Keyboard>/a\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard&Mouse\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"left\",\r\n                    \"id\": \"2e46982e-44cc-431b-9f0b-c11910bf467a\",\r\n                    \"path\": \"<Keyboard>/leftArrow\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard&Mouse\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"right\",\r\n                    \"id\": \"fcfe95b8-67b9-4526-84b5-5d0bc98d6400\",\r\n                    \"path\": \"<Keyboard>/d\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard&Mouse\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"right\",\r\n                    \"id\": \"77bff152-3580-4b21-b6de-dcd0c7e41164\",\r\n                    \"path\": \"<Keyboard>/rightArrow\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard&Mouse\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"1635d3fe-58b6-4ba9-a4e2-f4b964f6b5c8\",\r\n                    \"path\": \"<XRController>/{Primary2DAxis}\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"XR\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"3ea4d645-4504-4529-b061-ab81934c3752\",\r\n                    \"path\": \"<Joystick>/stick\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Joystick\",\r\n                    \"action\": \"Move\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"c1f7a91b-d0fd-4a62-997e-7fb9b69bf235\",\r\n                    \"path\": \"<Gamepad>/rightStick\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Gamepad\",\r\n                    \"action\": \"Look\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"8c8e490b-c610-4785-884f-f04217b23ca4\",\r\n                    \"path\": \"<Pointer>/delta\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard&Mouse;Touch\",\r\n                    \"action\": \"Look\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"3e5f5442-8668-4b27-a940-df99bad7e831\",\r\n                    \"path\": \"<Joystick>/{Hatswitch}\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Joystick\",\r\n                    \"action\": \"Look\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"143bb1cd-cc10-4eca-a2f0-a3664166fe91\",\r\n                    \"path\": \"<Gamepad>/rightTrigger\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Gamepad\",\r\n                    \"action\": \"Fire\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"05f6913d-c316-48b2-a6bb-e225f14c7960\",\r\n                    \"path\": \"<Mouse>/leftButton\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard&Mouse\",\r\n                    \"action\": \"Fire\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"886e731e-7071-4ae4-95c0-e61739dad6fd\",\r\n                    \"path\": \"<Touchscreen>/primaryTouch/tap\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Touch\",\r\n                    \"action\": \"Fire\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"ee3d0cd2-254e-47a7-a8cb-bc94d9658c54\",\r\n                    \"path\": \"<Joystick>/trigger\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Joystick\",\r\n                    \"action\": \"Fire\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"8255d333-5683-4943-a58a-ccb207ff1dce\",\r\n                    \"path\": \"<XRController>/{PrimaryAction}\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"XR\",\r\n                    \"action\": \"Fire\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"name\": \"UI\",\r\n            \"id\": \"272f6d14-89ba-496f-b7ff-215263d3219f\",\r\n            \"actions\": [\r\n                {\r\n                    \"name\": \"Navigate\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"c95b2375-e6d9-4b88-9c4c-c5e76515df4b\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"Submit\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"7607c7b6-cd76-4816-beef-bd0341cfe950\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Cancel\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"15cef263-9014-4fd5-94d9-4e4a6234a6ef\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Point\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"32b35790-4ed0-4e9a-aa41-69ac6d629449\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Click\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"3c7022bf-7922-4f7c-a998-c437916075ad\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"ScrollWheel\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"0489e84a-4833-4c40-bfae-cea84b696689\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"MiddleClick\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"dad70c86-b58c-4b17-88ad-f5e53adf419e\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"RightClick\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"44b200b1-1557-4083-816c-b22cbdf77ddf\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"TrackedDevicePosition\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"24908448-c609-4bc3-a128-ea258674378a\",\r\n                    \"expectedControlType\": \"Vector3\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"TrackedDeviceOrientation\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"9caa3d8a-6b2f-4e8e-8bad-6ede561bd9be\",\r\n                    \"expectedControlType\": \"Quaternion\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                }\r\n            ],\r\n            \"bindings\": [\r\n                {\r\n                    \"name\": \"Gamepad\",\r\n                    \"id\": \"809f371f-c5e2-4e7a-83a1-d867598f40dd\",\r\n                    \"path\": \"2DVector\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": true,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"14a5d6e8-4aaf-4119-a9ef-34b8c2c548bf\",\r\n                    \"path\": \"<Gamepad>/leftStick/up\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Gamepad\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"9144cbe6-05e1-4687-a6d7-24f99d23dd81\",\r\n                    \"path\": \"<Gamepad>/rightStick/up\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Gamepad\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"2db08d65-c5fb-421b-983f-c71163608d67\",\r\n                    \"path\": \"<Gamepad>/leftStick/down\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Gamepad\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"58748904-2ea9-4a80-8579-b500e6a76df8\",\r\n                    \"path\": \"<Gamepad>/rightStick/down\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Gamepad\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"left\",\r\n                    \"id\": \"8ba04515-75aa-45de-966d-393d9bbd1c14\",\r\n                    \"path\": \"<Gamepad>/leftStick/left\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Gamepad\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"left\",\r\n                    \"id\": \"712e721c-bdfb-4b23-a86c-a0d9fcfea921\",\r\n                    \"path\": \"<Gamepad>/rightStick/left\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Gamepad\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"right\",\r\n                    \"id\": \"fcd248ae-a788-4676-a12e-f4d81205600b\",\r\n                    \"path\": \"<Gamepad>/leftStick/right\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Gamepad\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"right\",\r\n                    \"id\": \"1f04d9bc-c50b-41a1-bfcc-afb75475ec20\",\r\n                    \"path\": \"<Gamepad>/rightStick/right\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Gamepad\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"fb8277d4-c5cd-4663-9dc7-ee3f0b506d90\",\r\n                    \"path\": \"<Gamepad>/dpad\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Gamepad\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"Joystick\",\r\n                    \"id\": \"e25d9774-381c-4a61-b47c-7b6b299ad9f9\",\r\n                    \"path\": \"2DVector\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": true,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"3db53b26-6601-41be-9887-63ac74e79d19\",\r\n                    \"path\": \"<Joystick>/stick/up\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Joystick\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"0cb3e13e-3d90-4178-8ae6-d9c5501d653f\",\r\n                    \"path\": \"<Joystick>/stick/down\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Joystick\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                [...string is too long...]");
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

	// Token: 0x0600000C RID: 12 RVA: 0x0000233B File Offset: 0x0000053B
	public void Dispose()
	{
		UnityEngine.Object.Destroy(this.asset);
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600000D RID: 13 RVA: 0x00002348 File Offset: 0x00000548
	// (set) Token: 0x0600000E RID: 14 RVA: 0x00002355 File Offset: 0x00000555
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

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600000F RID: 15 RVA: 0x00002363 File Offset: 0x00000563
	// (set) Token: 0x06000010 RID: 16 RVA: 0x00002370 File Offset: 0x00000570
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

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000011 RID: 17 RVA: 0x0000237E File Offset: 0x0000057E
	public ReadOnlyArray<InputControlScheme> controlSchemes
	{
		get
		{
			return this.asset.controlSchemes;
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x0000238B File Offset: 0x0000058B
	public bool Contains(InputAction action)
	{
		return this.asset.Contains(action);
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002399 File Offset: 0x00000599
	public IEnumerator<InputAction> GetEnumerator()
	{
		return this.asset.GetEnumerator();
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000023A6 File Offset: 0x000005A6
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000023AE File Offset: 0x000005AE
	public void Enable()
	{
		this.asset.Enable();
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000023BB File Offset: 0x000005BB
	public void Disable()
	{
		this.asset.Disable();
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000017 RID: 23 RVA: 0x000023C8 File Offset: 0x000005C8
	public IEnumerable<InputBinding> bindings
	{
		get
		{
			return this.asset.bindings;
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x000023D5 File Offset: 0x000005D5
	public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
	{
		return this.asset.FindAction(actionNameOrId, throwIfNotFound);
	}

	// Token: 0x06000019 RID: 25 RVA: 0x000023E4 File Offset: 0x000005E4
	public int FindBinding(InputBinding bindingMask, out InputAction action)
	{
		return this.asset.FindBinding(bindingMask, out action);
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600001A RID: 26 RVA: 0x000023F3 File Offset: 0x000005F3
	public MenuInputActions.PlayerActions Player
	{
		get
		{
			return new MenuInputActions.PlayerActions(this);
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600001B RID: 27 RVA: 0x000023FB File Offset: 0x000005FB
	public MenuInputActions.UIActions UI
	{
		get
		{
			return new MenuInputActions.UIActions(this);
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600001C RID: 28 RVA: 0x00002404 File Offset: 0x00000604
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

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600001D RID: 29 RVA: 0x0000244C File Offset: 0x0000064C
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

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x0600001E RID: 30 RVA: 0x00002494 File Offset: 0x00000694
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

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x0600001F RID: 31 RVA: 0x000024DC File Offset: 0x000006DC
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

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000020 RID: 32 RVA: 0x00002524 File Offset: 0x00000724
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

	// Token: 0x04000005 RID: 5
	private readonly InputActionMap m_Player;

	// Token: 0x04000006 RID: 6
	private MenuInputActions.IPlayerActions m_PlayerActionsCallbackInterface;

	// Token: 0x04000007 RID: 7
	private readonly InputAction m_Player_Move;

	// Token: 0x04000008 RID: 8
	private readonly InputAction m_Player_Look;

	// Token: 0x04000009 RID: 9
	private readonly InputAction m_Player_Fire;

	// Token: 0x0400000A RID: 10
	private readonly InputActionMap m_UI;

	// Token: 0x0400000B RID: 11
	private MenuInputActions.IUIActions m_UIActionsCallbackInterface;

	// Token: 0x0400000C RID: 12
	private readonly InputAction m_UI_Navigate;

	// Token: 0x0400000D RID: 13
	private readonly InputAction m_UI_Submit;

	// Token: 0x0400000E RID: 14
	private readonly InputAction m_UI_Cancel;

	// Token: 0x0400000F RID: 15
	private readonly InputAction m_UI_Point;

	// Token: 0x04000010 RID: 16
	private readonly InputAction m_UI_Click;

	// Token: 0x04000011 RID: 17
	private readonly InputAction m_UI_ScrollWheel;

	// Token: 0x04000012 RID: 18
	private readonly InputAction m_UI_MiddleClick;

	// Token: 0x04000013 RID: 19
	private readonly InputAction m_UI_RightClick;

	// Token: 0x04000014 RID: 20
	private readonly InputAction m_UI_TrackedDevicePosition;

	// Token: 0x04000015 RID: 21
	private readonly InputAction m_UI_TrackedDeviceOrientation;

	// Token: 0x04000016 RID: 22
	private int m_KeyboardMouseSchemeIndex = -1;

	// Token: 0x04000017 RID: 23
	private int m_GamepadSchemeIndex = -1;

	// Token: 0x04000018 RID: 24
	private int m_TouchSchemeIndex = -1;

	// Token: 0x04000019 RID: 25
	private int m_JoystickSchemeIndex = -1;

	// Token: 0x0400001A RID: 26
	private int m_XRSchemeIndex = -1;

	// Token: 0x02000006 RID: 6
	public struct PlayerActions
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002569 File Offset: 0x00000769
		public PlayerActions(MenuInputActions wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002572 File Offset: 0x00000772
		public InputAction Move
		{
			get
			{
				return this.m_Wrapper.m_Player_Move;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000257F File Offset: 0x0000077F
		public InputAction Look
		{
			get
			{
				return this.m_Wrapper.m_Player_Look;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000024 RID: 36 RVA: 0x0000258C File Offset: 0x0000078C
		public InputAction Fire
		{
			get
			{
				return this.m_Wrapper.m_Player_Fire;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002599 File Offset: 0x00000799
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_Player;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000025A6 File Offset: 0x000007A6
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000025B3 File Offset: 0x000007B3
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000025C0 File Offset: 0x000007C0
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000025CD File Offset: 0x000007CD
		public static implicit operator InputActionMap(MenuInputActions.PlayerActions set)
		{
			return set.Get();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000025D8 File Offset: 0x000007D8
		public void SetCallbacks(MenuInputActions.IPlayerActions instance)
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

		// Token: 0x0400001B RID: 27
		private MenuInputActions m_Wrapper;
	}

	// Token: 0x02000007 RID: 7
	public struct UIActions
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00002811 File Offset: 0x00000A11
		public UIActions(MenuInputActions wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600002C RID: 44 RVA: 0x0000281A File Offset: 0x00000A1A
		public InputAction Navigate
		{
			get
			{
				return this.m_Wrapper.m_UI_Navigate;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002827 File Offset: 0x00000A27
		public InputAction Submit
		{
			get
			{
				return this.m_Wrapper.m_UI_Submit;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002834 File Offset: 0x00000A34
		public InputAction Cancel
		{
			get
			{
				return this.m_Wrapper.m_UI_Cancel;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002841 File Offset: 0x00000A41
		public InputAction Point
		{
			get
			{
				return this.m_Wrapper.m_UI_Point;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000030 RID: 48 RVA: 0x0000284E File Offset: 0x00000A4E
		public InputAction Click
		{
			get
			{
				return this.m_Wrapper.m_UI_Click;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000285B File Offset: 0x00000A5B
		public InputAction ScrollWheel
		{
			get
			{
				return this.m_Wrapper.m_UI_ScrollWheel;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002868 File Offset: 0x00000A68
		public InputAction MiddleClick
		{
			get
			{
				return this.m_Wrapper.m_UI_MiddleClick;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002875 File Offset: 0x00000A75
		public InputAction RightClick
		{
			get
			{
				return this.m_Wrapper.m_UI_RightClick;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002882 File Offset: 0x00000A82
		public InputAction TrackedDevicePosition
		{
			get
			{
				return this.m_Wrapper.m_UI_TrackedDevicePosition;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000035 RID: 53 RVA: 0x0000288F File Offset: 0x00000A8F
		public InputAction TrackedDeviceOrientation
		{
			get
			{
				return this.m_Wrapper.m_UI_TrackedDeviceOrientation;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000289C File Offset: 0x00000A9C
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_UI;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000028A9 File Offset: 0x00000AA9
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000028B6 File Offset: 0x00000AB6
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000028C3 File Offset: 0x00000AC3
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000028D0 File Offset: 0x00000AD0
		public static implicit operator InputActionMap(MenuInputActions.UIActions set)
		{
			return set.Get();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000028DC File Offset: 0x00000ADC
		public void SetCallbacks(MenuInputActions.IUIActions instance)
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

		// Token: 0x0400001C RID: 28
		private MenuInputActions m_Wrapper;
	}

	// Token: 0x02000008 RID: 8
	public interface IPlayerActions
	{
		// Token: 0x0600003C RID: 60
		void OnMove(InputAction.CallbackContext context);

		// Token: 0x0600003D RID: 61
		void OnLook(InputAction.CallbackContext context);

		// Token: 0x0600003E RID: 62
		void OnFire(InputAction.CallbackContext context);
	}

	// Token: 0x02000009 RID: 9
	public interface IUIActions
	{
		// Token: 0x0600003F RID: 63
		void OnNavigate(InputAction.CallbackContext context);

		// Token: 0x06000040 RID: 64
		void OnSubmit(InputAction.CallbackContext context);

		// Token: 0x06000041 RID: 65
		void OnCancel(InputAction.CallbackContext context);

		// Token: 0x06000042 RID: 66
		void OnPoint(InputAction.CallbackContext context);

		// Token: 0x06000043 RID: 67
		void OnClick(InputAction.CallbackContext context);

		// Token: 0x06000044 RID: 68
		void OnScrollWheel(InputAction.CallbackContext context);

		// Token: 0x06000045 RID: 69
		void OnMiddleClick(InputAction.CallbackContext context);

		// Token: 0x06000046 RID: 70
		void OnRightClick(InputAction.CallbackContext context);

		// Token: 0x06000047 RID: 71
		void OnTrackedDevicePosition(InputAction.CallbackContext context);

		// Token: 0x06000048 RID: 72
		void OnTrackedDeviceOrientation(InputAction.CallbackContext context);
	}
}
