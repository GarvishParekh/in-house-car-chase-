using UnityEngine;

public class ParticleExplosion : MonoBehaviour 
{
	public ParticleSystem Particles1;
	public ParticleSystem Particles2;
	public ParticleSystem Particles3;

	void OnEnable()
	{
		PlayerCarHealth.OnPlayerCarExplosion += OnCarExploaded;
	}

	void OnDisable()
	{
		PlayerCarHealth.OnPlayerCarExplosion -= OnCarExploaded;
	}

	void OnCarExploaded()
	{
		Particles1.Play();
		Particles2.Play();
		Particles3.Play();
	}
}