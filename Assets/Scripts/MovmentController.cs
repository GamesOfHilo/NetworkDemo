using UnityEngine;
using System.Collections;
using System;
using MovementEventArgs;

[RequireComponent(typeof(NetworkView))]
[RequireComponent(typeof(Rigidbody))]
public class MovmentController : MonoBehaviour
{

    #region Events
    public event EventHandler<JumpEventArgs> Jump;
    protected void FireJump()
    {
        var myevent = Jump;
        if (myevent != null)
            myevent(gameObject, null);
        if (useSendMessage) SendMessage("OnJump", null);
    }
    public event EventHandler<FallsEventArgs> Falls;
    protected void FireFalls()
    {
        var myevent = Falls;
        if (myevent != null)
            myevent(gameObject, null);
        if (useSendMessage) SendMessage("OnFalls", null);
    }
    public event EventHandler<LandedEventArgs> Landed;
    protected void FireLanded()
    {
        var myevent = Landed;
        if (myevent != null)
            myevent(gameObject, null);
        if (useSendMessage) SendMessage("OnLanded", null);
    }
    public event EventHandler<BeginSprintingEventArgs> BeginSprinting;
    protected void FireBeginSprinting()
    {
        var myevent = BeginSprinting;
        if (myevent != null)
            myevent(gameObject, null);
        if (useSendMessage) SendMessage("OnBeginSprinting", null);
    }
    public event EventHandler<EndSprintingEventArgs> EndSprinting;
    protected void FireEndSprinting()
    {
        var myevent = EndSprinting;
        if (myevent != null)
            myevent(gameObject, null);
        if (useSendMessage) SendMessage("OnEndSprinting", null);
    }

    public event EventHandler<BeginWalkEventArgs> BeginWalk;
    protected void FireBeginWalk()
    {
        var myevent = BeginWalk;
        if (myevent != null)
            myevent(gameObject, null);
        if (useSendMessage) SendMessage("OnBeginWalk", null);
    }
    public event EventHandler<EndWalkEventArgs> EndWalk;
    protected void FireEndWalk()
    {
        var myevent = EndWalk;
        if (myevent != null)
            myevent(gameObject, null);
        if (useSendMessage) SendMessage("OnEndWalk", null);
    }
    #endregion

    #region Public Fields

    public Vector2 VelocityMulitplier = Vector3.one;
    public Vector2 camspeed = Vector3.one;

    public Vector2 SprintMultiplier = new Vector2(2, 2);

    public float UpAngleMax = 90.0f;
    public float DownAngleMax = -90.0f;

    public bool useGravity = true;
    public float gravityForce = 2.0f;
    public float jumpForce = 2.0f;

    public bool useSendMessage = false;
    #endregion

    #region Private Fields

    private float yRotation = 0.0f;
    private float xRotation = 0.0f;

    private NetworkController networkController;

    private bool jumps = false;
    private bool falls = false;
    private bool sprints = false;
    #endregion

    // Use this for initialization
    void Start()
    {
        rigidbody.freezeRotation = true;
        networkController = GameObject.Find("GameController").GetComponent<NetworkController>();
    }


    private bool lastSprint = false;
    private bool lastMoving = false;
    private bool moving = false;
    // Update is called once per frame
    void Update()
    {
        moving = rigidbody.velocity.getPlainMagnitude() > 0.1f;
        if (rigidbody.velocity.y < -0.1f)
        {
            falls = true;
            jumps = false;
            FireFalls();
        }
        else if (rigidbody.velocity.y > 0.1)
        {
            falls = false;
            jumps = true;
            FireJump();
        }
        if (sprints != lastSprint)
        {
            if (sprints) FireBeginSprinting();
            else FireEndSprinting();
            lastSprint = sprints;
        }
        if (moving != lastMoving)
        {
            if (moving) FireBeginWalk();
            else FireEndWalk();
            lastMoving = moving;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rigidbody.velocity.y < 0.1f && rigidbody.velocity.y > -0.1f)
        {
            jumps = false;
            falls = false;
            FireLanded();
        }
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

            if (Input.GetAxis("Jump") > 0.8f && !(jumps || falls))
            {
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            if (useGravity)
            {
                rigidbody.AddForce(Vector3.down * 9.81f * gravityForce, ForceMode.Acceleration);
            }
        }
    }


}
