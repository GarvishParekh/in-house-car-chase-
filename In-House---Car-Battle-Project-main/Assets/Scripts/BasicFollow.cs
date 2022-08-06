using UnityEngine;

public class BasicFollow : MonoBehaviour 
{
	public Transform Target;

	void Update () 
	{
		if(Target)
		{
			transform.position = Target.position;
		}
	}
}