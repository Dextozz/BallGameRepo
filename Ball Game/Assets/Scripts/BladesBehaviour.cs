using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladesBehaviour : MonoBehaviour {

    public bool rotateLeft;
    public bool rotateRight;

	// Update is called once per frame
	void Update ()
    {
        Rotate();
    }

    void Rotate()
    {
        if (rotateLeft)
            transform.Rotate(Vector3.forward, 90.0f * Time.deltaTime);
        else if (rotateRight)
            transform.Rotate(Vector3.forward, -90.0f * Time.deltaTime);
    }
}
