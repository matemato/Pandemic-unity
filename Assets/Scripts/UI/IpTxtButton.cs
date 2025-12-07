using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IpTxtButton : MonoBehaviour
{
	// Start is called before the first frame update
	[HideInInspector]
	public bool IsIpClicked = false;

	public void SetClickedColor()
	{
		var colors = gameObject.GetComponent<Button>().colors;
		colors.normalColor = Color.green;
		colors.selectedColor = Color.green;
		colors.pressedColor = Color.green;
		colors.highlightedColor = Color.green;
		gameObject.GetComponent<Button>().colors = colors;
	}

	public void SetNormalColor()
	{
		var colors = gameObject.GetComponent<Button>().colors;
		colors.normalColor = Color.white;
		colors.selectedColor = Color.white;
		colors.pressedColor = Color.white;
		colors.highlightedColor= Color.white;
		gameObject.GetComponent<Button>().colors = colors;
	}

	void Start()
	{
		gameObject.GetComponent<Button>().onClick.AddListener(IpClickedListener);
	}

	private void IpClickedListener()
	{
		IsIpClicked = true;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
