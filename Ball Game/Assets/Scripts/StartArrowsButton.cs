using UnityEngine;

public class StartArrowsButton : MonoBehaviour {

    Animator anim;
    [HideInInspector]
    public static Collider boxCollider;

    int downHash = Animator.StringToHash("bones|button down");

    void Start()
    {
        //anim = GameObject.Find("dugmefbx").GetComponent<Animator>();
        anim = gameObject.transform.parent.parent.GetComponent<Animator>();
        boxCollider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Start timer
            CountdownTimer.start = true;

            //Generate new combinations
            PressurePlate.newCombination = true;
            TileBoardScript.newCombination = true;
            TileBoardScript.buttonWasPressed = true;

            //Play animations
            anim.Play(downHash);

            //Disable Trigger
            boxCollider.enabled = false;
        }
    }
}
