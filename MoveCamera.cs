// MoveCamera
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
	public Transform player;

	public Vector3 offset;

	private Camera cam;

	public static MoveCamera Instance { get; private set; }

	private void Start()
	{
		Instance = this;
		cam = base.transform.GetChild(0).GetComponent<Camera>();
		cam.fieldOfView = GameState.Instance.fov;
		offset = base.transform.position - player.transform.position;

		rb = PlayerMovement.Instance.GetRb();
	}

	private void Update()
	{
		base.transform.position = player.transform.position + bobOffset + desyncOffset + vaultOffset + offset;
	}

	public void UpdateFov()
	{
		//cam.fieldOfView = GameState.Instance.fov;
		cam.fieldOfView = 80f;
	}

	public Vector3 desyncOffset;

	public Vector3 vaultOffset;

	private Rigidbody rb;

	public PlayerMovement playerMovement;

	public bool cinematic;

	private float desiredTilt;

	private float tilt;

	private Vector3 desiredBob;

	private Vector3 bobOffset;

	private float bobSpeed = 15f;

	private float bobMultiplier = 1f;

	private readonly float bobConstant = 0.2f;

	public Camera mainCam;

	private void LateUpdate()
	{
		UpdateBob();
		MoveGun();
		if (!cinematic)
		{
			Vector3 cameraRot = playerMovement.cameraRot;
			cameraRot.x = Mathf.Clamp(cameraRot.x, -90f, 90f);
			base.transform.rotation = Quaternion.Euler(cameraRot);
			//desyncOffset = Vector3.Lerp(desyncOffset, Vector3.zero, Time.deltaTime * 15f);
			vaultOffset = Vector3.Slerp(vaultOffset, Vector3.zero, Time.deltaTime * 7f);
			//if (PlayerMovement.Instance.IsCrouching())
			//{
				//desiredTilt = 6f;
			//}
			//else
			//{
			//	desiredTilt = 0f;
			//}
			//tilt = Mathf.Lerp(tilt, desiredTilt, Time.deltaTime * 8f);
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			//eulerAngles.z = tilt;
			base.transform.rotation = Quaternion.Euler(eulerAngles);
		}
	}

	private void MoveGun()
	{
		if ((bool)rb && !(Mathf.Abs(rb.velocity.magnitude) < 4f) && PlayerMovement.Instance.grounded)
		{
			PlayerMovement.Instance.IsCrouching();
		}
	}

	public void BobOnce(Vector3 bobDirection)
	{
		Vector3 vector = ClampVector(bobDirection * 0.15f, -3f, 3f);
		desiredBob = vector * bobMultiplier;
	}

	private void UpdateBob()
	{
		desiredBob = Vector3.Lerp(desiredBob, Vector3.zero, Time.deltaTime * bobSpeed * 0.5f);
		bobOffset = Vector3.Lerp(bobOffset, desiredBob, Time.deltaTime * bobSpeed);
	}

	private Vector3 ClampVector(Vector3 vec, float min, float max)
	{
		return new Vector3(Mathf.Clamp(vec.x, min, max), Mathf.Clamp(vec.y, min, max), Mathf.Clamp(vec.z, min, max));
	}
}
