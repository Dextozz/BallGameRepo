using UnityEngine;

public class Movement : MonoBehaviour {

    [HideInInspector]
    public bool isAlive;
    public float forwardVel = 16f;
    public float jumpHeight;
    public float jumpMultiplier;
    [HideInInspector]
    public static bool respawn;
    public float speedLimit;
    public VirtualJoystick joystick;

    Vector3 moveInput;
    float distToGround;
    bool detectInput;
    bool hasJumped;
    bool isGrounded;
    int counter = 0;

    //Vector3 tempY;
    Vector3 keepVelocity;
    Vector3 moveDirection;
    Rigidbody rb;
    Transform cameraPos;
    Transform GCV;
    Transform GCH;
    SphereCollider sphereCollider;

    // Use this for initialization
    void Start()
    { 
        rb = GetComponent<Rigidbody>();
        cameraPos = GameObject.Find("Main Camera").GetComponent<Transform>();
        GCV = GameObject.Find("GCV").GetComponent<Transform>();
        GCH = GameObject.Find("GCH").GetComponent<Transform>();
        sphereCollider = GetComponent<SphereCollider>();

        //Get dist to the ground to check if player is grounded
        distToGround = GetComponent<Collider>().bounds.extents.y;
        isAlive = true;

        Debug.Log("test");
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = IsGrounded();
        moveInput = joystick.MoveInput();

        if (isAlive)
        {
            if(Input.GetKeyDown(KeyCode.Space))
                Jump();
        }

        //Keep the velocity so it doesn't change during jump but only do that when the player is grounded to prevent jaggy edge falling, reset the friction
        if (isGrounded)
        {
            SetFriction(0.6f);
            keepVelocity = rb.velocity;
            hasJumped = false;
        }
        else
        {
            //Make the player frictionless so he doesn't "slide" on the walls
            SetFriction(0.0f);
        }

        //Smoother jump (Gravity increases when falling)
        if (rb.velocity.y < 0 && !isGrounded)
        {
            //Its vector3.up because gravity is -9.81
            rb.AddForce(Vector3.up * Physics.gravity.y * jumpMultiplier * Time.deltaTime, ForceMode.Acceleration);
        }

        //Keep the velocity so it doesn't change during jump FUNC
        if (!isGrounded)
        {
            rb.velocity = new Vector3(keepVelocity.x / 0.8f, rb.velocity.y, keepVelocity.z / 0.8f);
        }

        //Prevent movement when respawning
        if (respawn && !isGrounded)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
            respawn = false;

        LimitSpeed();
        MoveDirection();
    }

    void LimitSpeed()
    {
        //Positive x
        if (rb.velocity.x > speedLimit)
            rb.velocity = new Vector3(speedLimit, rb.velocity.y, rb.velocity.z);

        //Negative x
        if (rb.velocity.x < -speedLimit)
            rb.velocity = new Vector3(-speedLimit, rb.velocity.y, rb.velocity.z);

        //Positive z
        if (rb.velocity.z > speedLimit)
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speedLimit);

        //Negative z
        if (rb.velocity.z < -speedLimit)
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -speedLimit);
    }

    void SetFriction(float value)
    {
        sphereCollider.material.dynamicFriction = value;
        sphereCollider.material.staticFriction = value;
    }

    //This is used to determine players forward
    void MoveDirection()
    {
        if ((int)cameraPos.eulerAngles.y > -45 && (int)cameraPos.eulerAngles.y < 45)
        {
            //The cam is looking forward
            moveDirection = moveInput;
        }
        else if((int)cameraPos.eulerAngles.y > 45 && (int)cameraPos.eulerAngles.y < 135)
        {
            //The cam is looking right
            moveDirection = new Vector3(moveInput.z, 0, -moveInput.x);
        }
        else if((int)cameraPos.eulerAngles.y > 225 && (int)cameraPos.eulerAngles.y < 315)
        {
            //The cam is looking left
            moveDirection = new Vector3(-moveInput.z, 0, moveInput.x);
        }
        else if((int)cameraPos.eulerAngles.y > 135 && (int)cameraPos.eulerAngles.y < 225)
        {
            //The cam is looking back
            moveDirection = -moveInput;
        }
    }

    void FixedUpdate()
    {
        //Position of CH
        Vector3 tempPos = cameraPos.position - transform.position;
        tempPos.y = 0;
        tempPos = tempPos.normalized;
        GCV.position = transform.position + tempPos * 10;

        //Need this for some reason???
        GCH.LookAt(transform.position);
        GCV.LookAt(transform.position);

        //Moving the ball based on moveInput 
        if (isAlive && isGrounded && !hasJumped)
        {
            rb.velocity = new Vector3(moveDirection.x * forwardVel, rb.velocity.y, moveDirection.z * forwardVel);
        }
    }

    public void Jump()
    {
        if (isGrounded && isAlive)
        {
            hasJumped = true;

            //Keep the players velocity on X and Z but set in on the Y
            Vector3 temp = rb.velocity;
            temp.y = jumpHeight;
            rb.velocity = temp;
        }
    }

    bool IsGrounded()
    {
        //I add 0.1f to account for the additional distance between player center and the ground when ground is at an angle
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
    }

    //For the maze camera
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CameraSnap")
        {
            CameraMovement.setTopDown = true;
        }

        if(other.tag == "ElevatorPlatformTrigger")
        {
            Debug.Log("HitElevator");
            ElevatorMovement.movePlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "CameraSnap")
        {
            CameraMovement.setTopDown = false;
        }

        if (other.tag == "ElevatorPlatformTrigger")
        {
            Debug.Log("LeftElevator");
            ElevatorMovement.movePlayer = false;
        }
    }
}