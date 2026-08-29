using System;
using PaintIn3D;
using UnityEngine;
using Utils;

// Token: 0x02000131 RID: 305
public class PaintEditorItem
{
	// Token: 0x06000978 RID: 2424 RVA: 0x0002D264 File Offset: 0x0002B464
	public string GetDescription()
	{
		if (string.IsNullOrEmpty(this.description))
		{
			string text = "";
			if (this.Side != PaintableItemSideType.Center)
			{
				text = this.Side.GetDescription();
			}
			if (this.IsBall)
			{
				this.description = LocalizationHelpers.LocalizedText("txt_paintable_item_name_ball", new object[]
				{
					this.ParseBodyPartName(),
					text
				});
			}
			else
			{
				this.description = LocalizationHelpers.LocalizedText("txt_paintable_item_name", new object[]
				{
					this.ParseBodyPartName(),
					text
				});
			}
		}
		return this.description;
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x0002D2F4 File Offset: 0x0002B4F4
	private string ParseBodyPartName()
	{
		string text = this.gameObject.name;
		text = text.ToLower().Replace("mesh", "").Replace("mesh", "").Replace("left", "").Replace("right", "").Replace("quadsphere", "").ToString();
		return LocalizationHelpers.LocalizedText("txt_bodypart_name_" + text, Array.Empty<object>());
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x0600097A RID: 2426 RVA: 0x0002D37C File Offset: 0x0002B57C
	public bool IsBall
	{
		get
		{
			if (this._isBall == null)
			{
				if (this.gameObject.name.ToLower().Contains("sphere"))
				{
					this._isBall = new bool?(true);
				}
				else
				{
					this._isBall = new bool?(false);
				}
			}
			return this._isBall.Value;
		}
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x0600097B RID: 2427 RVA: 0x0002D3D8 File Offset: 0x0002B5D8
	public PaintableItemSideType Side
	{
		get
		{
			if (this._side == null)
			{
				if (this.gameObject.name.ToLower().Contains("left"))
				{
					this._side = new PaintableItemSideType?(PaintableItemSideType.Left);
				}
				else if (this.gameObject.name.ToLower().Contains("right"))
				{
					this._side = new PaintableItemSideType?(PaintableItemSideType.Right);
				}
				else
				{
					this._side = new PaintableItemSideType?(PaintableItemSideType.Center);
				}
			}
			return this._side.Value;
		}
	}

	// Token: 0x040006A4 RID: 1700
	public bool mainItem;

	// Token: 0x040006A5 RID: 1701
	public GameObject gameObject;

	// Token: 0x040006A6 RID: 1702
	public Renderer renderer;

	// Token: 0x040006A7 RID: 1703
	public MeshFilter meshFilter;

	// Token: 0x040006A8 RID: 1704
	public CwPaintableMeshTexture cwMainPaintableMeshTexture;

	// Token: 0x040006A9 RID: 1705
	public CwPaintableMesh cwMainPaintableMesh;

	// Token: 0x040006AA RID: 1706
	public CwPaintableMesh cwLocalPaintableMesh;

	// Token: 0x040006AB RID: 1707
	public MeshCollider meshCollider;

	// Token: 0x040006AC RID: 1708
	public string description;

	// Token: 0x040006AD RID: 1709
	private bool? _isBall;

	// Token: 0x040006AE RID: 1710
	private PaintableItemSideType? _side;
}
