using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMovement : MonoBehaviour {

    [SerializeField]
    float speed;

    Vector3 higherPos;
    Vector3 lowerPos;
    bool goDown;

    void Start()
    {
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
                    yield return null;
                }

                goDown = false;
            }
            else
            {
                while (transform.position.y < higherPos.y)
                {
                    transform.position = Vector3.MoveTowards(transform.position, higherPos, speed * Time.deltaTime);
                    yield return null;
                }

                goDown = true;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }
}
