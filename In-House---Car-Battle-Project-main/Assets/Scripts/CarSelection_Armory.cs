using UnityEngine;

public class CarSelection_Armory : MonoBehaviour 
{
	public GameObject[] Cars;
	public Transform[] CamTrans;

	public Transform Cam;
	public float CamMoveSpeed;
	public float CamRotSpeed;

	public int SelectedCar;

	private Transform TargetTransform;

	void Start () 
	{
		ShowCar (true);
	}



	void Update()
	{
		Cam.position = Vector3.Lerp (Cam.position, TargetTransform.position, CamMoveSpeed * Time.deltaTime);
		Cam.eulerAngles = Vector3.Lerp (Cam.eulerAngles, TargetTransform.eulerAngles, CamRotSpeed * Time.deltaTime);
	}




	void ShowCar(bool ShowDirect = false)
	{
		foreach(GameObject G in Cars)
		{
			if(G.activeSelf)
			{
				G.SetActive (false);
			}
		}

		Cars [SelectedCar].SetActive (true);

		if(ShowDirect)
		{
			Cam.position = CamTrans [SelectedCar].position;
			Cam.eulerAngles = CamTrans [SelectedCar].eulerAngles;
		}

		TargetTransform = CamTrans [SelectedCar];
	}

	public void OnCarSelect(int ID)
	{
		SelectedCar = ID;
		ShowCar (false);
	}
}