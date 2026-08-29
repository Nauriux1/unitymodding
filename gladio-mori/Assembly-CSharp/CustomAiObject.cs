using System;
using UnityEngine;

// Token: 0x0200005F RID: 95
[CreateAssetMenu(fileName = "CustomAiObject", menuName = "Ai/CustomAiObject", order = 1)]
public class CustomAiObject : ScriptableObject
{
	// Token: 0x04000198 RID: 408
	public bool useOverrideWalkDistance;

	// Token: 0x04000199 RID: 409
	public float overrideMaxWalkDistance;

	// Token: 0x0400019A RID: 410
	public float overrideMinWalkDistance;
}
