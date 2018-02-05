using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMovement : MonoBehaviour {

    [HideInInspector]
    public static bool movePlayer;

    [SerializeField]
    float speed;

    Vector3 higherPos;
    Vector3 lowerPos;
    Transform playerTransform;
    bool goDown;

    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        higherPos = transform.position;
        lowerPos = higherPos;
        lowerPos.y = higherPos.y - 4;
        goDown = true;
    }

    //This is called from simonSaysPuzzle when all the combinations are correct
    public void Move()
    {
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            if (goDown)
            {
                while (transform.position.y > lowerPos.y)
                {
                    transform.position = Vector3.MoveTowards(transform.position, lowerPos, speed * Time.deltaTime);

                    if(movePlayer)
                        playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y - speed * Time.deltaTime, playerTransform.position.z);

                    yield return null;
                }

                goDown = false;
            }
            else
            {
                while (transform.position.y < higherPos.y)
                {
                    transform.position = Vector3.MoveTowards(transform.position, higherPos, speed * Time.deltaTime);

                    if(movePlayer)
                        playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + speed * Time.deltaTime, playerTransform.position.z);

                    yield return null;
                }

                goDown = true;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }
}
