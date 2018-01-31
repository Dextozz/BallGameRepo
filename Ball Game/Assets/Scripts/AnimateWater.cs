using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateWater : MonoBehaviour {

	Renderer rend;

	public float scrollSpeed = -0.3f;

	// Use this for initialization
	void Start ()
	{
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		//Multiply by Time.time because offset is a number that is infinite and time.time is a number that is constantly increasing to infinity. 
		//I could also have done it by just creating a variable and increasing it iterativly
		float offset = Time.time * scrollSpeed;
		//Apply the offset to detail albedo. You can find the string by editing the material
		rend.material.SetTextureOffset("_DetailAlbedoMap", new Vector2(offset, 0));
	}
}
