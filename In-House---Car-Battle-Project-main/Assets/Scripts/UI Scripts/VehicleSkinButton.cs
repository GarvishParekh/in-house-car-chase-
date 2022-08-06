using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VehicleSkinButton : MonoBehaviour, IPointerClickHandler
{
	public int ButtonIndex;
	public int SkinStorageID;
	public int SkinListIndex;

	public Image ImSkin;
	public Image ImBG;

	public TextMeshProUGUI T_Cost;
	public Image RightMark;
	public GameObject CoinImage;

	public static Action<int, int> OnSkinButtonClicked;

	public void OnPointerClick(PointerEventData S)
	{
		if(OnSkinButtonClicked != null)
		{
			OnSkinButtonClicked(ButtonIndex, SkinListIndex);
		}
	}

	public void SkinPurchased()
	{
		T_Cost.gameObject.SetActive(false);
		RightMark.gameObject.SetActive(true);
		CoinImage.SetActive(false);
	}

	public void SkinNotPurchased()
	{
		T_Cost.gameObject.SetActive(true);
		RightMark.gameObject.SetActive(false);
		CoinImage.SetActive(true);
	}

	public void SkinSelected()
	{
		T_Cost.gameObject.SetActive(false);
		RightMark.gameObject.SetActive(true);
		CoinImage.SetActive(false);
	}
}