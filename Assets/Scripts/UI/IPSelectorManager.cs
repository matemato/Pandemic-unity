using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopulateScrollView : MonoBehaviour
{
	[SerializeField] private Transform m_ContentContainer;
	[SerializeField] private GameObject m_ItemPrefab;
	[SerializeField] private int m_ItemsToGenerate;

	void Start()
	{
		for (int i = 0; i < m_ItemsToGenerate; i++)
		{
			var item_go = Instantiate(m_ItemPrefab);
			// do something with the instantiated item -- for instance
			item_go.GetComponent<TMP_Text>().text = "Item #" + i;
			//parent the item to the content container
			item_go.transform.SetParent(m_ContentContainer);
			//reset the item's scale -- this can get munged with UI prefabs
			item_go.transform.localScale = Vector2.one;
		}
	}

	public void ClearList()
	{
		var IpObjects = GameObject.FindGameObjectsWithTag("IpText");
		foreach (var ipObject in IpObjects)
		{
			Destroy(ipObject);
		}
	}

	public void AddToList(string IP)
	{
		var item_go = Instantiate(m_ItemPrefab);
		item_go.GetComponent<TMP_Text>().text = IP;
		item_go.transform.SetParent(m_ContentContainer);
		item_go.transform.localScale = Vector2.one;
	}
}