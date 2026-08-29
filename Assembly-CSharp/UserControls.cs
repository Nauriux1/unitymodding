using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

// Token: 0x0200000F RID: 15
public class UserControls : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
{
	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000088 RID: 136 RVA: 0x00003E1B File Offset: 0x0000201B
	public InputActionAsset asset { get; }

	// Token: 0x06000089 RID: 137 RVA: 0x00003E24 File Offset: 0x00002024
	public UserControls()
	{
		this.asset = InputActionAsset.FromJson("{\n    \"name\": \"UserControls\",\n    \"maps\": [\n        {\n            \"name\": \"PlayerActionMap\",\n            \"id\": \"1f4b11a7-501d-4ecb-93e4-c55fa1bb3f01\",\n            \"actions\": [\n                {\n                    \"name\": \"Move_Forward\",\n                    \"type\": \"Value\",\n                    \"id\": \"704244a9-439e-4c72-af83-bef2db4c9912\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Move_Back\",\n                    \"type\": \"Value\",\n                    \"id\": \"1cbb818f-7a30-4344-b3db-a084bb81989b\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Move_Left\",\n                    \"type\": \"Value\",\n                    \"id\": \"ed4d8196-39ea-4611-850f-d0ec925237bc\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Move_Right\",\n                    \"type\": \"Value\",\n                    \"id\": \"b602a61c-625a-44e8-905d-cce937d313ae\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Turn_Left\",\n                    \"type\": \"Value\",\n                    \"id\": \"797b9f03-c76b-4b96-8a8a-c4bf3ca87b80\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Turn_Right\",\n                    \"type\": \"Value\",\n                    \"id\": \"c3f59557-7ea7-4ce1-8b67-ba1ec26a2f9a\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Turn_Up\",\n                    \"type\": \"Value\",\n                    \"id\": \"32bf7456-a39d-4b04-94a3-f26faaa86098\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Turn_Down\",\n                    \"type\": \"Value\",\n                    \"id\": \"0d20a618-ed23-4ebb-857c-7f16f552d704\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Turn_Mouse_Vertical\",\n                    \"type\": \"Value\",\n                    \"id\": \"a57c5426-3edc-4494-8abd-fc4adb02be69\",\n                    \"expectedControlType\": \"Axis\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Turn_Mouse_Horizontal\",\n                    \"type\": \"Value\",\n                    \"id\": \"a2cbb7fb-2b16-498e-bb36-3688f8aeb3ea\",\n                    \"expectedControlType\": \"Axis\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Action1\",\n                    \"type\": \"Button\",\n                    \"id\": \"0eaa7d8e-92ec-4506-8113-52f370c02981\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Action2\",\n                    \"type\": \"Button\",\n                    \"id\": \"275e4ad6-85c7-4bdd-8846-51760ec5bdfa\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Action3\",\n                    \"type\": \"Button\",\n                    \"id\": \"985c4816-c280-48ff-bc34-bc389ec644f0\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Action4\",\n                    \"type\": \"Button\",\n                    \"id\": \"16511834-e73f-41eb-9590-b01215752a24\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Action5\",\n                    \"type\": \"Button\",\n                    \"id\": \"5120225e-6503-4904-99eb-ce21ca1fdfa9\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Action6\",\n                    \"type\": \"Button\",\n                    \"id\": \"44efc5f4-00d1-4dfe-9fb8-0534db3c9bcf\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Action7\",\n                    \"type\": \"Button\",\n                    \"id\": \"b3522a1c-50e3-422f-8479-5067684de9af\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Action8\",\n                    \"type\": \"Button\",\n                    \"id\": \"23a14d1f-a374-4983-87d0-67075e39ba6a\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Action9\",\n                    \"type\": \"Button\",\n                    \"id\": \"3c079c9e-ad9d-4f37-945a-3346841ed4ac\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Action10\",\n                    \"type\": \"Button\",\n                    \"id\": \"3bc3b956-c8d2-4849-9055-6d9f318367e1\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Action11\",\n                    \"type\": \"Button\",\n                    \"id\": \"542b22c4-a4f5-422f-9767-616cf7e58dba\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Action12\",\n                    \"type\": \"Button\",\n                    \"id\": \"1999f091-9be2-4009-8991-2dff5987b4fa\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Action13\",\n                    \"type\": \"Button\",\n                    \"id\": \"9d1f14d2-909c-4676-815e-c4b2f408693f\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Action14\",\n                    \"type\": \"Button\",\n                    \"id\": \"69ce9570-be9b-4e7c-add6-500b3eb81e61\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Action15\",\n                    \"type\": \"Button\",\n                    \"id\": \"b4306a45-2a6d-48cb-9af3-614b44044cfc\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Directional_Action1\",\n                    \"type\": \"Button\",\n                    \"id\": \"d92ff75e-8bed-45ac-9024-728108371ccd\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Directional_Action2\",\n                    \"type\": \"Button\",\n                    \"id\": \"9972d5df-ac56-461d-8cb4-9b69a3f83d8f\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"AxisDeadzone\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"\",\n                    \"id\": \"75f106f7-803c-4a4c-9346-159d9a42f0bc\",\n                    \"path\": \"<Gamepad>/leftStick/up\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move_Forward\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"143da520-8e97-4184-aec7-3e8132dcde36\",\n                    \"path\": \"<Gamepad>/dpad/up\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move_Forward\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"3c9beb2d-220c-4f16-af32-a4c6cac376e9\",\n                    \"path\": \"<Keyboard>/w\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move_Forward\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"1cbe2b84-e780-4dfb-939e-0b4b5c639aa8\",\n                    \"path\": \"<Gamepad>/leftStick/down\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move_Back\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"1991e90b-a4e1-4917-9d36-1ab8cd9a5091\",\n                    \"path\": \"<Gamepad>/dpad/down\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move_Back\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"f1884faa-abf8-47ac-bc0c-47823b1f58eb\",\n                    \"path\": \"<Keyboard>/s\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move_Back\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"f4fcd440-d0b1-4d7d-a9f0-1859d23e7490\",\n                    \"path\": \"<Gamepad>/leftStick/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move_Left\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"5a561ffd-f932-4712-9d16-18bc40c30f00\",\n                    \"path\": \"<Gamepad>/dpad/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move_Left\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"26df006d-bddd-455f-a11c-61c5934952c5\",\n                    \"path\": \"<Keyboard>/a\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move_Left\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"6ef7bc2f-0f91-4598-b632-d09fc6b94c9e\",\n                    \"path\": \"<Gamepad>/leftStick/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move_Right\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"685f9b85-e5db-4f39-812b-0166468076ec\",\n                    \"path\": \"<Gamepad>/dpad/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move_Right\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"6fe19cfa-158d-41e3-93fd-2ebfc727b2ff\",\n                    \"path\": \"<Keyboard>/d\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move_Right\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"03858db0-7b73-4ada-9e34-4bad25e0607b\",\n                    \"path\": \"<Gamepad>/rightStick/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Turn_Left\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"8185b953-d3a2-43a1-a1e0-9b9e2d88b963\",\n                    \"path\": \"<Keyboard>/q\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Turn_Left\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"a510f5f2-8504-4181-9891-e65cb3cc71b1\",\n                    \"path\": \"<Gamepad>/rightStick/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Turn_Right\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c78361fd-cfe1-4b54-9171-25649372e853\",\n                    \"path\": \"<Keyboard>/e\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Turn_Right\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"66bb01a5-954e-4133-bd5a-66c6ea2fbd6d\",\n                    \"path\": \"<Gamepad>/buttonWest\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Action1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"65edd9d9-5f70-453d-b46b-6cdbe5855fea\",\n                    \"path\": \"<Keyboard>/j\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Action1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"6526e8e1-f0ae-4e8f-b88e-76f2b0fdbb6e\",\n                    \"path\": \"<Gamepad>/rightTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Action2\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"4de8ad6c-745f-47fb-9026-762d513fcd96\",\n                    \"path\": \"<Keyboard>/k\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Action2\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"af2c93d2-b51e-4028-8234-7a49a7d0aee1\",\n                    \"path\": \"<Gamepad>/buttonEast\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Action3\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"b2d2522e-2c05-40b5-a817-19932fa1d935\",\n                    \"path\": \"<Keyboard>/l\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Action3\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"cec7a60b-2458-4768-a0da-0c9d12ab1e51\",\n                    \"path\": \"<Gamepad>/leftShoulder\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Action4\",\n                    \"isComposite\": fals[...string is too long...]");
		this.m_PlayerActionMap = this.asset.FindActionMap("PlayerActionMap", true);
		this.m_PlayerActionMap_Move_Forward = this.m_PlayerActionMap.FindAction("Move_Forward", true);
		this.m_PlayerActionMap_Move_Back = this.m_PlayerActionMap.FindAction("Move_Back", true);
		this.m_PlayerActionMap_Move_Left = this.m_PlayerActionMap.FindAction("Move_Left", true);
		this.m_PlayerActionMap_Move_Right = this.m_PlayerActionMap.FindAction("Move_Right", true);
		this.m_PlayerActionMap_Turn_Left = this.m_PlayerActionMap.FindAction("Turn_Left", true);
		this.m_PlayerActionMap_Turn_Right = this.m_PlayerActionMap.FindAction("Turn_Right", true);
		this.m_PlayerActionMap_Turn_Up = this.m_PlayerActionMap.FindAction("Turn_Up", true);
		this.m_PlayerActionMap_Turn_Down = this.m_PlayerActionMap.FindAction("Turn_Down", true);
		this.m_PlayerActionMap_Turn_Mouse_Vertical = this.m_PlayerActionMap.FindAction("Turn_Mouse_Vertical", true);
		this.m_PlayerActionMap_Turn_Mouse_Horizontal = this.m_PlayerActionMap.FindAction("Turn_Mouse_Horizontal", true);
		this.m_PlayerActionMap_Action1 = this.m_PlayerActionMap.FindAction("Action1", true);
		this.m_PlayerActionMap_Action2 = this.m_PlayerActionMap.FindAction("Action2", true);
		this.m_PlayerActionMap_Action3 = this.m_PlayerActionMap.FindAction("Action3", true);
		this.m_PlayerActionMap_Action4 = this.m_PlayerActionMap.FindAction("Action4", true);
		this.m_PlayerActionMap_Action5 = this.m_PlayerActionMap.FindAction("Action5", true);
		this.m_PlayerActionMap_Action6 = this.m_PlayerActionMap.FindAction("Action6", true);
		this.m_PlayerActionMap_Action7 = this.m_PlayerActionMap.FindAction("Action7", true);
		this.m_PlayerActionMap_Action8 = this.m_PlayerActionMap.FindAction("Action8", true);
		this.m_PlayerActionMap_Action9 = this.m_PlayerActionMap.FindAction("Action9", true);
		this.m_PlayerActionMap_Action10 = this.m_PlayerActionMap.FindAction("Action10", true);
		this.m_PlayerActionMap_Action11 = this.m_PlayerActionMap.FindAction("Action11", true);
		this.m_PlayerActionMap_Action12 = this.m_PlayerActionMap.FindAction("Action12", true);
		this.m_PlayerActionMap_Action13 = this.m_PlayerActionMap.FindAction("Action13", true);
		this.m_PlayerActionMap_Action14 = this.m_PlayerActionMap.FindAction("Action14", true);
		this.m_PlayerActionMap_Action15 = this.m_PlayerActionMap.FindAction("Action15", true);
		this.m_PlayerActionMap_Directional_Action1 = this.m_PlayerActionMap.FindAction("Directional_Action1", true);
		this.m_PlayerActionMap_Directional_Action2 = this.m_PlayerActionMap.FindAction("Directional_Action2", true);
		this.m_MoveEditorMap = this.asset.FindActionMap("MoveEditorMap", true);
		this.m_MoveEditorMap_Save_Move = this.m_MoveEditorMap.FindAction("Save_Move", true);
		this.m_MoveEditorMap_Delete = this.m_MoveEditorMap.FindAction("Delete", true);
		this.m_MoveEditorMap_Focus_Camera = this.m_MoveEditorMap.FindAction("Focus_Camera", true);
		this.m_MoveEditorMap_Left_Click = this.m_MoveEditorMap.FindAction("Left_Click", true);
		this.m_MoveEditorMap_Right_Click = this.m_MoveEditorMap.FindAction("Right_Click", true);
		this.m_MoveEditorMap_Drag_Select = this.m_MoveEditorMap.FindAction("Drag_Select", true);
		this.m_MoveEditorMap_Copy = this.m_MoveEditorMap.FindAction("Copy", true);
		this.m_MoveEditorMap_Paste = this.m_MoveEditorMap.FindAction("Paste", true);
		this.m_MoveEditorMap_Back = this.m_MoveEditorMap.FindAction("Back", true);
		this.m_MoveEditorMap_EditMode_Rotate = this.m_MoveEditorMap.FindAction("EditMode_Rotate", true);
		this.m_MoveEditorMap_EditMode_Move = this.m_MoveEditorMap.FindAction("EditMode_Move", true);
		this.m_MoveEditorMap_Undo = this.m_MoveEditorMap.FindAction("Undo", true);
		this.m_MoveEditorMap_Redo = this.m_MoveEditorMap.FindAction("Redo", true);
		this.m_MoveEditorMap_Save = this.m_MoveEditorMap.FindAction("Save", true);
		this.m_ReplayMap = this.asset.FindActionMap("ReplayMap", true);
		this.m_ReplayMap_Move_Up = this.m_ReplayMap.FindAction("Move_Up", true);
		this.m_ReplayMap_Move_Down = this.m_ReplayMap.FindAction("Move_Down", true);
		this.m_ReplayMap_ToggleToolbarVisibility = this.m_ReplayMap.FindAction("ToggleToolbarVisibility", true);
		this.m_ReplayMap_TogglePlay = this.m_ReplayMap.FindAction("TogglePlay", true);
		this.m_ReplayMap_SetReplaySpeed1 = this.m_ReplayMap.FindAction("SetReplaySpeed1", true);
		this.m_ReplayMap_SetReplaySpeed2 = this.m_ReplayMap.FindAction("SetReplaySpeed2", true);
		this.m_ReplayMap_SetReplaySpeed3 = this.m_ReplayMap.FindAction("SetReplaySpeed3", true);
		this.m_ReplayMap_SetReplaySpeed4 = this.m_ReplayMap.FindAction("SetReplaySpeed4", true);
		this.m_ReplayMap_SetReplaySpeed5 = this.m_ReplayMap.FindAction("SetReplaySpeed5", true);
		this.m_ReplayMap_SetReplaySpeed6 = this.m_ReplayMap.FindAction("SetReplaySpeed6", true);
		this.m_ReplayMap_SetCameraMode1 = this.m_ReplayMap.FindAction("SetCameraMode1", true);
		this.m_ReplayMap_SetCameraMode2 = this.m_ReplayMap.FindAction("SetCameraMode2", true);
		this.m_ReplayMap_SetCameraMode3 = this.m_ReplayMap.FindAction("SetCameraMode3", true);
		this.m_ReplayMap_PreviousPlayer = this.m_ReplayMap.FindAction("PreviousPlayer", true);
		this.m_ReplayMap_NextPlayer = this.m_ReplayMap.FindAction("NextPlayer", true);
		this.m_Generic = this.asset.FindActionMap("Generic", true);
		this.m_Generic_OpenMenu = this.m_Generic.FindAction("OpenMenu", true);
		this.m_Generic_Back = this.m_Generic.FindAction("Back", true);
		this.m_Generic_ControllerAlternativeClick = this.m_Generic.FindAction("ControllerAlternativeClick", true);
		this.m_Generic_Left_Click_Modifier_Or_Middle = this.m_Generic.FindAction("Left_Click_Modifier_Or_Middle", true);
		this.m_Generic_Modifier = this.m_Generic.FindAction("Modifier", true);
		this.m_General = this.asset.FindActionMap("General", true);
		this.m_General_Chat = this.m_General.FindAction("Chat", true);
		this.m_General_Restart = this.m_General.FindAction("Restart", true);
		this.m_General_SaveReplay = this.m_General.FindAction("SaveReplay", true);
		this.m_General_PushToTalk = this.m_General.FindAction("PushToTalk", true);
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00004491 File Offset: 0x00002691
	public void Dispose()
	{
		UnityEngine.Object.Destroy(this.asset);
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x0600008B RID: 139 RVA: 0x0000449E File Offset: 0x0000269E
	// (set) Token: 0x0600008C RID: 140 RVA: 0x000044AB File Offset: 0x000026AB
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

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x0600008D RID: 141 RVA: 0x000044B9 File Offset: 0x000026B9
	// (set) Token: 0x0600008E RID: 142 RVA: 0x000044C6 File Offset: 0x000026C6
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

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x0600008F RID: 143 RVA: 0x000044D4 File Offset: 0x000026D4
	public ReadOnlyArray<InputControlScheme> controlSchemes
	{
		get
		{
			return this.asset.controlSchemes;
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x000044E1 File Offset: 0x000026E1
	public bool Contains(InputAction action)
	{
		return this.asset.Contains(action);
	}

	// Token: 0x06000091 RID: 145 RVA: 0x000044EF File Offset: 0x000026EF
	public IEnumerator<InputAction> GetEnumerator()
	{
		return this.asset.GetEnumerator();
	}

	// Token: 0x06000092 RID: 146 RVA: 0x000044FC File Offset: 0x000026FC
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00004504 File Offset: 0x00002704
	public void Enable()
	{
		this.asset.Enable();
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00004511 File Offset: 0x00002711
	public void Disable()
	{
		this.asset.Disable();
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000095 RID: 149 RVA: 0x0000451E File Offset: 0x0000271E
	public IEnumerable<InputBinding> bindings
	{
		get
		{
			return this.asset.bindings;
		}
	}

	// Token: 0x06000096 RID: 150 RVA: 0x0000452B File Offset: 0x0000272B
	public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
	{
		return this.asset.FindAction(actionNameOrId, throwIfNotFound);
	}

	// Token: 0x06000097 RID: 151 RVA: 0x0000453A File Offset: 0x0000273A
	public int FindBinding(InputBinding bindingMask, out InputAction action)
	{
		return this.asset.FindBinding(bindingMask, out action);
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x06000098 RID: 152 RVA: 0x00004549 File Offset: 0x00002749
	public UserControls.PlayerActionMapActions PlayerActionMap
	{
		get
		{
			return new UserControls.PlayerActionMapActions(this);
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000099 RID: 153 RVA: 0x00004551 File Offset: 0x00002751
	public UserControls.MoveEditorMapActions MoveEditorMap
	{
		get
		{
			return new UserControls.MoveEditorMapActions(this);
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x0600009A RID: 154 RVA: 0x00004559 File Offset: 0x00002759
	public UserControls.ReplayMapActions ReplayMap
	{
		get
		{
			return new UserControls.ReplayMapActions(this);
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x0600009B RID: 155 RVA: 0x00004561 File Offset: 0x00002761
	public UserControls.GenericActions Generic
	{
		get
		{
			return new UserControls.GenericActions(this);
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x0600009C RID: 156 RVA: 0x00004569 File Offset: 0x00002769
	public UserControls.GeneralActions General
	{
		get
		{
			return new UserControls.GeneralActions(this);
		}
	}

	// Token: 0x04000037 RID: 55
	private readonly InputActionMap m_PlayerActionMap;

	// Token: 0x04000038 RID: 56
	private UserControls.IPlayerActionMapActions m_PlayerActionMapActionsCallbackInterface;

	// Token: 0x04000039 RID: 57
	private readonly InputAction m_PlayerActionMap_Move_Forward;

	// Token: 0x0400003A RID: 58
	private readonly InputAction m_PlayerActionMap_Move_Back;

	// Token: 0x0400003B RID: 59
	private readonly InputAction m_PlayerActionMap_Move_Left;

	// Token: 0x0400003C RID: 60
	private readonly InputAction m_PlayerActionMap_Move_Right;

	// Token: 0x0400003D RID: 61
	private readonly InputAction m_PlayerActionMap_Turn_Left;

	// Token: 0x0400003E RID: 62
	private readonly InputAction m_PlayerActionMap_Turn_Right;

	// Token: 0x0400003F RID: 63
	private readonly InputAction m_PlayerActionMap_Turn_Up;

	// Token: 0x04000040 RID: 64
	private readonly InputAction m_PlayerActionMap_Turn_Down;

	// Token: 0x04000041 RID: 65
	private readonly InputAction m_PlayerActionMap_Turn_Mouse_Vertical;

	// Token: 0x04000042 RID: 66
	private readonly InputAction m_PlayerActionMap_Turn_Mouse_Horizontal;

	// Token: 0x04000043 RID: 67
	private readonly InputAction m_PlayerActionMap_Action1;

	// Token: 0x04000044 RID: 68
	private readonly InputAction m_PlayerActionMap_Action2;

	// Token: 0x04000045 RID: 69
	private readonly InputAction m_PlayerActionMap_Action3;

	// Token: 0x04000046 RID: 70
	private readonly InputAction m_PlayerActionMap_Action4;

	// Token: 0x04000047 RID: 71
	private readonly InputAction m_PlayerActionMap_Action5;

	// Token: 0x04000048 RID: 72
	private readonly InputAction m_PlayerActionMap_Action6;

	// Token: 0x04000049 RID: 73
	private readonly InputAction m_PlayerActionMap_Action7;

	// Token: 0x0400004A RID: 74
	private readonly InputAction m_PlayerActionMap_Action8;

	// Token: 0x0400004B RID: 75
	private readonly InputAction m_PlayerActionMap_Action9;

	// Token: 0x0400004C RID: 76
	private readonly InputAction m_PlayerActionMap_Action10;

	// Token: 0x0400004D RID: 77
	private readonly InputAction m_PlayerActionMap_Action11;

	// Token: 0x0400004E RID: 78
	private readonly InputAction m_PlayerActionMap_Action12;

	// Token: 0x0400004F RID: 79
	private readonly InputAction m_PlayerActionMap_Action13;

	// Token: 0x04000050 RID: 80
	private readonly InputAction m_PlayerActionMap_Action14;

	// Token: 0x04000051 RID: 81
	private readonly InputAction m_PlayerActionMap_Action15;

	// Token: 0x04000052 RID: 82
	private readonly InputAction m_PlayerActionMap_Directional_Action1;

	// Token: 0x04000053 RID: 83
	private readonly InputAction m_PlayerActionMap_Directional_Action2;

	// Token: 0x04000054 RID: 84
	private readonly InputActionMap m_MoveEditorMap;

	// Token: 0x04000055 RID: 85
	private UserControls.IMoveEditorMapActions m_MoveEditorMapActionsCallbackInterface;

	// Token: 0x04000056 RID: 86
	private readonly InputAction m_MoveEditorMap_Save_Move;

	// Token: 0x04000057 RID: 87
	private readonly InputAction m_MoveEditorMap_Delete;

	// Token: 0x04000058 RID: 88
	private readonly InputAction m_MoveEditorMap_Focus_Camera;

	// Token: 0x04000059 RID: 89
	private readonly InputAction m_MoveEditorMap_Left_Click;

	// Token: 0x0400005A RID: 90
	private readonly InputAction m_MoveEditorMap_Right_Click;

	// Token: 0x0400005B RID: 91
	private readonly InputAction m_MoveEditorMap_Drag_Select;

	// Token: 0x0400005C RID: 92
	private readonly InputAction m_MoveEditorMap_Copy;

	// Token: 0x0400005D RID: 93
	private readonly InputAction m_MoveEditorMap_Paste;

	// Token: 0x0400005E RID: 94
	private readonly InputAction m_MoveEditorMap_Back;

	// Token: 0x0400005F RID: 95
	private readonly InputAction m_MoveEditorMap_EditMode_Rotate;

	// Token: 0x04000060 RID: 96
	private readonly InputAction m_MoveEditorMap_EditMode_Move;

	// Token: 0x04000061 RID: 97
	private readonly InputAction m_MoveEditorMap_Undo;

	// Token: 0x04000062 RID: 98
	private readonly InputAction m_MoveEditorMap_Redo;

	// Token: 0x04000063 RID: 99
	private readonly InputAction m_MoveEditorMap_Save;

	// Token: 0x04000064 RID: 100
	private readonly InputActionMap m_ReplayMap;

	// Token: 0x04000065 RID: 101
	private UserControls.IReplayMapActions m_ReplayMapActionsCallbackInterface;

	// Token: 0x04000066 RID: 102
	private readonly InputAction m_ReplayMap_Move_Up;

	// Token: 0x04000067 RID: 103
	private readonly InputAction m_ReplayMap_Move_Down;

	// Token: 0x04000068 RID: 104
	private readonly InputAction m_ReplayMap_ToggleToolbarVisibility;

	// Token: 0x04000069 RID: 105
	private readonly InputAction m_ReplayMap_TogglePlay;

	// Token: 0x0400006A RID: 106
	private readonly InputAction m_ReplayMap_SetReplaySpeed1;

	// Token: 0x0400006B RID: 107
	private readonly InputAction m_ReplayMap_SetReplaySpeed2;

	// Token: 0x0400006C RID: 108
	private readonly InputAction m_ReplayMap_SetReplaySpeed3;

	// Token: 0x0400006D RID: 109
	private readonly InputAction m_ReplayMap_SetReplaySpeed4;

	// Token: 0x0400006E RID: 110
	private readonly InputAction m_ReplayMap_SetReplaySpeed5;

	// Token: 0x0400006F RID: 111
	private readonly InputAction m_ReplayMap_SetReplaySpeed6;

	// Token: 0x04000070 RID: 112
	private readonly InputAction m_ReplayMap_SetCameraMode1;

	// Token: 0x04000071 RID: 113
	private readonly InputAction m_ReplayMap_SetCameraMode2;

	// Token: 0x04000072 RID: 114
	private readonly InputAction m_ReplayMap_SetCameraMode3;

	// Token: 0x04000073 RID: 115
	private readonly InputAction m_ReplayMap_PreviousPlayer;

	// Token: 0x04000074 RID: 116
	private readonly InputAction m_ReplayMap_NextPlayer;

	// Token: 0x04000075 RID: 117
	private readonly InputActionMap m_Generic;

	// Token: 0x04000076 RID: 118
	private UserControls.IGenericActions m_GenericActionsCallbackInterface;

	// Token: 0x04000077 RID: 119
	private readonly InputAction m_Generic_OpenMenu;

	// Token: 0x04000078 RID: 120
	private readonly InputAction m_Generic_Back;

	// Token: 0x04000079 RID: 121
	private readonly InputAction m_Generic_ControllerAlternativeClick;

	// Token: 0x0400007A RID: 122
	private readonly InputAction m_Generic_Left_Click_Modifier_Or_Middle;

	// Token: 0x0400007B RID: 123
	private readonly InputAction m_Generic_Modifier;

	// Token: 0x0400007C RID: 124
	private readonly InputActionMap m_General;

	// Token: 0x0400007D RID: 125
	private UserControls.IGeneralActions m_GeneralActionsCallbackInterface;

	// Token: 0x0400007E RID: 126
	private readonly InputAction m_General_Chat;

	// Token: 0x0400007F RID: 127
	private readonly InputAction m_General_Restart;

	// Token: 0x04000080 RID: 128
	private readonly InputAction m_General_SaveReplay;

	// Token: 0x04000081 RID: 129
	private readonly InputAction m_General_PushToTalk;

	// Token: 0x02000010 RID: 16
	public struct PlayerActionMapActions
	{
		// Token: 0x0600009D RID: 157 RVA: 0x00004571 File Offset: 0x00002771
		public PlayerActionMapActions(UserControls wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600009E RID: 158 RVA: 0x0000457A File Offset: 0x0000277A
		public InputAction Move_Forward
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Move_Forward;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004587 File Offset: 0x00002787
		public InputAction Move_Back
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Move_Back;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004594 File Offset: 0x00002794
		public InputAction Move_Left
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Move_Left;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000045A1 File Offset: 0x000027A1
		public InputAction Move_Right
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Move_Right;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000045AE File Offset: 0x000027AE
		public InputAction Turn_Left
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Turn_Left;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000045BB File Offset: 0x000027BB
		public InputAction Turn_Right
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Turn_Right;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000045C8 File Offset: 0x000027C8
		public InputAction Turn_Up
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Turn_Up;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000045D5 File Offset: 0x000027D5
		public InputAction Turn_Down
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Turn_Down;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000045E2 File Offset: 0x000027E2
		public InputAction Turn_Mouse_Vertical
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Turn_Mouse_Vertical;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000045EF File Offset: 0x000027EF
		public InputAction Turn_Mouse_Horizontal
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Turn_Mouse_Horizontal;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000045FC File Offset: 0x000027FC
		public InputAction Action1
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action1;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00004609 File Offset: 0x00002809
		public InputAction Action2
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action2;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004616 File Offset: 0x00002816
		public InputAction Action3
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action3;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00004623 File Offset: 0x00002823
		public InputAction Action4
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action4;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004630 File Offset: 0x00002830
		public InputAction Action5
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action5;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0000463D File Offset: 0x0000283D
		public InputAction Action6
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action6;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000AE RID: 174 RVA: 0x0000464A File Offset: 0x0000284A
		public InputAction Action7
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action7;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004657 File Offset: 0x00002857
		public InputAction Action8
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action8;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004664 File Offset: 0x00002864
		public InputAction Action9
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action9;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004671 File Offset: 0x00002871
		public InputAction Action10
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action10;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x0000467E File Offset: 0x0000287E
		public InputAction Action11
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action11;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x0000468B File Offset: 0x0000288B
		public InputAction Action12
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action12;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004698 File Offset: 0x00002898
		public InputAction Action13
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action13;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x000046A5 File Offset: 0x000028A5
		public InputAction Action14
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action14;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x000046B2 File Offset: 0x000028B2
		public InputAction Action15
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Action15;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000046BF File Offset: 0x000028BF
		public InputAction Directional_Action1
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Directional_Action1;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000046CC File Offset: 0x000028CC
		public InputAction Directional_Action2
		{
			get
			{
				return this.m_Wrapper.m_PlayerActionMap_Directional_Action2;
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000046D9 File Offset: 0x000028D9
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_PlayerActionMap;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000046E6 File Offset: 0x000028E6
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000046F3 File Offset: 0x000028F3
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004700 File Offset: 0x00002900
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000470D File Offset: 0x0000290D
		public static implicit operator InputActionMap(UserControls.PlayerActionMapActions set)
		{
			return set.Get();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004718 File Offset: 0x00002918
		public void SetCallbacks(UserControls.IPlayerActionMapActions instance)
		{
			if (this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface != null)
			{
				this.Move_Forward.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove_Forward;
				this.Move_Forward.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove_Forward;
				this.Move_Forward.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove_Forward;
				this.Move_Back.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove_Back;
				this.Move_Back.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove_Back;
				this.Move_Back.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove_Back;
				this.Move_Left.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove_Left;
				this.Move_Left.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove_Left;
				this.Move_Left.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove_Left;
				this.Move_Right.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove_Right;
				this.Move_Right.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove_Right;
				this.Move_Right.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove_Right;
				this.Turn_Left.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Left;
				this.Turn_Left.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Left;
				this.Turn_Left.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Left;
				this.Turn_Right.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Right;
				this.Turn_Right.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Right;
				this.Turn_Right.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Right;
				this.Turn_Up.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Up;
				this.Turn_Up.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Up;
				this.Turn_Up.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Up;
				this.Turn_Down.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Down;
				this.Turn_Down.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Down;
				this.Turn_Down.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Down;
				this.Turn_Mouse_Vertical.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Mouse_Vertical;
				this.Turn_Mouse_Vertical.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Mouse_Vertical;
				this.Turn_Mouse_Vertical.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Mouse_Vertical;
				this.Turn_Mouse_Horizontal.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Mouse_Horizontal;
				this.Turn_Mouse_Horizontal.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Mouse_Horizontal;
				this.Turn_Mouse_Horizontal.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnTurn_Mouse_Horizontal;
				this.Action1.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction1;
				this.Action1.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction1;
				this.Action1.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction1;
				this.Action2.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction2;
				this.Action2.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction2;
				this.Action2.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction2;
				this.Action3.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction3;
				this.Action3.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction3;
				this.Action3.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction3;
				this.Action4.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction4;
				this.Action4.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction4;
				this.Action4.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction4;
				this.Action5.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction5;
				this.Action5.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction5;
				this.Action5.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction5;
				this.Action6.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction6;
				this.Action6.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction6;
				this.Action6.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction6;
				this.Action7.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction7;
				this.Action7.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction7;
				this.Action7.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction7;
				this.Action8.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction8;
				this.Action8.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction8;
				this.Action8.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction8;
				this.Action9.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction9;
				this.Action9.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction9;
				this.Action9.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction9;
				this.Action10.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction10;
				this.Action10.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction10;
				this.Action10.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction10;
				this.Action11.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction11;
				this.Action11.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction11;
				this.Action11.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction11;
				this.Action12.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction12;
				this.Action12.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction12;
				this.Action12.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction12;
				this.Action13.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction13;
				this.Action13.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction13;
				this.Action13.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction13;
				this.Action14.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction14;
				this.Action14.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction14;
				this.Action14.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction14;
				this.Action15.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction15;
				this.Action15.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction15;
				this.Action15.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnAction15;
				this.Directional_Action1.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnDirectional_Action1;
				this.Directional_Action1.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnDirectional_Action1;
				this.Directional_Action1.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnDirectional_Action1;
				this.Directional_Action2.started -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnDirectional_Action2;
				this.Directional_Action2.performed -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnDirectional_Action2;
				this.Directional_Action2.canceled -= this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnDirectional_Action2;
			}
			this.m_Wrapper.m_PlayerActionMapActionsCallbackInterface = instance;
			if (instance != null)
			{
				this.Move_Forward.started += instance.OnMove_Forward;
				this.Move_Forward.performed += instance.OnMove_Forward;
				this.Move_Forward.canceled += instance.OnMove_Forward;
				this.Move_Back.started += instance.OnMove_Back;
				this.Move_Back.performed += instance.OnMove_Back;
				this.Move_Back.canceled += instance.OnMove_Back;
				this.Move_Left.started += instance.OnMove_Left;
				this.Move_Left.performed += instance.OnMove_Left;
				this.Move_Left.canceled += instance.OnMove_Left;
				this.Move_Right.started += instance.OnMove_Right;
				this.Move_Right.performed += instance.OnMove_Right;
				this.Move_Right.canceled += instance.OnMove_Right;
				this.Turn_Left.started += instance.OnTurn_Left;
				this.Turn_Left.performed += instance.OnTurn_Left;
				this.Turn_Left.canceled += instance.OnTurn_Left;
				this.Turn_Right.started += instance.OnTurn_Right;
				this.Turn_Right.performed += instance.OnTurn_Right;
				this.Turn_Right.canceled += instance.OnTurn_Right;
				this.Turn_Up.started += instance.OnTurn_Up;
				this.Turn_Up.performed += instance.OnTurn_Up;
				this.Turn_Up.canceled += instance.OnTurn_Up;
				this.Turn_Down.started += instance.OnTurn_Down;
				this.Turn_Down.performed += instance.OnTurn_Down;
				this.Turn_Down.canceled += instance.OnTurn_Down;
				this.Turn_Mouse_Vertical.started += instance.OnTurn_Mouse_Vertical;
				this.Turn_Mouse_Vertical.performed += instance.OnTurn_Mouse_Vertical;
				this.Turn_Mouse_Vertical.canceled += instance.OnTurn_Mouse_Vertical;
				this.Turn_Mouse_Horizontal.started += instance.OnTurn_Mouse_Horizontal;
				this.Turn_Mouse_Horizontal.performed += instance.OnTurn_Mouse_Horizontal;
				this.Turn_Mouse_Horizontal.canceled += instance.OnTurn_Mouse_Horizontal;
				this.Action1.started += instance.OnAction1;
				this.Action1.performed += instance.OnAction1;
				this.Action1.canceled += instance.OnAction1;
				this.Action2.started += instance.OnAction2;
				this.Action2.performed += instance.OnAction2;
				this.Action2.canceled += instance.OnAction2;
				this.Action3.started += instance.OnAction3;
				this.Action3.performed += instance.OnAction3;
				this.Action3.canceled += instance.OnAction3;
				this.Action4.started += instance.OnAction4;
				this.Action4.performed += instance.OnAction4;
				this.Action4.canceled += instance.OnAction4;
				this.Action5.started += instance.OnAction5;
				this.Action5.performed += instance.OnAction5;
				this.Action5.canceled += instance.OnAction5;
				this.Action6.started += instance.OnAction6;
				this.Action6.performed += instance.OnAction6;
				this.Action6.canceled += instance.OnAction6;
				this.Action7.started += instance.OnAction7;
				this.Action7.performed += instance.OnAction7;
				this.Action7.canceled += instance.OnAction7;
				this.Action8.started += instance.OnAction8;
				this.Action8.performed += instance.OnAction8;
				this.Action8.canceled += instance.OnAction8;
				this.Action9.started += instance.OnAction9;
				this.Action9.performed += instance.OnAction9;
				this.Action9.canceled += instance.OnAction9;
				this.Action10.started += instance.OnAction10;
				this.Action10.performed += instance.OnAction10;
				this.Action10.canceled += instance.OnAction10;
				this.Action11.started += instance.OnAction11;
				this.Action11.performed += instance.OnAction11;
				this.Action11.canceled += instance.OnAction11;
				this.Action12.started += instance.OnAction12;
				this.Action12.performed += instance.OnAction12;
				this.Action12.canceled += instance.OnAction12;
				this.Action13.started += instance.OnAction13;
				this.Action13.performed += instance.OnAction13;
				this.Action13.canceled += instance.OnAction13;
				this.Action14.started += instance.OnAction14;
				this.Action14.performed += instance.OnAction14;
				this.Action14.canceled += instance.OnAction14;
				this.Action15.started += instance.OnAction15;
				this.Action15.performed += instance.OnAction15;
				this.Action15.canceled += instance.OnAction15;
				this.Directional_Action1.started += instance.OnDirectional_Action1;
				this.Directional_Action1.performed += instance.OnDirectional_Action1;
				this.Directional_Action1.canceled += instance.OnDirectional_Action1;
				this.Directional_Action2.started += instance.OnDirectional_Action2;
				this.Directional_Action2.performed += instance.OnDirectional_Action2;
				this.Directional_Action2.canceled += instance.OnDirectional_Action2;
			}
		}

		// Token: 0x04000082 RID: 130
		private UserControls m_Wrapper;
	}

	// Token: 0x02000011 RID: 17
	public struct MoveEditorMapActions
	{
		// Token: 0x060000BF RID: 191 RVA: 0x000059A1 File Offset: 0x00003BA1
		public MoveEditorMapActions(UserControls wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000059AA File Offset: 0x00003BAA
		public InputAction Save_Move
		{
			get
			{
				return this.m_Wrapper.m_MoveEditorMap_Save_Move;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x000059B7 File Offset: 0x00003BB7
		public InputAction Delete
		{
			get
			{
				return this.m_Wrapper.m_MoveEditorMap_Delete;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x000059C4 File Offset: 0x00003BC4
		public InputAction Focus_Camera
		{
			get
			{
				return this.m_Wrapper.m_MoveEditorMap_Focus_Camera;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000059D1 File Offset: 0x00003BD1
		public InputAction Left_Click
		{
			get
			{
				return this.m_Wrapper.m_MoveEditorMap_Left_Click;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000059DE File Offset: 0x00003BDE
		public InputAction Right_Click
		{
			get
			{
				return this.m_Wrapper.m_MoveEditorMap_Right_Click;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x000059EB File Offset: 0x00003BEB
		public InputAction Drag_Select
		{
			get
			{
				return this.m_Wrapper.m_MoveEditorMap_Drag_Select;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x000059F8 File Offset: 0x00003BF8
		public InputAction Copy
		{
			get
			{
				return this.m_Wrapper.m_MoveEditorMap_Copy;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005A05 File Offset: 0x00003C05
		public InputAction Paste
		{
			get
			{
				return this.m_Wrapper.m_MoveEditorMap_Paste;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00005A12 File Offset: 0x00003C12
		public InputAction Back
		{
			get
			{
				return this.m_Wrapper.m_MoveEditorMap_Back;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00005A1F File Offset: 0x00003C1F
		public InputAction EditMode_Rotate
		{
			get
			{
				return this.m_Wrapper.m_MoveEditorMap_EditMode_Rotate;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00005A2C File Offset: 0x00003C2C
		public InputAction EditMode_Move
		{
			get
			{
				return this.m_Wrapper.m_MoveEditorMap_EditMode_Move;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00005A39 File Offset: 0x00003C39
		public InputAction Undo
		{
			get
			{
				return this.m_Wrapper.m_MoveEditorMap_Undo;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00005A46 File Offset: 0x00003C46
		public InputAction Redo
		{
			get
			{
				return this.m_Wrapper.m_MoveEditorMap_Redo;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00005A53 File Offset: 0x00003C53
		public InputAction Save
		{
			get
			{
				return this.m_Wrapper.m_MoveEditorMap_Save;
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005A60 File Offset: 0x00003C60
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_MoveEditorMap;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005A6D File Offset: 0x00003C6D
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005A7A File Offset: 0x00003C7A
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00005A87 File Offset: 0x00003C87
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005A94 File Offset: 0x00003C94
		public static implicit operator InputActionMap(UserControls.MoveEditorMapActions set)
		{
			return set.Get();
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005AA0 File Offset: 0x00003CA0
		public void SetCallbacks(UserControls.IMoveEditorMapActions instance)
		{
			if (this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface != null)
			{
				this.Save_Move.started -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnSave_Move;
				this.Save_Move.performed -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnSave_Move;
				this.Save_Move.canceled -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnSave_Move;
				this.Delete.started -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnDelete;
				this.Delete.performed -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnDelete;
				this.Delete.canceled -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnDelete;
				this.Focus_Camera.started -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnFocus_Camera;
				this.Focus_Camera.performed -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnFocus_Camera;
				this.Focus_Camera.canceled -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnFocus_Camera;
				this.Left_Click.started -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnLeft_Click;
				this.Left_Click.performed -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnLeft_Click;
				this.Left_Click.canceled -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnLeft_Click;
				this.Right_Click.started -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnRight_Click;
				this.Right_Click.performed -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnRight_Click;
				this.Right_Click.canceled -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnRight_Click;
				this.Drag_Select.started -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnDrag_Select;
				this.Drag_Select.performed -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnDrag_Select;
				this.Drag_Select.canceled -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnDrag_Select;
				this.Copy.started -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnCopy;
				this.Copy.performed -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnCopy;
				this.Copy.canceled -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnCopy;
				this.Paste.started -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnPaste;
				this.Paste.performed -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnPaste;
				this.Paste.canceled -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnPaste;
				this.Back.started -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnBack;
				this.Back.performed -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnBack;
				this.Back.canceled -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnBack;
				this.EditMode_Rotate.started -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnEditMode_Rotate;
				this.EditMode_Rotate.performed -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnEditMode_Rotate;
				this.EditMode_Rotate.canceled -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnEditMode_Rotate;
				this.EditMode_Move.started -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnEditMode_Move;
				this.EditMode_Move.performed -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnEditMode_Move;
				this.EditMode_Move.canceled -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnEditMode_Move;
				this.Undo.started -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnUndo;
				this.Undo.performed -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnUndo;
				this.Undo.canceled -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnUndo;
				this.Redo.started -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnRedo;
				this.Redo.performed -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnRedo;
				this.Redo.canceled -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnRedo;
				this.Save.started -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnSave;
				this.Save.performed -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnSave;
				this.Save.canceled -= this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface.OnSave;
			}
			this.m_Wrapper.m_MoveEditorMapActionsCallbackInterface = instance;
			if (instance != null)
			{
				this.Save_Move.started += instance.OnSave_Move;
				this.Save_Move.performed += instance.OnSave_Move;
				this.Save_Move.canceled += instance.OnSave_Move;
				this.Delete.started += instance.OnDelete;
				this.Delete.performed += instance.OnDelete;
				this.Delete.canceled += instance.OnDelete;
				this.Focus_Camera.started += instance.OnFocus_Camera;
				this.Focus_Camera.performed += instance.OnFocus_Camera;
				this.Focus_Camera.canceled += instance.OnFocus_Camera;
				this.Left_Click.started += instance.OnLeft_Click;
				this.Left_Click.performed += instance.OnLeft_Click;
				this.Left_Click.canceled += instance.OnLeft_Click;
				this.Right_Click.started += instance.OnRight_Click;
				this.Right_Click.performed += instance.OnRight_Click;
				this.Right_Click.canceled += instance.OnRight_Click;
				this.Drag_Select.started += instance.OnDrag_Select;
				this.Drag_Select.performed += instance.OnDrag_Select;
				this.Drag_Select.canceled += instance.OnDrag_Select;
				this.Copy.started += instance.OnCopy;
				this.Copy.performed += instance.OnCopy;
				this.Copy.canceled += instance.OnCopy;
				this.Paste.started += instance.OnPaste;
				this.Paste.performed += instance.OnPaste;
				this.Paste.canceled += instance.OnPaste;
				this.Back.started += instance.OnBack;
				this.Back.performed += instance.OnBack;
				this.Back.canceled += instance.OnBack;
				this.EditMode_Rotate.started += instance.OnEditMode_Rotate;
				this.EditMode_Rotate.performed += instance.OnEditMode_Rotate;
				this.EditMode_Rotate.canceled += instance.OnEditMode_Rotate;
				this.EditMode_Move.started += instance.OnEditMode_Move;
				this.EditMode_Move.performed += instance.OnEditMode_Move;
				this.EditMode_Move.canceled += instance.OnEditMode_Move;
				this.Undo.started += instance.OnUndo;
				this.Undo.performed += instance.OnUndo;
				this.Undo.canceled += instance.OnUndo;
				this.Redo.started += instance.OnRedo;
				this.Redo.performed += instance.OnRedo;
				this.Redo.canceled += instance.OnRedo;
				this.Save.started += instance.OnSave;
				this.Save.performed += instance.OnSave;
				this.Save.canceled += instance.OnSave;
			}
		}

		// Token: 0x04000083 RID: 131
		private UserControls m_Wrapper;
	}

	// Token: 0x02000012 RID: 18
	public struct ReplayMapActions
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x00006453 File Offset: 0x00004653
		public ReplayMapActions(UserControls wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x0000645C File Offset: 0x0000465C
		public InputAction Move_Up
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_Move_Up;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00006469 File Offset: 0x00004669
		public InputAction Move_Down
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_Move_Down;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00006476 File Offset: 0x00004676
		public InputAction ToggleToolbarVisibility
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_ToggleToolbarVisibility;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00006483 File Offset: 0x00004683
		public InputAction TogglePlay
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_TogglePlay;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00006490 File Offset: 0x00004690
		public InputAction SetReplaySpeed1
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_SetReplaySpeed1;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060000DA RID: 218 RVA: 0x0000649D File Offset: 0x0000469D
		public InputAction SetReplaySpeed2
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_SetReplaySpeed2;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060000DB RID: 219 RVA: 0x000064AA File Offset: 0x000046AA
		public InputAction SetReplaySpeed3
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_SetReplaySpeed3;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000064B7 File Offset: 0x000046B7
		public InputAction SetReplaySpeed4
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_SetReplaySpeed4;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000064C4 File Offset: 0x000046C4
		public InputAction SetReplaySpeed5
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_SetReplaySpeed5;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000064D1 File Offset: 0x000046D1
		public InputAction SetReplaySpeed6
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_SetReplaySpeed6;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000064DE File Offset: 0x000046DE
		public InputAction SetCameraMode1
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_SetCameraMode1;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000064EB File Offset: 0x000046EB
		public InputAction SetCameraMode2
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_SetCameraMode2;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000064F8 File Offset: 0x000046F8
		public InputAction SetCameraMode3
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_SetCameraMode3;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00006505 File Offset: 0x00004705
		public InputAction PreviousPlayer
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_PreviousPlayer;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00006512 File Offset: 0x00004712
		public InputAction NextPlayer
		{
			get
			{
				return this.m_Wrapper.m_ReplayMap_NextPlayer;
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000651F File Offset: 0x0000471F
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_ReplayMap;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000652C File Offset: 0x0000472C
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006539 File Offset: 0x00004739
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00006546 File Offset: 0x00004746
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006553 File Offset: 0x00004753
		public static implicit operator InputActionMap(UserControls.ReplayMapActions set)
		{
			return set.Get();
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000655C File Offset: 0x0000475C
		public void SetCallbacks(UserControls.IReplayMapActions instance)
		{
			if (this.m_Wrapper.m_ReplayMapActionsCallbackInterface != null)
			{
				this.Move_Up.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnMove_Up;
				this.Move_Up.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnMove_Up;
				this.Move_Up.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnMove_Up;
				this.Move_Down.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnMove_Down;
				this.Move_Down.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnMove_Down;
				this.Move_Down.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnMove_Down;
				this.ToggleToolbarVisibility.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnToggleToolbarVisibility;
				this.ToggleToolbarVisibility.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnToggleToolbarVisibility;
				this.ToggleToolbarVisibility.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnToggleToolbarVisibility;
				this.TogglePlay.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnTogglePlay;
				this.TogglePlay.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnTogglePlay;
				this.TogglePlay.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnTogglePlay;
				this.SetReplaySpeed1.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed1;
				this.SetReplaySpeed1.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed1;
				this.SetReplaySpeed1.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed1;
				this.SetReplaySpeed2.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed2;
				this.SetReplaySpeed2.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed2;
				this.SetReplaySpeed2.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed2;
				this.SetReplaySpeed3.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed3;
				this.SetReplaySpeed3.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed3;
				this.SetReplaySpeed3.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed3;
				this.SetReplaySpeed4.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed4;
				this.SetReplaySpeed4.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed4;
				this.SetReplaySpeed4.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed4;
				this.SetReplaySpeed5.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed5;
				this.SetReplaySpeed5.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed5;
				this.SetReplaySpeed5.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed5;
				this.SetReplaySpeed6.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed6;
				this.SetReplaySpeed6.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed6;
				this.SetReplaySpeed6.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetReplaySpeed6;
				this.SetCameraMode1.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetCameraMode1;
				this.SetCameraMode1.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetCameraMode1;
				this.SetCameraMode1.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetCameraMode1;
				this.SetCameraMode2.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetCameraMode2;
				this.SetCameraMode2.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetCameraMode2;
				this.SetCameraMode2.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetCameraMode2;
				this.SetCameraMode3.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetCameraMode3;
				this.SetCameraMode3.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetCameraMode3;
				this.SetCameraMode3.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnSetCameraMode3;
				this.PreviousPlayer.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnPreviousPlayer;
				this.PreviousPlayer.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnPreviousPlayer;
				this.PreviousPlayer.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnPreviousPlayer;
				this.NextPlayer.started -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnNextPlayer;
				this.NextPlayer.performed -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnNextPlayer;
				this.NextPlayer.canceled -= this.m_Wrapper.m_ReplayMapActionsCallbackInterface.OnNextPlayer;
			}
			this.m_Wrapper.m_ReplayMapActionsCallbackInterface = instance;
			if (instance != null)
			{
				this.Move_Up.started += instance.OnMove_Up;
				this.Move_Up.performed += instance.OnMove_Up;
				this.Move_Up.canceled += instance.OnMove_Up;
				this.Move_Down.started += instance.OnMove_Down;
				this.Move_Down.performed += instance.OnMove_Down;
				this.Move_Down.canceled += instance.OnMove_Down;
				this.ToggleToolbarVisibility.started += instance.OnToggleToolbarVisibility;
				this.ToggleToolbarVisibility.performed += instance.OnToggleToolbarVisibility;
				this.ToggleToolbarVisibility.canceled += instance.OnToggleToolbarVisibility;
				this.TogglePlay.started += instance.OnTogglePlay;
				this.TogglePlay.performed += instance.OnTogglePlay;
				this.TogglePlay.canceled += instance.OnTogglePlay;
				this.SetReplaySpeed1.started += instance.OnSetReplaySpeed1;
				this.SetReplaySpeed1.performed += instance.OnSetReplaySpeed1;
				this.SetReplaySpeed1.canceled += instance.OnSetReplaySpeed1;
				this.SetReplaySpeed2.started += instance.OnSetReplaySpeed2;
				this.SetReplaySpeed2.performed += instance.OnSetReplaySpeed2;
				this.SetReplaySpeed2.canceled += instance.OnSetReplaySpeed2;
				this.SetReplaySpeed3.started += instance.OnSetReplaySpeed3;
				this.SetReplaySpeed3.performed += instance.OnSetReplaySpeed3;
				this.SetReplaySpeed3.canceled += instance.OnSetReplaySpeed3;
				this.SetReplaySpeed4.started += instance.OnSetReplaySpeed4;
				this.SetReplaySpeed4.performed += instance.OnSetReplaySpeed4;
				this.SetReplaySpeed4.canceled += instance.OnSetReplaySpeed4;
				this.SetReplaySpeed5.started += instance.OnSetReplaySpeed5;
				this.SetReplaySpeed5.performed += instance.OnSetReplaySpeed5;
				this.SetReplaySpeed5.canceled += instance.OnSetReplaySpeed5;
				this.SetReplaySpeed6.started += instance.OnSetReplaySpeed6;
				this.SetReplaySpeed6.performed += instance.OnSetReplaySpeed6;
				this.SetReplaySpeed6.canceled += instance.OnSetReplaySpeed6;
				this.SetCameraMode1.started += instance.OnSetCameraMode1;
				this.SetCameraMode1.performed += instance.OnSetCameraMode1;
				this.SetCameraMode1.canceled += instance.OnSetCameraMode1;
				this.SetCameraMode2.started += instance.OnSetCameraMode2;
				this.SetCameraMode2.performed += instance.OnSetCameraMode2;
				this.SetCameraMode2.canceled += instance.OnSetCameraMode2;
				this.SetCameraMode3.started += instance.OnSetCameraMode3;
				this.SetCameraMode3.performed += instance.OnSetCameraMode3;
				this.SetCameraMode3.canceled += instance.OnSetCameraMode3;
				this.PreviousPlayer.started += instance.OnPreviousPlayer;
				this.PreviousPlayer.performed += instance.OnPreviousPlayer;
				this.PreviousPlayer.canceled += instance.OnPreviousPlayer;
				this.NextPlayer.started += instance.OnNextPlayer;
				this.NextPlayer.performed += instance.OnNextPlayer;
				this.NextPlayer.canceled += instance.OnNextPlayer;
			}
		}

		// Token: 0x04000084 RID: 132
		private UserControls m_Wrapper;
	}

	// Token: 0x02000013 RID: 19
	public struct GenericActions
	{
		// Token: 0x060000EA RID: 234 RVA: 0x00006FBD File Offset: 0x000051BD
		public GenericActions(UserControls wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00006FC6 File Offset: 0x000051C6
		public InputAction OpenMenu
		{
			get
			{
				return this.m_Wrapper.m_Generic_OpenMenu;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00006FD3 File Offset: 0x000051D3
		public InputAction Back
		{
			get
			{
				return this.m_Wrapper.m_Generic_Back;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00006FE0 File Offset: 0x000051E0
		public InputAction ControllerAlternativeClick
		{
			get
			{
				return this.m_Wrapper.m_Generic_ControllerAlternativeClick;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00006FED File Offset: 0x000051ED
		public InputAction Left_Click_Modifier_Or_Middle
		{
			get
			{
				return this.m_Wrapper.m_Generic_Left_Click_Modifier_Or_Middle;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00006FFA File Offset: 0x000051FA
		public InputAction Modifier
		{
			get
			{
				return this.m_Wrapper.m_Generic_Modifier;
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007007 File Offset: 0x00005207
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_Generic;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00007014 File Offset: 0x00005214
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007021 File Offset: 0x00005221
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x0000702E File Offset: 0x0000522E
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000703B File Offset: 0x0000523B
		public static implicit operator InputActionMap(UserControls.GenericActions set)
		{
			return set.Get();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00007044 File Offset: 0x00005244
		public void SetCallbacks(UserControls.IGenericActions instance)
		{
			if (this.m_Wrapper.m_GenericActionsCallbackInterface != null)
			{
				this.OpenMenu.started -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnOpenMenu;
				this.OpenMenu.performed -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnOpenMenu;
				this.OpenMenu.canceled -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnOpenMenu;
				this.Back.started -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnBack;
				this.Back.performed -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnBack;
				this.Back.canceled -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnBack;
				this.ControllerAlternativeClick.started -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnControllerAlternativeClick;
				this.ControllerAlternativeClick.performed -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnControllerAlternativeClick;
				this.ControllerAlternativeClick.canceled -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnControllerAlternativeClick;
				this.Left_Click_Modifier_Or_Middle.started -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnLeft_Click_Modifier_Or_Middle;
				this.Left_Click_Modifier_Or_Middle.performed -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnLeft_Click_Modifier_Or_Middle;
				this.Left_Click_Modifier_Or_Middle.canceled -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnLeft_Click_Modifier_Or_Middle;
				this.Modifier.started -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnModifier;
				this.Modifier.performed -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnModifier;
				this.Modifier.canceled -= this.m_Wrapper.m_GenericActionsCallbackInterface.OnModifier;
			}
			this.m_Wrapper.m_GenericActionsCallbackInterface = instance;
			if (instance != null)
			{
				this.OpenMenu.started += instance.OnOpenMenu;
				this.OpenMenu.performed += instance.OnOpenMenu;
				this.OpenMenu.canceled += instance.OnOpenMenu;
				this.Back.started += instance.OnBack;
				this.Back.performed += instance.OnBack;
				this.Back.canceled += instance.OnBack;
				this.ControllerAlternativeClick.started += instance.OnControllerAlternativeClick;
				this.ControllerAlternativeClick.performed += instance.OnControllerAlternativeClick;
				this.ControllerAlternativeClick.canceled += instance.OnControllerAlternativeClick;
				this.Left_Click_Modifier_Or_Middle.started += instance.OnLeft_Click_Modifier_Or_Middle;
				this.Left_Click_Modifier_Or_Middle.performed += instance.OnLeft_Click_Modifier_Or_Middle;
				this.Left_Click_Modifier_Or_Middle.canceled += instance.OnLeft_Click_Modifier_Or_Middle;
				this.Modifier.started += instance.OnModifier;
				this.Modifier.performed += instance.OnModifier;
				this.Modifier.canceled += instance.OnModifier;
			}
		}

		// Token: 0x04000085 RID: 133
		private UserControls m_Wrapper;
	}

	// Token: 0x02000014 RID: 20
	public struct GeneralActions
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x000073D9 File Offset: 0x000055D9
		public GeneralActions(UserControls wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000073E2 File Offset: 0x000055E2
		public InputAction Chat
		{
			get
			{
				return this.m_Wrapper.m_General_Chat;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000073EF File Offset: 0x000055EF
		public InputAction Restart
		{
			get
			{
				return this.m_Wrapper.m_General_Restart;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000073FC File Offset: 0x000055FC
		public InputAction SaveReplay
		{
			get
			{
				return this.m_Wrapper.m_General_SaveReplay;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00007409 File Offset: 0x00005609
		public InputAction PushToTalk
		{
			get
			{
				return this.m_Wrapper.m_General_PushToTalk;
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00007416 File Offset: 0x00005616
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_General;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00007423 File Offset: 0x00005623
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00007430 File Offset: 0x00005630
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000743D File Offset: 0x0000563D
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000744A File Offset: 0x0000564A
		public static implicit operator InputActionMap(UserControls.GeneralActions set)
		{
			return set.Get();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00007454 File Offset: 0x00005654
		public void SetCallbacks(UserControls.IGeneralActions instance)
		{
			if (this.m_Wrapper.m_GeneralActionsCallbackInterface != null)
			{
				this.Chat.started -= this.m_Wrapper.m_GeneralActionsCallbackInterface.OnChat;
				this.Chat.performed -= this.m_Wrapper.m_GeneralActionsCallbackInterface.OnChat;
				this.Chat.canceled -= this.m_Wrapper.m_GeneralActionsCallbackInterface.OnChat;
				this.Restart.started -= this.m_Wrapper.m_GeneralActionsCallbackInterface.OnRestart;
				this.Restart.performed -= this.m_Wrapper.m_GeneralActionsCallbackInterface.OnRestart;
				this.Restart.canceled -= this.m_Wrapper.m_GeneralActionsCallbackInterface.OnRestart;
				this.SaveReplay.started -= this.m_Wrapper.m_GeneralActionsCallbackInterface.OnSaveReplay;
				this.SaveReplay.performed -= this.m_Wrapper.m_GeneralActionsCallbackInterface.OnSaveReplay;
				this.SaveReplay.canceled -= this.m_Wrapper.m_GeneralActionsCallbackInterface.OnSaveReplay;
				this.PushToTalk.started -= this.m_Wrapper.m_GeneralActionsCallbackInterface.OnPushToTalk;
				this.PushToTalk.performed -= this.m_Wrapper.m_GeneralActionsCallbackInterface.OnPushToTalk;
				this.PushToTalk.canceled -= this.m_Wrapper.m_GeneralActionsCallbackInterface.OnPushToTalk;
			}
			this.m_Wrapper.m_GeneralActionsCallbackInterface = instance;
			if (instance != null)
			{
				this.Chat.started += instance.OnChat;
				this.Chat.performed += instance.OnChat;
				this.Chat.canceled += instance.OnChat;
				this.Restart.started += instance.OnRestart;
				this.Restart.performed += instance.OnRestart;
				this.Restart.canceled += instance.OnRestart;
				this.SaveReplay.started += instance.OnSaveReplay;
				this.SaveReplay.performed += instance.OnSaveReplay;
				this.SaveReplay.canceled += instance.OnSaveReplay;
				this.PushToTalk.started += instance.OnPushToTalk;
				this.PushToTalk.performed += instance.OnPushToTalk;
				this.PushToTalk.canceled += instance.OnPushToTalk;
			}
		}

		// Token: 0x04000086 RID: 134
		private UserControls m_Wrapper;
	}

	// Token: 0x02000015 RID: 21
	public interface IPlayerActionMapActions
	{
		// Token: 0x06000101 RID: 257
		void OnMove_Forward(InputAction.CallbackContext context);

		// Token: 0x06000102 RID: 258
		void OnMove_Back(InputAction.CallbackContext context);

		// Token: 0x06000103 RID: 259
		void OnMove_Left(InputAction.CallbackContext context);

		// Token: 0x06000104 RID: 260
		void OnMove_Right(InputAction.CallbackContext context);

		// Token: 0x06000105 RID: 261
		void OnTurn_Left(InputAction.CallbackContext context);

		// Token: 0x06000106 RID: 262
		void OnTurn_Right(InputAction.CallbackContext context);

		// Token: 0x06000107 RID: 263
		void OnTurn_Up(InputAction.CallbackContext context);

		// Token: 0x06000108 RID: 264
		void OnTurn_Down(InputAction.CallbackContext context);

		// Token: 0x06000109 RID: 265
		void OnTurn_Mouse_Vertical(InputAction.CallbackContext context);

		// Token: 0x0600010A RID: 266
		void OnTurn_Mouse_Horizontal(InputAction.CallbackContext context);

		// Token: 0x0600010B RID: 267
		void OnAction1(InputAction.CallbackContext context);

		// Token: 0x0600010C RID: 268
		void OnAction2(InputAction.CallbackContext context);

		// Token: 0x0600010D RID: 269
		void OnAction3(InputAction.CallbackContext context);

		// Token: 0x0600010E RID: 270
		void OnAction4(InputAction.CallbackContext context);

		// Token: 0x0600010F RID: 271
		void OnAction5(InputAction.CallbackContext context);

		// Token: 0x06000110 RID: 272
		void OnAction6(InputAction.CallbackContext context);

		// Token: 0x06000111 RID: 273
		void OnAction7(InputAction.CallbackContext context);

		// Token: 0x06000112 RID: 274
		void OnAction8(InputAction.CallbackContext context);

		// Token: 0x06000113 RID: 275
		void OnAction9(InputAction.CallbackContext context);

		// Token: 0x06000114 RID: 276
		void OnAction10(InputAction.CallbackContext context);

		// Token: 0x06000115 RID: 277
		void OnAction11(InputAction.CallbackContext context);

		// Token: 0x06000116 RID: 278
		void OnAction12(InputAction.CallbackContext context);

		// Token: 0x06000117 RID: 279
		void OnAction13(InputAction.CallbackContext context);

		// Token: 0x06000118 RID: 280
		void OnAction14(InputAction.CallbackContext context);

		// Token: 0x06000119 RID: 281
		void OnAction15(InputAction.CallbackContext context);

		// Token: 0x0600011A RID: 282
		void OnDirectional_Action1(InputAction.CallbackContext context);

		// Token: 0x0600011B RID: 283
		void OnDirectional_Action2(InputAction.CallbackContext context);
	}

	// Token: 0x02000016 RID: 22
	public interface IMoveEditorMapActions
	{
		// Token: 0x0600011C RID: 284
		void OnSave_Move(InputAction.CallbackContext context);

		// Token: 0x0600011D RID: 285
		void OnDelete(InputAction.CallbackContext context);

		// Token: 0x0600011E RID: 286
		void OnFocus_Camera(InputAction.CallbackContext context);

		// Token: 0x0600011F RID: 287
		void OnLeft_Click(InputAction.CallbackContext context);

		// Token: 0x06000120 RID: 288
		void OnRight_Click(InputAction.CallbackContext context);

		// Token: 0x06000121 RID: 289
		void OnDrag_Select(InputAction.CallbackContext context);

		// Token: 0x06000122 RID: 290
		void OnCopy(InputAction.CallbackContext context);

		// Token: 0x06000123 RID: 291
		void OnPaste(InputAction.CallbackContext context);

		// Token: 0x06000124 RID: 292
		void OnBack(InputAction.CallbackContext context);

		// Token: 0x06000125 RID: 293
		void OnEditMode_Rotate(InputAction.CallbackContext context);

		// Token: 0x06000126 RID: 294
		void OnEditMode_Move(InputAction.CallbackContext context);

		// Token: 0x06000127 RID: 295
		void OnUndo(InputAction.CallbackContext context);

		// Token: 0x06000128 RID: 296
		void OnRedo(InputAction.CallbackContext context);

		// Token: 0x06000129 RID: 297
		void OnSave(InputAction.CallbackContext context);
	}

	// Token: 0x02000017 RID: 23
	public interface IReplayMapActions
	{
		// Token: 0x0600012A RID: 298
		void OnMove_Up(InputAction.CallbackContext context);

		// Token: 0x0600012B RID: 299
		void OnMove_Down(InputAction.CallbackContext context);

		// Token: 0x0600012C RID: 300
		void OnToggleToolbarVisibility(InputAction.CallbackContext context);

		// Token: 0x0600012D RID: 301
		void OnTogglePlay(InputAction.CallbackContext context);

		// Token: 0x0600012E RID: 302
		void OnSetReplaySpeed1(InputAction.CallbackContext context);

		// Token: 0x0600012F RID: 303
		void OnSetReplaySpeed2(InputAction.CallbackContext context);

		// Token: 0x06000130 RID: 304
		void OnSetReplaySpeed3(InputAction.CallbackContext context);

		// Token: 0x06000131 RID: 305
		void OnSetReplaySpeed4(InputAction.CallbackContext context);

		// Token: 0x06000132 RID: 306
		void OnSetReplaySpeed5(InputAction.CallbackContext context);

		// Token: 0x06000133 RID: 307
		void OnSetReplaySpeed6(InputAction.CallbackContext context);

		// Token: 0x06000134 RID: 308
		void OnSetCameraMode1(InputAction.CallbackContext context);

		// Token: 0x06000135 RID: 309
		void OnSetCameraMode2(InputAction.CallbackContext context);

		// Token: 0x06000136 RID: 310
		void OnSetCameraMode3(InputAction.CallbackContext context);

		// Token: 0x06000137 RID: 311
		void OnPreviousPlayer(InputAction.CallbackContext context);

		// Token: 0x06000138 RID: 312
		void OnNextPlayer(InputAction.CallbackContext context);
	}

	// Token: 0x02000018 RID: 24
	public interface IGenericActions
	{
		// Token: 0x06000139 RID: 313
		void OnOpenMenu(InputAction.CallbackContext context);

		// Token: 0x0600013A RID: 314
		void OnBack(InputAction.CallbackContext context);

		// Token: 0x0600013B RID: 315
		void OnControllerAlternativeClick(InputAction.CallbackContext context);

		// Token: 0x0600013C RID: 316
		void OnLeft_Click_Modifier_Or_Middle(InputAction.CallbackContext context);

		// Token: 0x0600013D RID: 317
		void OnModifier(InputAction.CallbackContext context);
	}

	// Token: 0x02000019 RID: 25
	public interface IGeneralActions
	{
		// Token: 0x0600013E RID: 318
		void OnChat(InputAction.CallbackContext context);

		// Token: 0x0600013F RID: 319
		void OnRestart(InputAction.CallbackContext context);

		// Token: 0x06000140 RID: 320
		void OnSaveReplay(InputAction.CallbackContext context);

		// Token: 0x06000141 RID: 321
		void OnPushToTalk(InputAction.CallbackContext context);
	}
}
