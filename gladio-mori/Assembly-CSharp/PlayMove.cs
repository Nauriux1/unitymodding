using System;
using System.Linq;
using MoveClasses;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000208 RID: 520
public class PlayMove : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
{
	// Token: 0x06000FF8 RID: 4088 RVA: 0x000536F3 File Offset: 0x000518F3
	private void Start()
	{
		this.animator = (PlayerAnimator)UnityEngine.Object.FindObjectsOfType(typeof(PlayerAnimator)).FirstOrDefault<UnityEngine.Object>();
	}

	// Token: 0x06000FF9 RID: 4089 RVA: 0x00053714 File Offset: 0x00051914
	public void Play()
	{
		this.animator.PlayMove(this.move, false, false, 0f, false);
	}

	// Token: 0x06000FFA RID: 4090 RVA: 0x0005372F File Offset: 0x0005192F
	public void CancelMove()
	{
		if (this.move.inputType == inputType.Continuous || this.move.inputType == inputType.HoldDown)
		{
			this.animator.CancelMove(this.move.guid);
		}
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x00053763 File Offset: 0x00051963
	public void OnPointerDown(PointerEventData eventData)
	{
		this.Play();
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x0005376B File Offset: 0x0005196B
	public void OnPointerUp(PointerEventData eventData)
	{
		this.CancelMove();
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x0000777A File Offset: 0x0000597A
	public void OnDrag(PointerEventData eventData)
	{
	}

	// Token: 0x04000B72 RID: 2930
	public PlayerAnimator animator;

	// Token: 0x04000B73 RID: 2931
	public Move move;
}
