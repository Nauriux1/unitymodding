using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Utils;

// Token: 0x020000C2 RID: 194
public class ReplayListManager : MonoBehaviour
{
	// Token: 0x060006C4 RID: 1732 RVA: 0x000225D8 File Offset: 0x000207D8
	private void Start()
	{
		this.recordingDestination = RecordingHelper.GetRecordingDestination();
		this.UpdateReplayList();
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x000225EB File Offset: 0x000207EB
	private void Update()
	{
		this.DrawReplayList();
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x000225F4 File Offset: 0x000207F4
	public void DrawReplayList()
	{
		if (this.drawReplayList)
		{
			this.drawReplayList = false;
			foreach (object obj in this.replayListHolder.transform)
			{
				UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
			foreach (Recording recording in this.recordingList)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.replayListItemPrefab);
				gameObject.transform.parent = this.replayListHolder.transform;
				ReplayListItem item = gameObject.GetComponent<ReplayListItem>();
				item.SetRecording(recording);
				item.playReplayButton.onClick.AddListener(delegate()
				{
					this.PlayReplay(item.recording.name);
				});
				item.deleteReplayButton.onClick.AddListener(delegate()
				{
					this.DeleteReplayButtonClicked(item);
				});
				item.renameReplayButton.onClick.AddListener(delegate()
				{
					this.RenameReplayButtonClicked(item);
				});
			}
		}
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x00022760 File Offset: 0x00020960
	public void UpdateReplayList()
	{
		ReplayListManager.<UpdateReplayList>d__10 <UpdateReplayList>d__;
		<UpdateReplayList>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<UpdateReplayList>d__.<>4__this = this;
		<UpdateReplayList>d__.<>1__state = -1;
		<UpdateReplayList>d__.<>t__builder.Start<ReplayListManager.<UpdateReplayList>d__10>(ref <UpdateReplayList>d__);
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x00022797 File Offset: 0x00020997
	public void GetReplayList()
	{
		this.recordingList = RecordingHelper.LoadRecordingsList(this.recordingDestination);
		this.drawReplayList = true;
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x000227B4 File Offset: 0x000209B4
	public void PlayReplay(string replayName)
	{
		if (ReplayManager.singleton != null)
		{
			Recording newRecording = RecordingHelper.LoadRecording(this.recordingDestination, replayName);
			ReplayManager.singleton.LoadRecording(newRecording);
		}
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x000227E8 File Offset: 0x000209E8
	private void DeleteReplayButtonClicked(ReplayListItem item)
	{
		BasicConfirmDialog component = UnityEngine.Object.Instantiate<GameObject>(this.confirmDialogPrefab).GetComponent<BasicConfirmDialog>();
		component.SetText(LocalizationHelpers.LocalizedText("confirm_txt_delete_moveset", new object[]
		{
			item.recording.name
		}), null, false);
		component.okButton.onClick.AddListener(delegate()
		{
			this.DeleteReplay(item);
		});
		component.cancelButton.Select();
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x0002286C File Offset: 0x00020A6C
	public void DeleteReplay(ReplayListItem item)
	{
		if (RecordingHelper.DeleteRecording(this.recordingDestination, item.recording.name))
		{
			this.recordingList.Remove(item.recording);
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x000228A4 File Offset: 0x00020AA4
	private void RenameReplayButtonClicked(ReplayListItem item)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.textConfirmDialogPrefab);
		BasicTextConfirmDialog dialog = gameObject.GetComponent<BasicTextConfirmDialog>();
		dialog.SetText(LocalizationHelpers.LocalizedText("btn_rename", new object[0]), null, false);
		dialog.okButton.onClick.RemoveAllListeners();
		dialog.textInputField.text = item.recording.name;
		dialog.okButton.onClick.AddListener(delegate()
		{
			this.RenameReplay(item, dialog);
		});
		dialog.textInputField.Select();
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x00022960 File Offset: 0x00020B60
	public void RenameReplay(ReplayListItem item, BasicTextConfirmDialog dialog)
	{
		string text = dialog.textInputField.text;
		if (RecordingHelper.RenameRecording(this.recordingDestination, item.recording.name, text))
		{
			UnityEngine.Object.Destroy(dialog.canvas.gameObject);
			item.recording.name = text;
			item.replayNameText.text = text;
		}
	}

	// Token: 0x04000496 RID: 1174
	public GameObject replayListItemPrefab;

	// Token: 0x04000497 RID: 1175
	public GameObject replayListHolder;

	// Token: 0x04000498 RID: 1176
	public bool drawReplayList;

	// Token: 0x04000499 RID: 1177
	private List<Recording> recordingList = new List<Recording>();

	// Token: 0x0400049A RID: 1178
	public string recordingDestination = "";

	// Token: 0x0400049B RID: 1179
	public GameObject confirmDialogPrefab;

	// Token: 0x0400049C RID: 1180
	public GameObject textConfirmDialogPrefab;
}
