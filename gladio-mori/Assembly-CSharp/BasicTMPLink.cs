using System;
using BasicUI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001B9 RID: 441
[RequireComponent(typeof(TMP_Text))]
public class BasicTMPLink : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x06000D4F RID: 3407 RVA: 0x000438BA File Offset: 0x00041ABA
	public void Awake()
	{
		this.m_TextMeshPro = base.GetComponent<TMP_Text>();
		this.m_TextMeshPro.color = UISettings.BasicTextColor;
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x000438D8 File Offset: 0x00041AD8
	public void OnPointerClick(PointerEventData eventData)
	{
		int num = TMP_TextUtilities.FindIntersectingLink(this.m_TextMeshPro, eventData.position, null);
		if (num != -1)
		{
			TMP_LinkInfo tmp_LinkInfo = this.m_TextMeshPro.textInfo.linkInfo[num];
			Application.OpenURL(tmp_LinkInfo.GetLinkID());
		}
	}

	// Token: 0x06000D51 RID: 3409 RVA: 0x00043924 File Offset: 0x00041B24
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isHoveringObject = true;
	}

	// Token: 0x06000D52 RID: 3410 RVA: 0x0004392D File Offset: 0x00041B2D
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isHoveringObject = false;
	}

	// Token: 0x06000D53 RID: 3411 RVA: 0x00043938 File Offset: 0x00041B38
	private void LateUpdate()
	{
		if (this.isHoveringObject)
		{
			int num = TMP_TextUtilities.FindIntersectingLink(this.m_TextMeshPro, Input.mousePosition, null);
			if ((num == -1 && this.m_selectedLink != -1) || (num != this.m_selectedLink && this.m_selectedLink != -1))
			{
				TMP_LinkInfo tmp_LinkInfo = this.m_TextMeshPro.textInfo.linkInfo[this.m_selectedLink];
				for (int i = 0; i < tmp_LinkInfo.GetLinkText().Length; i++)
				{
					int num2 = tmp_LinkInfo.linkTextfirstCharacterIndex + i;
					if (!char.IsWhiteSpace(this.m_TextMeshPro.textInfo.characterInfo[num2].character))
					{
						int materialReferenceIndex = this.m_TextMeshPro.textInfo.characterInfo[num2].materialReferenceIndex;
						int vertexIndex = this.m_TextMeshPro.textInfo.characterInfo[num2].vertexIndex;
						Color32[] colors = this.m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].colors32;
						Color32 color = colors[vertexIndex].Tint(1.33333f);
						colors[vertexIndex] = color;
						colors[vertexIndex + 1] = color;
						colors[vertexIndex + 2] = color;
						colors[vertexIndex + 3] = color;
					}
				}
				this.m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
				this.m_selectedLink = -1;
			}
			if (num != -1 && num != this.m_selectedLink)
			{
				this.m_selectedLink = num;
				TMP_LinkInfo tmp_LinkInfo2 = this.m_TextMeshPro.textInfo.linkInfo[num];
				for (int j = 0; j < tmp_LinkInfo2.linkTextLength; j++)
				{
					int num3 = tmp_LinkInfo2.linkTextfirstCharacterIndex + j;
					if (!char.IsWhiteSpace(this.m_TextMeshPro.textInfo.characterInfo[num3].character))
					{
						int materialReferenceIndex2 = this.m_TextMeshPro.textInfo.characterInfo[num3].materialReferenceIndex;
						int vertexIndex2 = this.m_TextMeshPro.textInfo.characterInfo[num3].vertexIndex;
						Color32[] colors2 = this.m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex2].colors32;
						Color32 color2 = colors2[vertexIndex2].Tint(0.75f);
						colors2[vertexIndex2] = color2;
						colors2[vertexIndex2 + 1] = color2;
						colors2[vertexIndex2 + 2] = color2;
						colors2[vertexIndex2 + 3] = color2;
					}
				}
				this.m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
			}
		}
	}

	// Token: 0x0400099E RID: 2462
	private TMP_Text m_TextMeshPro;

	// Token: 0x0400099F RID: 2463
	private bool isHoveringObject;

	// Token: 0x040009A0 RID: 2464
	private int m_selectedLink = -1;
}
