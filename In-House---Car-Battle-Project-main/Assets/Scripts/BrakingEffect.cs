using UnityEngine;

public class BrakingEffect : MonoBehaviour 
{
	public bool isLeft;
	public bool isRight;

	public enum Effect_Type
	{
		Mesh,
		Texture
	}

	public Effect_Type EffectType;

	[Header("For Mesh Type")]

	public Renderer R1;
	public Renderer R2;

	public Color ColON;
	public Color ColOFF;

	[Header("For Texture Type")]

	public Renderer Car;
	public Texture TexBrakeON;
	public Texture TexBrakeOFF;

	[Header("Common Varialbles")]

	public LensFlare Flare1;
	public LensFlare Flare2;

	private float DefaultBrightness;
	public bool isEditor;

	public MyCameraFollow.Cam_Positions CamPosition;
	public bool isBreaking;

	void OnEnable()
	{
		DeviceInput.OnLeftUp += OnLeftUp;
		DeviceInput.OnLeftDown += OnLeftDown;

		DeviceInput.OnRightUp += OnRightUp;
		DeviceInput.OnRightDown += OnRightDown;

		MyCameraFollow.OnCameraViewChanged += OnCamViewChanged;
	}

	void OnDisable()
	{
		DeviceInput.OnLeftUp -= OnLeftUp;
		DeviceInput.OnLeftDown -= OnLeftDown;

		DeviceInput.OnRightUp -= OnRightUp;
		DeviceInput.OnRightDown -= OnRightDown;

		MyCameraFollow.OnCameraViewChanged -= OnCamViewChanged;
	}


	void Start () 
	{
		DefaultBrightness = Flare1.brightness;
		Brake_OFF();

		if(Application.platform == RuntimePlatform.WindowsEditor)
		{
			isEditor = true;
		}
		else
		{
			isEditor = false;
		}
	}



	void OnLeftUp()
	{
		isLeft = false;
		UpdateCarVisuals();
	}

	void OnLeftDown()
	{
		isLeft = true;
		UpdateCarVisuals();
	}

	void OnRightUp()
	{
		isRight = false;
		UpdateCarVisuals();
	}

	void OnRightDown()
	{
		isRight = true;
		UpdateCarVisuals();
	}



	void UpdateCarVisuals()
	{
		if(CamPosition == MyCameraFollow.Cam_Positions.Racing)
		{
			if(isLeft && isRight)
			{
				Brake_ON();
			}
			else
			{
				Brake_OFF();
			}
		}
		else
		{
			if(isBreaking) // Check if the breaking effect is ON when Top View camera is enabled, turn it off.
			{
				Brake_OFF();
			}
		}
	}

	void Brake_ON()
	{
		isBreaking = true;

		if(EffectType == Effect_Type.Mesh)
		{
			R1.material.color = ColON;
			R2.material.color = ColON;

			Flare1.brightness = DefaultBrightness;
			Flare2.brightness = DefaultBrightness;
		}
		else
		{
			//
		}
	}

	void Brake_OFF()
	{
		isBreaking = false;

		if(EffectType == Effect_Type.Mesh)
		{
			R1.material.color = ColOFF;
			R2.material.color = ColOFF;

			Flare1.brightness = 0;
			Flare2.brightness = 0;
		}
		else
		{

		}
	}

	void OnCamViewChanged(MyCameraFollow.Cam_Positions Pos)
	{
		CamPosition = Pos;

		if(Pos == MyCameraFollow.Cam_Positions.Top_Down)
		{
			if(isBreaking) // Check if the breaking effect is ON when Top View camera is enabled, turn it off.
			{
				Brake_OFF();
			}
		}
		else
		{
			if(isRight && isLeft) // Turn on brakes if camera mode being switched to Racing while brakes are ON.
			{
				Brake_ON();
			}
		}
	}


	void Update()
	{
		if(isEditor)
		{
			if(Input.GetKeyUp(KeyCode.A))
			{
				isLeft = false;
				UpdateCarVisuals();
			}
			else if(Input.GetKeyUp(KeyCode.D))
			{
				isRight = false;
				UpdateCarVisuals();
			}

			if(Input.GetKeyDown(KeyCode.A))
			{
				isLeft = true;
				UpdateCarVisuals();
			}
			else if(Input.GetKeyDown(KeyCode.D))
			{
				isRight = true;
				UpdateCarVisuals();
			}
		}
	}
}