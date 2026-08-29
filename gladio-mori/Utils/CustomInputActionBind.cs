using System;
using System.Collections.Generic;

namespace Utils
{
	// Token: 0x0200028A RID: 650
	internal class CustomInputActionBind
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06001339 RID: 4921 RVA: 0x00063B0D File Offset: 0x00061D0D
		// (set) Token: 0x0600133A RID: 4922 RVA: 0x00063B15 File Offset: 0x00061D15
		public string inputActionName { get; set; }

		// Token: 0x04000E2C RID: 3628
		public List<CustomKeyBind> customKeyBind = new List<CustomKeyBind>();
	}
}
