using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
[RequireComponent(typeof(Rigidbody))]
public class MovmentController : MonoBehaviour
{

    public Vector2 speed;
    public Vector2 camspeed;

    public float UpAngleMax = 90.0f;
    public float DownAngleMax = -90.0f;

    private float yRotation = 0.0f;
    private float xRotation = 0.0f;

    private NetworkController networkcontroller;

    // Use this for initialization
    void Start()
    {
        rigidbody.freezeRotation = true;
        networkcontroller = GameObject.Find("GameController").GetComponent<NetworkController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (networkView.isMine || !networkcontroller.isConnected)
        {
            yRotation += Input.GetAxis("Mouse X");
            xRotation -= Input.GetAxis("Mouse Y");
            xRotation = Mathf.Clamp(xRotation, DownAngleMax, UpAngleMax);
            rigidbody.position += Quaternion.Euler(0, yRotation, 0) * (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
            rigidbody.rotation = Quaternion.Euler(0, yRotation, 0);
            GetComponentInChildren<Camera>().transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
    }
}
