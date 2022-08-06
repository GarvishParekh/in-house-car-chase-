using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour 
{
	[SerializeField]
	private AudioClip[] Musics;

	private int CurrentMusic;

	[SerializeField]
	private AudioSource AS;

	[SerializeField]
	private Animator Anim;

	void Start () 
	{
		CurrentMusic = PlayerPrefs.GetInt("LastMenuMusic", 0);

		CurrentMusic++;

		if(CurrentMusic >= Musics.Length)
		{
			CurrentMusic = 0;
		}

		PlayerPrefs.SetInt("LastMenuMusic", CurrentMusic);

		AS.clip = Musics[CurrentMusic];
		AS.Play();
	}

	void OnLeavingCurrentScene()
	{
		Anim.Play("MusicFadeOut");
	}
}