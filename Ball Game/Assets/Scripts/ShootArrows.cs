using UnityEngine;

public class ShootArrows : MonoBehaviour {

    GameObject arrow;

    //This variable is being changed from PressurePlateTile
    [HideInInspector]
    public static bool shoot = false;

    float speed = 15;
    float delay = 0.2f;
    float timestamp;

    // Use this for initialization
    void Awake ()
    {
        arrow = GameObject.Find("Arrow");
	}

    void SpawnArrow()
    {
            foreach (Transform arrowSpawn in transform)
            {
                //Instantiate the arrow
                Rigidbody instance = Instantiate(arrow.GetComponent<Rigidbody>());
                //Set its position to the empty's position
                instance.gameObject.transform.position = arrowSpawn.position;
                //Set it's velocity
                instance.velocity = Vector3.right * speed;

                //Parent all the arrows to one empty
                instance.gameObject.transform.SetParent(GameObject.Find("Arrows").GetComponent<Transform>());
            }
    }

    void Update()
    {
        if(shoot)
        {
            //Shoot arrows with delay
            if (timestamp < Time.time)
            {
                SpawnArrow();
                timestamp = Time.time + delay;
            }

            //Reset the timer
            CountdownTimer.resetTime = true;
        }
    }
}