using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2 : MonoBehaviour {

    [SerializeField]
    float moveSpeed;

    Transform GCV;
    Transform GCH;

    float horizontalInput;
    float verticalInput;

	// Use this for initialization
	void Start () {
        GCH = GameObject.Find("GCH").GetComponent<Transform>();
        GCV = GameObject.Find("GCV").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
