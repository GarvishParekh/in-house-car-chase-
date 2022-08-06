using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMoveToPoint : MonoBehaviour 
{
	public ParticleSystem PS;

	public ParticleSystem.Particle[] Particles;
	public bool isEnabled;
	public Transform TargetPos;
	public float Speed;
	public int TotalParticles;
	public Color CBlank;
	private int TotalParticlesProcessed;

	void Start () 
	{
		Invoke("SetNow", 2);
	}

	void SetNow()
	{
		Particles = new ParticleSystem.Particle[PS.particleCount];

		TotalParticles = PS.GetParticles(Particles);

		isEnabled = true;
	}

	void Update () 
	{
		if(isEnabled)
		{
			for(int i = 0; i < TotalParticles; i++)
			{
				Particles[i].position = Vector3.MoveTowards(Particles[i].position, TargetPos.position, Speed * Time.deltaTime);

				if(Particles[i].position == TargetPos.position)
				{
					Particles[i].startColor = CBlank;
				}
			}

			PS.SetParticles(Particles, TotalParticles);
		}
	}
}