using System.Collections;
using UnityEngine;
using TMPro;

public class TipManager : MonoBehaviour 
{
	[SerializeField]
	private string[] TipList;

	[SerializeField]
	private int TotalTips;

	private int CurrentTip;

	[SerializeField]
	private TextMeshProUGUI T_Tip;

	void Start()
	{
		T_Tip.text = TipList[CurrentTip];
	}

	public void ShowNextTip()
	{
		CurrentTip++;

		if(CurrentTip >= TotalTips)
		{
			CurrentTip = 0;
		}

		T_Tip.text = TipList[CurrentTip];
	}
}