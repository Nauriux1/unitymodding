using System;
using UnityEngine;

namespace BasicUI
{
	// Token: 0x0200026C RID: 620
	public static class UISettings
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060011EA RID: 4586 RVA: 0x0005BB11 File Offset: 0x00059D11
		public static Color BasicBackgroundColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicBackgroundColor, 1f);
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060011EB RID: 4587 RVA: 0x0005BB22 File Offset: 0x00059D22
		public static Color BasicScrollviewColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicBackgroundColor, 0.85f);
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x0005BB33 File Offset: 0x00059D33
		public static Color BasicPanelColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicPanelColor, 1f);
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060011ED RID: 4589 RVA: 0x0005BB44 File Offset: 0x00059D44
		public static Color BasicIconColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicIconColor, 1f);
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x0005BB55 File Offset: 0x00059D55
		public static Color BasicBackgroundMainMenuColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicBackgroundMainMenuColor, 0.95f);
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060011EF RID: 4591 RVA: 0x0005BB66 File Offset: 0x00059D66
		public static Color BasicBackgroundMenuColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicBackgroundMenuColor, 1f);
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060011F0 RID: 4592 RVA: 0x0005BB77 File Offset: 0x00059D77
		public static Color BasicSubPanelColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicSubPanelColor, 1f);
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x0005BB88 File Offset: 0x00059D88
		public static Color BasicOverlayPanelColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicOverlayPanelColor, 0.8f);
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x0005BB99 File Offset: 0x00059D99
		public static Color BasicButtonColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicButtonColor, 1f);
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060011F3 RID: 4595 RVA: 0x0005BBAA File Offset: 0x00059DAA
		public static Color BasicDisabledColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicDisabledColor, 1f);
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060011F4 RID: 4596 RVA: 0x0005BBBB File Offset: 0x00059DBB
		public static Color BasicDisabledTextColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicDisabledTextColor, 1f);
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060011F5 RID: 4597 RVA: 0x0005BBCC File Offset: 0x00059DCC
		public static Color BasicButtonSelectedColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicButtonSelectedColor, 1f);
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060011F6 RID: 4598 RVA: 0x0005BBDD File Offset: 0x00059DDD
		public static Color BasicButtonReadyColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicButtonReadyColor, 1f);
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060011F7 RID: 4599 RVA: 0x0005BBEE File Offset: 0x00059DEE
		public static Color BasicButtonNotReadyColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicButtonNotReadyColor, 1f);
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x0005BBFF File Offset: 0x00059DFF
		public static Color BasicTextReadyColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicTextReadyColor, 1f);
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060011F9 RID: 4601 RVA: 0x0005BC10 File Offset: 0x00059E10
		public static Color BasicTextNotReadyColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicTextNotReadyColor, 1f);
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x0005BC21 File Offset: 0x00059E21
		public static Color BasicTextColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicTextColor, 1f);
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060011FB RID: 4603 RVA: 0x0005BC32 File Offset: 0x00059E32
		public static Color BasicScrollbarColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicScrollbarColor, 1f);
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x0005BC43 File Offset: 0x00059E43
		public static Color BasicScrollbarBackgroundColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicScrollbarBackgroundColor, 1f);
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060011FD RID: 4605 RVA: 0x0005BC54 File Offset: 0x00059E54
		public static Color BasicTableTitleRowColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicTableTitleRowColor, 1f);
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060011FE RID: 4606 RVA: 0x0005BC65 File Offset: 0x00059E65
		public static Color SelectionBoxColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._selectionBoxColor, 0.05f);
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060011FF RID: 4607 RVA: 0x0005BC76 File Offset: 0x00059E76
		public static Color SelectionBoxBorderColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._selectionBoxBorderColor, 0.9f);
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06001200 RID: 4608 RVA: 0x0005BC87 File Offset: 0x00059E87
		public static Color BrightTest
		{
			get
			{
				return UISettings.ParseColor(UISettings._brightTest, 1f);
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06001201 RID: 4609 RVA: 0x0005BC98 File Offset: 0x00059E98
		public static Color HandHoldColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._handHoldColor, 1f);
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06001202 RID: 4610 RVA: 0x0005BCA9 File Offset: 0x00059EA9
		public static Color HandLooseHoldColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._handLooseHoldColor, 1f);
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06001203 RID: 4611 RVA: 0x0005BCBA File Offset: 0x00059EBA
		public static Color HandNoHoldColor
		{
			get
			{
				return UISettings.ParseColor(UISettings._handNoHoldColor, 1f);
			}
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x0005BCCC File Offset: 0x00059ECC
		public static Color ParseColor(string colorHex, float alpha = 1f)
		{
			Color result = default(Color);
			ColorUtility.TryParseHtmlString(colorHex, out result);
			result.a = alpha;
			return result;
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x0005BCF3 File Offset: 0x00059EF3
		public static Color BasicButtonColorNewStyle
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicButtonColorNewStyle, 1f);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x0005BD04 File Offset: 0x00059F04
		public static Color BasicButtonSelectedColorNewStyle
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicButtonSelectedColorNewStyle, 1f);
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06001207 RID: 4615 RVA: 0x0005BD15 File Offset: 0x00059F15
		public static Color BasicButtonReadyColorNewStyle
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicButtonReadyColorNewStyle, 1f);
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06001208 RID: 4616 RVA: 0x0005BD26 File Offset: 0x00059F26
		public static Color BasicButtonNotReadyColorNewStyle
		{
			get
			{
				return UISettings.ParseColor(UISettings._basicButtonNotReadyColorNewStyle, 1f);
			}
		}

		// Token: 0x04000D8F RID: 3471
		public static string BasicFontName = "cinzel.decorative-regular";

		// Token: 0x04000D90 RID: 3472
		public static string OptionsFontName = "Noto_Sans/NotoSans-Regular";

		// Token: 0x04000D91 RID: 3473
		public static string _basicBackgroundColor = "#1A1A1B";

		// Token: 0x04000D92 RID: 3474
		private static string _basicPanelColor = "#3D3D3D";

		// Token: 0x04000D93 RID: 3475
		private static string _basicIconColor = "#71B9AE";

		// Token: 0x04000D94 RID: 3476
		private static string _basicBackgroundMenuColor = "#000000";

		// Token: 0x04000D95 RID: 3477
		private static string _basicBackgroundMainMenuColor = "#440000";

		// Token: 0x04000D96 RID: 3478
		private static string _basicSubPanelColor = "#3D3D3D";

		// Token: 0x04000D97 RID: 3479
		private static string _basicOverlayPanelColor = "#464646";

		// Token: 0x04000D98 RID: 3480
		private static string _basicButtonColor = "#2B3233";

		// Token: 0x04000D99 RID: 3481
		private static string _basicButtonSelectedColor = "#0D7377";

		// Token: 0x04000D9A RID: 3482
		private static string _basicButtonReadyColor = "#1B8A40";

		// Token: 0x04000D9B RID: 3483
		private static string _basicButtonNotReadyColor = "#8A1C1C";

		// Token: 0x04000D9C RID: 3484
		private static string _basicTextReadyColor = "#42ff81";

		// Token: 0x04000D9D RID: 3485
		private static string _basicTextNotReadyColor = "#ff4242";

		// Token: 0x04000D9E RID: 3486
		public static string _basicTextColor = "#94F3E4";

		// Token: 0x04000D9F RID: 3487
		public static string _basicSystemTextColor = "#f3ee94";

		// Token: 0x04000DA0 RID: 3488
		private static string _basicScrollbarColor = "#0D7377";

		// Token: 0x04000DA1 RID: 3489
		private static string _basicScrollbarBackgroundColor = "#484849";

		// Token: 0x04000DA2 RID: 3490
		private static string _selectionBoxColor = "#ffffff";

		// Token: 0x04000DA3 RID: 3491
		private static string _selectionBoxBorderColor = "#000000";

		// Token: 0x04000DA4 RID: 3492
		private static string _basicTableTitleRowColor = "#2E2E2E";

		// Token: 0x04000DA5 RID: 3493
		public static string _basicDisabledColor = "#4D4D4D";

		// Token: 0x04000DA6 RID: 3494
		private static string _basicDisabledTextColor = "#A4A4A4";

		// Token: 0x04000DA7 RID: 3495
		private static string _brightTest = "#FF00FF";

		// Token: 0x04000DA8 RID: 3496
		private static string _handHoldColor = "#40FF81";

		// Token: 0x04000DA9 RID: 3497
		private static string _handLooseHoldColor = "#FF8D40";

		// Token: 0x04000DAA RID: 3498
		private static string _handNoHoldColor = "#FF40BD";

		// Token: 0x04000DAB RID: 3499
		private static string _basicButtonColorNewStyle = "#FFFFFF";

		// Token: 0x04000DAC RID: 3500
		private static string _basicButtonSelectedColorNewStyle = "#1CF7FF";

		// Token: 0x04000DAD RID: 3501
		private static string _basicButtonReadyColorNewStyle = "#32FF77";

		// Token: 0x04000DAE RID: 3502
		private static string _basicButtonNotReadyColorNewStyle = "#FF3434";
	}
}
