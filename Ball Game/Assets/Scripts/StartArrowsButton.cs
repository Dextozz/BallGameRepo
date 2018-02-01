using UnityEngine;

public class StartArrowsButton : MonoBehaviour {

    Animator anim;
    //This is public static because another script is probably turning it off
    [HideInInspector]
    public static Collider boxCollider;

    int downHash = Animator.StringToHash("bones|button down");

    void Start()
    {
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
