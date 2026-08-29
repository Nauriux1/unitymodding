using System;
using System.Collections.Generic;
using BasicUI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Mirror;
using Mirror.RemoteCalls;
using TMPro;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

// Token: 0x02000123 RID: 291
public class MultiplayerChat : NetworkBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060008F6 RID: 2294 RVA: 0x0002BA48 File Offset: 0x00029C48
	// (remove) Token: 0x060008F7 RID: 2295 RVA: 0x0002BA7C File Offset: 0x00029C7C
	private static event Action<string, string> OnMessage;

	// Token: 0x060008F8 RID: 2296 RVA: 0x0002BAAF File Offset: 0x00029CAF
	private void Awake()
	{
		this.multiplayerRoomPlayer = base.gameObject.GetComponent<MultiplayerRoomPlayer>();
		this.InitJobStuff();
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0002BAC8 File Offset: 0x00029CC8
	private void Update()
	{
		if (this.hideTime != null)
		{
			this.UpdateVisibility();
		}
		this.CheckIfJobDone();
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0002BAE4 File Offset: 0x00029CE4
	public override void OnStartAuthority()
	{
		MultiplayerChat.singleton = this;
		MultiplayerChat.OnMessage -= this.HandleNewMessage;
		this.chatGameObject = UnityEngine.Object.Instantiate<GameObject>(this.chatPrefab);
		UnityEngine.Object.DontDestroyOnLoad(this.chatGameObject);
		this.canvasGroup = this.chatGameObject.GetComponent<CanvasGroup>();
		this.chatText = this.chatGameObject.GetComponentInChildren<TMP_Text>();
		this.chatInputField = this.chatGameObject.GetComponentInChildren<TMP_InputField>();
		this.scrollRect = this.chatGameObject.GetComponentInChildren<ScrollRect>();
		this.chatHolder = this.scrollRect.content.transform;
		this.chatPoolHolder = this.chatGameObject.transform.Find("Pool");
		this.InitializeChatMessages();
		this.chatInputField.characterLimit = this.maxMessageLength;
		this.chatInputField.onSubmit.AddListener(delegate(string <p0>)
		{
			this.Send(this.chatInputField.text);
		});
		this.chatInputField.onEndEdit.AddListener(delegate(string <p0>)
		{
			this.EndEdit();
		});
		MultiplayerChat.OnMessage += this.HandleNewMessage;
		this.SetAlwaysVisible(!SceneManager.GetActiveScene().name.Contains("map_"));
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0002BC1C File Offset: 0x00029E1C
	private void OnDestroy()
	{
		this.DisposeJobStuff();
		MultiplayerChat.OnMessage -= this.HandleNewMessage;
		if (this.chatGameObject != null)
		{
			UnityEngine.Object.Destroy(this.chatGameObject);
		}
		if (MultiplayerChat.singleton == this)
		{
			MultiplayerChat.singleton = null;
		}
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x0002BC6C File Offset: 0x00029E6C
	private void HandleNewMessage(string message, string playerName)
	{
		if (SettingsHelper.GetChatOption() != ChatOption.Disabled)
		{
			this.ShowNewMessage(message, playerName, "");
		}
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0002BC82 File Offset: 0x00029E82
	private void ShowNewMessage(string message, string playerName, string nameColor = "")
	{
		ChatMessageItem chatMessageItemFromPool = this.GetChatMessageItemFromPool();
		chatMessageItemFromPool.multiplayerChatMessage.SetMessage(message, playerName, nameColor);
		chatMessageItemFromPool.gameObject.transform.SetParent(this.chatHolder);
		this.UpdateHideTime();
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0002BCB3 File Offset: 0x00029EB3
	public void DisplaySystemMessage(string message)
	{
		this.ShowNewMessage(message, LocalizationHelpers.LocalizedText("txt_system_message", Array.Empty<object>()), UISettings._basicSystemTextColor);
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0002BCD0 File Offset: 0x00029ED0
	private void UpdateHideTime()
	{
		this.hideTime = new float?(Time.unscaledTime + this.messageVisibilityTime);
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0002BCEC File Offset: 0x00029EEC
	[Client]
	public void Send(string message)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void MultiplayerChat::Send(System.String)' called when client was not active");
			return;
		}
		if (SettingsHelper.GetChatOption() != ChatOption.Disabled)
		{
			if (!string.IsNullOrWhiteSpace(message) && message.Length <= this.maxMessageLength)
			{
				this.CmdSendMessage(message);
				this.UpdateHideTime();
			}
		}
		else
		{
			this.DisplaySystemMessage(LocalizationHelpers.LocalizedText("txt_system_message_chat_disabled", Array.Empty<object>()));
		}
		this.chatInputField.text = string.Empty;
		this.chatInputField.DeactivateInputField(true);
		EventSystem.current.SetSelectedGameObject(null);
		this.UpdateVisibility();
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x0002BD7C File Offset: 0x00029F7C
	[Command]
	private void CmdSendMessage(string message)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(message);
		base.SendCommandInternal("System.Void MultiplayerChat::CmdSendMessage(System.String)", -1528187634, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x0002BDB8 File Offset: 0x00029FB8
	[ClientRpc]
	private void RpcHandleMessage(string message, string playerName)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(message);
		writer.WriteString(playerName);
		this.SendRPCInternal("System.Void MultiplayerChat::RpcHandleMessage(System.String,System.String)", -7720107, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x0002BDFC File Offset: 0x00029FFC
	public void SetAlwaysVisible(bool visible)
	{
		this.alwaysVisibe = visible;
		this.UpdateVisibility();
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x0002BE0B File Offset: 0x0002A00B
	public void SetAlwaysHidden(bool hidden)
	{
		this.alwaysHidden = hidden;
		this.UpdateVisibility();
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x0002BE1C File Offset: 0x0002A01C
	public void UpdateVisibility()
	{
		if (!this.alwaysHidden)
		{
			if (SettingsHelper.GetChatOption() != ChatOption.AlwaysVisible && !this.alwaysVisibe && !this.currentlyWritingToChat && GameMenu.GameMenuCurrentlyHidden)
			{
				if (this.hideTime == null)
				{
					goto IL_5E;
				}
				float unscaledTime = Time.unscaledTime;
				float? num = this.hideTime;
				if (!(unscaledTime <= num.GetValueOrDefault() & num != null))
				{
					goto IL_5E;
				}
			}
			this.SetVisibility(true);
			return;
		}
		IL_5E:
		this.hideTime = null;
		this.SetVisibility(false);
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0002BE9C File Offset: 0x0002A09C
	private void SetVisibility(bool newVisibility)
	{
		if (newVisibility != this.chatIsVisible)
		{
			if (this.runningTween != null && this.runningTween.active)
			{
				this.runningTween.Kill(false);
			}
			this.chatIsVisible = newVisibility;
			if (this.chatIsVisible)
			{
				this.canvasGroup.blocksRaycasts = true;
				this.runningTween = this.canvasGroup.DOFade(1f, 0.04f * Time.timeScale);
				return;
			}
			this.canvasGroup.blocksRaycasts = false;
			this.runningTween = this.canvasGroup.DOFade(0f, 0.4f * Time.timeScale);
		}
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0002BF40 File Offset: 0x0002A140
	public void ActivateInputField()
	{
		if (!this.chatInputField.isFocused)
		{
			this.currentlyWritingToChat = true;
			this.chatInputField.ActivateInputField();
			this.UpdateInputSystemState();
			this.UpdateVisibility();
		}
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x0002BF6D File Offset: 0x0002A16D
	public void DeactivateInputField()
	{
		if (this.chatInputField != null)
		{
			this.chatInputField.DeactivateInputField(true);
		}
		if (EventSystem.current != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		this.EndEdit();
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x0002BFA7 File Offset: 0x0002A1A7
	[Client]
	public void EndEdit()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void MultiplayerChat::EndEdit()' called when client was not active");
			return;
		}
		this.currentlyWritingToChat = false;
		this.UpdateInputSystemState();
		this.UpdateVisibility();
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x0002BFD1 File Offset: 0x0002A1D1
	private void UpdateInputSystemState()
	{
		if (GeneralManager.singleton != null)
		{
			GeneralManager.singleton.UpdateInputSystemState();
		}
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0002BFEC File Offset: 0x0002A1EC
	private void InitializeChatMessages()
	{
		this.pool_chatMessages = new List<ChatMessageItem>(64);
		this.inUse_chatMessages = new List<ChatMessageItem>(64);
		for (int i = 0; i < 33; i++)
		{
			this.pool_chatMessages.Add(this.CreateNewChatMessageItem());
		}
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0002C031 File Offset: 0x0002A231
	private ChatMessageItem CreateNewChatMessageItem()
	{
		ChatMessageItem chatMessageItem = new ChatMessageItem();
		chatMessageItem.gameObject = UnityEngine.Object.Instantiate<GameObject>(this.messagePrefab, this.chatPoolHolder);
		chatMessageItem.multiplayerChatMessage = chatMessageItem.gameObject.GetComponent<MultiplayerChatMessage>();
		return chatMessageItem;
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0002C060 File Offset: 0x0002A260
	private ChatMessageItem GetChatMessageItemFromPool()
	{
		ChatMessageItem chatMessageItem = null;
		if (this.pool_chatMessages.Count == 0)
		{
			this.ReturnFirstMessageItemToPool();
		}
		if (this.pool_chatMessages.Count > 0)
		{
			int index = this.pool_chatMessages.Count - 1;
			chatMessageItem = this.pool_chatMessages[index];
			this.pool_chatMessages.RemoveAt(index);
		}
		if (chatMessageItem == null)
		{
			chatMessageItem = this.CreateNewChatMessageItem();
		}
		this.inUse_chatMessages.Add(chatMessageItem);
		return chatMessageItem;
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x0002C0CE File Offset: 0x0002A2CE
	private void ReturnChatMessageItemToPool(ChatMessageItem chatMessageItem)
	{
		chatMessageItem.gameObject.transform.parent = this.chatPoolHolder;
		this.pool_chatMessages.Add(chatMessageItem);
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0002C0F4 File Offset: 0x0002A2F4
	private void ReturnFirstMessageItemToPool()
	{
		if (this.inUse_chatMessages.Count > 0)
		{
			int index = 0;
			ChatMessageItem chatMessageItem = this.inUse_chatMessages[index];
			this.inUse_chatMessages.RemoveAt(index);
			this.ReturnChatMessageItemToPool(chatMessageItem);
		}
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0002C131 File Offset: 0x0002A331
	private void InitJobStuff()
	{
		this.regex = new FixedString4096Bytes(GeneralManager.singleton.generatedRegexText);
		this.resultMessage = new NativeArray<FixedString512Bytes>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.messagesToHandle = new List<FixedString512Bytes>(8);
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x0002C162 File Offset: 0x0002A362
	private void DisposeJobStuff()
	{
		if (this.resultMessage.IsCreated)
		{
			this.resultMessage.Dispose();
		}
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x0002C17C File Offset: 0x0002A37C
	private void HandleNewMessageOnServer(string text)
	{
		this.messagesToHandle.Add(new FixedString512Bytes(text));
		this.AttemptToHandleNextMessage();
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x0002C198 File Offset: 0x0002A398
	private void CheckIfJobDone()
	{
		if (this.doingJob && this._jobHandle.IsCompleted)
		{
			this._jobHandle.Complete();
			this.RpcHandleMessage(this.resultMessage[0].ToString(), this.multiplayerRoomPlayer.playerName);
			this.doingJob = false;
			this.AttemptToHandleNextMessage();
		}
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x0002C200 File Offset: 0x0002A400
	private void AttemptToHandleNextMessage()
	{
		if (this._jobHandle.IsCompleted && this.messagesToHandle.Count > 0)
		{
			this.doingJob = true;
			this.currentMessage = this.messagesToHandle[0];
			this.messagesToHandle.RemoveAt(0);
			StringFilterJob jobData = new StringFilterJob(this.currentMessage, this.regex, this.resultMessage);
			this._jobHandle = jobData.Schedule(default(JobHandle));
		}
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x0002C2C2 File Offset: 0x0002A4C2
	protected void UserCode_CmdSendMessage__String(string message)
	{
		if (message.Length <= this.maxMessageLength)
		{
			this.HandleNewMessageOnServer(message);
		}
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x0002C2D9 File Offset: 0x0002A4D9
	protected static void InvokeUserCode_CmdSendMessage__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSendMessage called on client.");
			return;
		}
		((MultiplayerChat)obj).UserCode_CmdSendMessage__String(reader.ReadString());
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x0002C302 File Offset: 0x0002A502
	protected void UserCode_RpcHandleMessage__String__String(string message, string playerName)
	{
		Action<string, string> onMessage = MultiplayerChat.OnMessage;
		if (onMessage == null)
		{
			return;
		}
		onMessage(message, playerName);
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x0002C315 File Offset: 0x0002A515
	protected static void InvokeUserCode_RpcHandleMessage__String__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcHandleMessage called on server.");
			return;
		}
		((MultiplayerChat)obj).UserCode_RpcHandleMessage__String__String(reader.ReadString(), reader.ReadString());
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x0002C344 File Offset: 0x0002A544
	static MultiplayerChat()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(MultiplayerChat), "System.Void MultiplayerChat::CmdSendMessage(System.String)", new RemoteCallDelegate(MultiplayerChat.InvokeUserCode_CmdSendMessage__String), true);
		RemoteProcedureCalls.RegisterRpc(typeof(MultiplayerChat), "System.Void MultiplayerChat::RpcHandleMessage(System.String,System.String)", new RemoteCallDelegate(MultiplayerChat.InvokeUserCode_RpcHandleMessage__String__String));
	}

	// Token: 0x04000644 RID: 1604
	public static MultiplayerChat singleton;

	// Token: 0x04000645 RID: 1605
	public bool currentlyWritingToChat;

	// Token: 0x04000646 RID: 1606
	public MultiplayerRoomPlayer multiplayerRoomPlayer;

	// Token: 0x04000647 RID: 1607
	public GameObject chatPrefab;

	// Token: 0x04000648 RID: 1608
	public GameObject messagePrefab;

	// Token: 0x04000649 RID: 1609
	public ScrollRect scrollRect;

	// Token: 0x0400064A RID: 1610
	public Transform chatHolder;

	// Token: 0x0400064B RID: 1611
	public Transform chatPoolHolder;

	// Token: 0x0400064C RID: 1612
	public GameObject chatGameObject;

	// Token: 0x0400064D RID: 1613
	public TMP_Text chatText;

	// Token: 0x0400064E RID: 1614
	public TMP_InputField chatInputField;

	// Token: 0x0400064F RID: 1615
	private bool alwaysHidden;

	// Token: 0x04000650 RID: 1616
	private bool alwaysVisibe = true;

	// Token: 0x04000651 RID: 1617
	public CanvasGroup canvasGroup;

	// Token: 0x04000652 RID: 1618
	private float messageVisibilityTime = 5f;

	// Token: 0x04000654 RID: 1620
	private int maxMessageLength = 200;

	// Token: 0x04000655 RID: 1621
	private bool chatIsVisible = true;

	// Token: 0x04000656 RID: 1622
	private float? hideTime;

	// Token: 0x04000657 RID: 1623
	private TweenerCore<float, float, FloatOptions> runningTween;

	// Token: 0x04000658 RID: 1624
	private List<ChatMessageItem> pool_chatMessages;

	// Token: 0x04000659 RID: 1625
	private List<ChatMessageItem> inUse_chatMessages;

	// Token: 0x0400065A RID: 1626
	private JobHandle _jobHandle;

	// Token: 0x0400065B RID: 1627
	private FixedString512Bytes currentMessage;

	// Token: 0x0400065C RID: 1628
	private FixedString4096Bytes regex;

	// Token: 0x0400065D RID: 1629
	private NativeArray<FixedString512Bytes> resultMessage;

	// Token: 0x0400065E RID: 1630
	private List<FixedString512Bytes> messagesToHandle;

	// Token: 0x0400065F RID: 1631
	private bool doingJob;
}
