using UnityEngine;

[RequireComponent(typeof(Car_AI))]
[RequireComponent(typeof(Rigidbody))]

public class AICarExplosion : MonoBehaviour 
{
	public bool isReady;
	private Vector3 OffCamPos;

	[Header("Explosion Configuration")]

	public float ExplosionPower;
	public float ExplosionRange;
	public Transform ExplosionPos;
	public ForceMode FM;

	[Header("Objects Deteachment")]

	public WheelCollider WC_FL;
	public WheelCollider WC_FR;
	public WheelCollider WC_RL;
	public WheelCollider WC_RR;

	public Rigidbody W_Mesh_FL;
	public Rigidbody W_Mesh_FR;
	public Rigidbody W_Mesh_RL;
	public Rigidbody W_Mesh_RR;

	public Rigidbody W_Mesh_Extra1;
	public Rigidbody W_Mesh_Extra2;

	public Transform COM_Explosion;

	public float WheelForceMultiplier;

	private Rigidbody _RB;
	private Car_AI AiCar;

	[Header("Explosion Effect")]

	public ParticleSystem ParticleFire;
	public Renderer[] BodyParts;
	public Color ColDark;

	void Start()
	{
		_RB = GetComponent<Rigidbody>();
		AiCar = GetComponent<Car_AI>();
	}

	public void OnExplosion ()
	{
		if(!AiCar.isExploded)
		{
			AiCar.isExploded = true;
			_RB.drag = 1.2f;
			_RB.centerOfMass = COM_Explosion.localPosition;

			_RB.AddExplosionForce(ExplosionPower * 1.3f, ExplosionPos.position, ExplosionRange, 1.0f, FM);

			DeteachWheel(WC_FL, W_Mesh_FL);
			DeteachWheel(WC_FR, W_Mesh_FR);
			DeteachWheel(WC_RL, W_Mesh_RL);
			DeteachWheel(WC_RR, W_Mesh_RR);

			if(W_Mesh_Extra1)
			{
				DeteachObject(W_Mesh_Extra1);
			}

			if(W_Mesh_Extra2)
			{
				DeteachObject(W_Mesh_Extra2);
			}

			ParticleFire.Play();

			if(BodyParts.Length > 0)
			{
				foreach(Renderer R in BodyParts)
				{
					if(R)
					{
						R.material.color = ColDark;
					}
				}
			}
		}
	}

	void DeteachWheel(WheelCollider WC, Rigidbody R)
	{
		WC.gameObject.SetActive(false);

		R.transform.SetParent(null);
		R.interpolation = RigidbodyInterpolation.Interpolate;
		R.isKinematic = false;

		R.AddExplosionForce(ExplosionPower * WheelForceMultiplier, ExplosionPos.position, ExplosionRange, 1.0f, FM);

		if(R.gameObject.GetComponent<BoxCollider>())
		{
			R.gameObject.GetComponent<BoxCollider>().enabled = true;
		}
	}

	void DeteachObject(Rigidbody R)
	{
		R.transform.SetParent(null);
		R.interpolation = RigidbodyInterpolation.Interpolate;
		R.isKinematic = false;

		R.AddExplosionForce(ExplosionPower * WheelForceMultiplier, ExplosionPos.position, ExplosionRange, 1.0f, FM);

		if(R.gameObject.GetComponent<BoxCollider>())
		{
			R.gameObject.GetComponent<BoxCollider>().enabled = true;
		}
	}
}