using UnityEngine;

public class CollisionParticles : MonoBehaviour 
{
	public ParticleSystem[] Sparks;
	public int TotalSparks;
	public int CurrentSpark;

	void OnEnable()
	{
		PlayerCarTrigger.OnPlayerCarCollision += ShowSparkAtPoint;
	}

	void OnDisable()
	{
		PlayerCarTrigger.OnPlayerCarCollision -= ShowSparkAtPoint;
	}

	void Start()
	{
		TotalSparks = Sparks.Length;
	}

	public void ShowSparkAtPoint(Vector3 Pos)
	{
		if(TotalSparks > 0)
		{
			if(!Sparks[CurrentSpark].isPlaying)
			{
				Sparks[CurrentSpark].transform.position = Pos;
				Sparks[CurrentSpark].Play(true);
			}

			CurrentSpark++;

			if(CurrentSpark >= TotalSparks)
			{
				CurrentSpark = 0;
			}
		}
	}
}