using UnityEngine;
using System.Collections;

public class SpaceshipController : MonoBehaviour
{
	float geschwindigkeit;
	public int standartGeschwindigkeit;
	float deltaSpeed;//(geschwindigkeit - standartGeschwindigkeit)
	public int minSpeed;
	public int maxSpeed;
	public float beschleunigung, bremsen;

	Vector3 angVel;
	Vector3 shipRot;
	public int sensitivity;
	
	public Vector3 cameraOffset; 
	
	void Start()
	{
		geschwindigkeit = standartGeschwindigkeit;

	}
	
	void FixedUpdate()
	{	
		shipRot = transform.GetChild(1).localEulerAngles; 

		if (shipRot.x > 180) shipRot.x -= 360;
		if (shipRot.y > 180) shipRot.y -= 360;
		if (shipRot.z > 180) shipRot.z -= 360;

		angVel.x += Input.GetAxis("Vertical") * Mathf.Abs(Input.GetAxis("Vertical")) * sensitivity * Time.fixedDeltaTime;

		float turn = Input.GetAxis("Horizontal") * Mathf.Abs(Input.GetAxis("Horizontal")) * sensitivity * Time.fixedDeltaTime;
		angVel.y += turn * .5f;
		angVel.z -= turn * .5f;

		if (Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.I))
		{
			angVel.y -= 20;
			angVel.z += 50;
			geschwindigkeit -= 5 * Time.fixedDeltaTime;
		}
		
		if (Input.GetKey(KeyCode.Joystick1Button5) || Input.GetKey(KeyCode.O))
		{
			angVel.y += 20;
			angVel.z -= 50;
			geschwindigkeit -= 5 * Time.fixedDeltaTime;
		}

		angVel /= 1 + deltaSpeed * .001f;

		angVel -= angVel.normalized * angVel.sqrMagnitude * .08f * Time.fixedDeltaTime;

		transform.GetChild(1).Rotate(angVel * Time.fixedDeltaTime);

		transform.GetChild(1).Rotate(-shipRot.normalized * .015f * (shipRot.sqrMagnitude + 500) * (1 + geschwindigkeit / maxSpeed) * Time.fixedDeltaTime);
		
		deltaSpeed = geschwindigkeit - standartGeschwindigkeit;

        bremsen = geschwindigkeit - minSpeed;
        beschleunigung = maxSpeed - geschwindigkeit;

		if (Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.LeftShift))
			geschwindigkeit = geschwindigkeit + 100 * Time.fixedDeltaTime;
		else if (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Space))
            geschwindigkeit -= bremsen * Time.fixedDeltaTime;

		else if (Mathf.Abs(deltaSpeed) > .1f)
			geschwindigkeit -= Mathf.Clamp(deltaSpeed * Mathf.Abs(deltaSpeed), -30, 100) * Time.fixedDeltaTime;

		transform.GetChild(0).localPosition = cameraOffset + new Vector3(0, 0, -deltaSpeed * .02f);
		
		
		float sqrOffset = transform.GetChild(1).localPosition.sqrMagnitude;
		Vector3 offsetDir = transform.GetChild(1).localPosition.normalized;

		transform.GetChild(1).Translate(-offsetDir * sqrOffset * 20 * Time.fixedDeltaTime);
		transform.Translate((offsetDir * sqrOffset * 50 + transform.GetChild(1).forward * geschwindigkeit) * Time.fixedDeltaTime, Space.World);
		transform.Rotate(shipRot.x * Time.fixedDeltaTime, (shipRot.y * Mathf.Abs(shipRot.y) * .02f) * Time.fixedDeltaTime, shipRot.z * Time.fixedDeltaTime);
	}
	
	void Update()
	{
	}
}