using UnityEngine;

public class DeathBehaviour : MonoBehaviour {

    GameObject panel;

    [HideInInspector]
    public static bool repeat;
    [HideInInspector]
    public static bool hasDied;

    int diedTimes;
    public static bool hasDiedByTrap;

    GameObject player;
    Rigidbody rb;
    Collider col;
    MeshRenderer playerMesh;
    ParticleSystem deathSmoke;

    void Start()
    {
        repeat = true;
        player = gameObject;
        playerMesh = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        deathSmoke = GameObject.Find("DeathParticles").GetComponent<ParticleSystem>();
        panel = GameObject.Find("DeathScreenPanel");
        col = GetComponent<Collider>();

        if (!PlayerPrefs.HasKey("DiedTimes"))
            PlayerPrefs.SetInt("DiedTimes", 0);

        diedTimes = PlayerPrefs.GetInt("DiedTimes");
    }

    void OnTriggerEnter(Collider collider)
    {
        //Don't do death actions if the player is already dead
        if (!hasDied)
        {
            StandardDeathBehaviour(collider);
            if (collider.tag == "Trap" || collider.tag == "Death")
            {
                TrapDeathBehaviour();
            }
        }
    }

    void TrapDeathBehaviour()
    {
        //Sets the position of particle system to players position (death pos)
        deathSmoke.transform.position = player.transform.position;
        //Disables player mesh so he is invisible during particle emission
        playerMesh.enabled = false;
        //Play the smoke particle emiddion
        deathSmoke.Play();
        //Set the movement speed of the player to 0 in update while dead
        hasDiedByTrap = true;

        //Next 2 are used for the squishing trap
        //Remove gravity
        rb.useGravity = false;
        //Disable collider
        col.enabled = false;
    }

    void StandardDeathBehaviour(Collider collider)
    {
        if (collider.gameObject.tag == "Water" || collider.gameObject.tag == "Trap" && repeat)
        {
            StartCoroutine(panel.GetComponent<Transition>().DoTransition());

            CameraMovement.cameraLocked = true;

            //STATS: Increase death count
            PlayerPrefs.SetInt("DiedTimes", ++diedTimes);

            repeat = false;
            hasDied = true;
            player.GetComponent<Movement>().isAlive = false;
        }
    }

    void Update()
    {
        if (Transition.alpha == 1.0f)
        {
            GetComponent<RespawnScript>().Respawn();
            Transition.alpha = 0.0f;
        }

        //If the player is dead, and if the player died by a trap (I don't want velocity = 0 when underwater)
        if(hasDiedByTrap && hasDied)
        {
            //Sets the player velocity to 0 so the camera is focused on the smoke and not on the player
            rb.velocity = Vector3.zero;
        }
    }
}