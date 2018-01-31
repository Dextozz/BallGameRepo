using UnityEngine;

public class MoveTile : MonoBehaviour {

    Animator anim;

    [HideInInspector]
    public static bool reset;

    int moveDownHash = Animator.StringToHash("Armature|MoveDown");
    int moveUpHash = Animator.StringToHash("Armature|MoveUp");

    Vector3 startingPos;

    void Start()
    {
        startingPos = gameObject.transform.position;
        anim = transform.parent.parent.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Stop the moveUp animation from replaying
            reset = false;

            playDown();

            if (transform.parent.parent.tag == "SafeTile")
            {
                //Play "gears moving and stuff falling into place sound"
            }
            else
            {
                //Play incorrect sound;
                ShootArrows.shoot = true;
            }

            //Simon says part
            gameObject.tag = "DoNotResetTile";
            SimonSaysPuzzle.detectPress = true;
        }
    }

    public void playUp()
    {
        anim.Play(moveUpHash);
    }

    public void playDown()
    {
        anim.Play(moveDownHash);

    }

    void OnTriggerExit(Collider other)
    {
        //This is used to determine if the player is still sitting on the field
        SimonSaysPuzzle.resetTile = true;

        gameObject.tag = "Untagged";
    }

    void Update()
    {
        whenToReset();

        if (reset && transform.localPosition != startingPos)
        {
            playUp();
            RespawnScript.resetTile = false;
        }
    }

    void whenToReset()
    {
        if (RespawnScript.resetTile)
        {
            reset = true;
            RespawnScript.resetTile = false;
        }
    }
}