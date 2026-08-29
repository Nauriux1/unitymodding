using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C1 RID: 193
public class ReplayListItem : MonoBehaviour
{
	// Token: 0x060006C0 RID: 1728 RVA: 0x00022591 File Offset: 0x00020791
	public void SetRecording(Recording newRecording)
	{
		this.recording = newRecording;
		this.SetNameText(this.recording.name);
		this.SetSizeText(this.recording.fileSizeString);
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x000225BC File Offset: 0x000207BC
	public void SetNameText(string newName)
	{
		this.replayNameText.text = newName;
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x000225CA File Offset: 0x000207CA
	public void SetSizeText(string newText)
	{
		this.replaySizeText.text = newText;
	}

	// Token: 0x04000490 RID: 1168
	public Text replayNameText;

	// Token: 0x04000491 RID: 1169
	public Text replaySizeText;

	// Token: 0x04000492 RID: 1170
	public Button playReplayButton;

	// Token: 0x04000493 RID: 1171
	public Button deleteReplayButton;

	// Token: 0x04000494 RID: 1172
	public Button renameReplayButton;

	// Token: 0x04000495 RID: 1173
	public Recording recording;
}
