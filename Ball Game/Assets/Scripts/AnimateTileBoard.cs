using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTileBoard : MonoBehaviour {

	public float duration;
	public float increment;
	public int counts;

	//up = true, down = false;
	bool upDown;

	int counter;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(Animate());
	}

	IEnumerator Animate()
	{
		while (true)
		{
			if (upDown)
			{
				for (int i = 0; i < counts; i++)
				{
					transform.position = new Vector3(transform.position.x, transform.position.y - increment, transform.position.z);
					yield return new WaitForSeconds(duration);
				}

				upDown = false;
			}
			else
			{
				for (int i = 0; i < counts; i++)
				{
					transform.position = new Vector3(transform.position.x, transform.position.y + increment, transform.position.z);
					yield return new WaitForSeconds(duration);
				}

				upDown = true;
			}
		}

		//Total duration of animation is duration * counts
	}
}
