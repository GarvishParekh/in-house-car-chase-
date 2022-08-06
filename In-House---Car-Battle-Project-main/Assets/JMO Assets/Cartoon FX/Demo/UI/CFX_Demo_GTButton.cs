﻿using UnityEngine;
using System.Collections;

// Cartoon FX - (c) 2014 - Jean Moreno
//
// Script for the Demo scene

public class CFX_Demo_GTButton : MonoBehaviour
{
	public Color NormalColor = new Color32(128,128,128,128), HoverColor = new Color32(128,128,128,128);
	
	public string Callback;
	public GameObject Receiver;
	
	//private Rect CollisionRect;
	private bool Over;
	
	//-------------------------------------------------------------
	
	void Awake()
	{
		//CollisionRect = this.GetComponent<GUITexture>().GetScreenRect(Camera.main);
	}
	
	//-------------------------------------------------------------
	
	private void OnClick()
	{
		Receiver.SendMessage(Callback);
	}
}
