using System;
using System.Collections.Generic;
using MoveClasses;
using UnityEngine;
using UnityEngine.UI;
using Utils;

// Token: 0x02000210 RID: 528
public class MoveSetSelector : MonoBehaviour
{
	// Token: 0x06001014 RID: 4116 RVA: 0x00053A44 File Offset: 0x00051C44
	private void SelectMoveSet(MoveSet moveSet)
	{
		if (moveSet == null)
		{
			moveSet = MoveSetHelpers.CreateNewMoveSet();
		}
		SceneManagerWithParameters.LoadScene("MoveEditor", new Dictionary<string, object>
		{
			{
				"MoveSet",
				moveSet
			}
		}, false, false);
	}

	// Token: 0x06001015 RID: 4117 RVA: 0x00053A70 File Offset: 0x00051C70
	private void Start()
	{
		int num = 0;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
		Button component = gameObject.GetComponent<Button>();
		gameObject.GetComponentInChildren<Text>().text = "Add";
		component.onClick.AddListener(delegate()
		{
			this.SelectMoveSet(null);
		});
		gameObject.transform.SetParent(base.gameObject.transform);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		num++;
		gameObject.transform.SetSiblingIndex(num);
		using (List<MoveSet>.Enumerator enumerator = MoveSetHelpers.MoveSets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MoveSet move = enumerator.Current;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
				Button component2 = gameObject2.GetComponent<Button>();
				gameObject2.GetComponentInChildren<Text>().text = move.name;
				component2.onClick.AddListener(delegate()
				{
					this.SelectMoveSet(move);
				});
				gameObject2.transform.SetParent(base.gameObject.transform);
				gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
				num++;
				gameObject2.transform.SetSiblingIndex(num);
			}
		}
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x0000777A File Offset: 0x0000597A
	private void Update()
	{
	}

	// Token: 0x04000B88 RID: 2952
	public GameObject buttonPrefab;
}
