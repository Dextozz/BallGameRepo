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

    float moveHorizontal;
    float moveVertical;
    float distToGround;
    bool detectInput;
    bool hasJumped;
    int counter = 0;

    //Vector3 tempY;
    Vector3 keepVelocity;
    Rigidbody rb;
    Transform cameraPos;
    Transform GCV;
    Transform GCH;
    LayerMask waterMask;
    SphereCollider sphereCollider;

    // Use this for initialization
    void Start()
    { 
        rb = GetComponent<Rigidbody>();
        cameraPos = GameObject.Find("Main Camera").GetComponent<Transform>();
        GCV = GameObject.Find("GCV").GetComponent<Transform>();
        GCH = GameObject.Find("GCH").GetComponent<Transform>();
        sphereCollider = GetComponent<SphereCollider>();
        waterMask = 0 << 4;

        //Get dist to the ground to check if player is grounded
        distToGround = GetComponent<Collider>().bounds.extents.y;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (IsGrounded())
            {
                //moveHorizontal = Input.GetAxis("Horizontal");
                //moveVertical = Input.GetAxis("Vertical");

                moveHorizontal = joystick.Horizontal();
                moveVertical = joystick.Vertical(); 
            }

            if(Input.GetKeyDown(KeyCode.Space))
                Jump();
        }

        //Keep the velocity so it doesn't change during jump but only do that when the player is grounded to prevent jaggy edge falling, reset the friction
        if (IsGrounded())
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
        if (rb.velocity.y < 0 && !IsGrounded())
        {
            //Its vector3.up because gravity is -9.81
            rb.AddForce(Vector3.up * Physics.gravity.y * jumpMultiplier * Time.deltaTime, ForceMode.Acceleration);
        }

        //Keep the velocity so it doesn't change during jump FUNC
        if (!IsGrounded())
        {
            rb.velocity = new Vector3(keepVelocity.x / 0.8f, rb.velocity.y, keepVelocity.z / 0.8f);
        }

        //Prevent movement when respawning
        if (respawn && !IsGrounded())
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
            respawn = false;

        LimitSpeed();
        IncreaseDragWhenStill();
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

    void IncreaseDragWhenStill()
    {
        if (moveHorizontal == 0 && moveVertical == 0 && IsGrounded() && !DeathBehaviour.hasDied)
            rb.drag = 10.0f;
        else
            rb.drag = 1.0f;
    }

    void SetFriction(float value)
    {
        sphereCollider.material.dynamicFriction = value;
        sphereCollider.material.staticFriction = value;
    }

    void FixedUpdate()
    {
        //Position of CH
        Vector3 tempPos = cameraPos.position;
        tempPos.y = transform.position.y;
        GCV.position = tempPos;

        //Need this for some reason???
        GCH.LookAt(transform.position);
        GCV.LookAt(transform.position);

        //Moving the ball based on GC locations
        if (isAlive)
        {
            if (moveVertical > 0)
                rb.AddForce((transform.position - GCV.position).normalized * forwardVel);
            if (moveVertical < 0)
                rb.AddForce((transform.position - GCV.position).normalized * -forwardVel);
            if (moveHorizontal > 0)
                rb.AddForce((transform.position - GCH.position).normalized * forwardVel);
            if (moveHorizontal < 0)
                rb.AddForce((transform.position - GCH.position).normalized * -forwardVel);
        }
    }

    public void Jump()
    {
        if (IsGrounded() && isAlive)
        {
            rb.drag = 1.0f;

            //Keep the players velocity on X and Z but set in on the Y
            Vector3 temp = rb.velocity;
            temp.y = jumpHeight;
            rb.velocity = temp;

            hasJumped = true;
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