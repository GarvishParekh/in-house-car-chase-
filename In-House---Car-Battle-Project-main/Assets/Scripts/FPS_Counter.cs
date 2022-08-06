using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPS_Counter : MonoBehaviour 
{
	public TextMeshProUGUI T;
	public int CurrentFPS;
	private float Counter;

	void Start () 
	{
		if(Application.platform == RuntimePlatform.Android)
		{
			Application.targetFrameRate = 100;
		}

		Counter = 1.0f;
		T.text = "--";
	}
	
	void Update () 
	{
		Counter -= Time.deltaTime;
		CurrentFPS++;

		if(Counter <= 0)
		{
			T.text = CurrentFPS.ToString();
			CurrentFPS = 0;
			Counter = 1.0f;
		}
	}
}