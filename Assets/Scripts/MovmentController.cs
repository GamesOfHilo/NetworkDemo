using UnityEngine;
using System.Collections;
using System;
using MovementEventArgs;

[RequireComponent(typeof(NetworkView))]
[RequireComponent(typeof(Rigidbody))]
public class MovmentController : MonoBehaviour
{

    public event EventHandler<JumpEventArgs> Jump;
    public event EventHandler<LandedEventArgs> Landed;
    public event EventHandler<StartSprintingEventArgs> StartSprinting;
    public event EventHandler<EndSprintingEventArgs> EndSprinting;

    public event EventHandler<BeginWalkEventArgs> BeginWalk;
    public event EventHandler<EndWalkEventArgs> EndWalk;

    public Vector2 VelocityMulitplier = Vector3.one;
    public Vector2 camspeed = Vector3.one;

    public Vector2 SprintMultiplier = new Vector2(2, 2);

    public float UpAngleMax = 90.0f;
    public float DownAngleMax = -90.0f;

    private float yRotation = 0.0f;
    private float xRotation = 0.0f;

    private NetworkController networkController;

    public bool useGravity = true;
    public float gravityForce = 2.0f;
    public float jumpForce = 2.0f;

    public bool useSendMessage = false;

    private bool jumps = false;
    private bool sprints = false;

    // Use this for initialization
    void Start()
    {
        rigidbody.freezeRotation = true;
        networkController = GameObject.Find("GameController").GetComponent<NetworkController>();
    }


    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rigidbody.velocity.y < 0.01f)
            jumps = false;
    }


    void FixedUpdate()
    {
        if (networkView.isMine || !networkController.isConnected)
        {
            yRotation += Input.GetAxis("Mouse X") * camspeed.y;
            xRotation -= Input.GetAxis("Mouse Y") * camspeed.x;
            xRotation = Mathf.Clamp(xRotation, DownAngleMax, UpAngleMax);

            sprints = Input.GetAxis("Sprint") > 0.8f;

            if (!jumps)
                rigidbody.velocity = Quaternion.Euler(0, yRotation, 0) *
                    (new Vector3(Input.GetAxis("Horizontal") * VelocityMulitplier.x * (sprints ? SprintMultiplier.x : 1),
                    rigidbody.velocity.y, Input.GetAxis("Vertical") * VelocityMulitplier.y * (sprints ? SprintMultiplier.y : 1)));

            rigidbody.rotation = Quaternion.Euler(0, yRotation, 0);
            GetComponentInChildren<Camera>().transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            if (Input.GetAxis("Jump") > 0.8f && !jumps)
            {
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumps = true;
            }
            if (useGravity)
            {
                rigidbody.AddForce(Vector3.down * 9.81f * gravityForce, ForceMode.Acceleration);
            }
        }
    }


}
