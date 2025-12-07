using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RefreshButton : MonoBehaviour
{
	// Start is called before the first frame update
	[HideInInspector]
	public bool IsRefreshButtonClicked = false;

	void Start()
	{
		gameObject.GetComponent<Button>().onClick.AddListener(RefreshButtonClickedListener);
	}

	private void RefreshButtonClickedListener()
	{
		IsRefreshButtonClicked = true;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
