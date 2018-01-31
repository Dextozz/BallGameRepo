using System;
using UnityEngine;

public class FallingTrapBehaviour : MonoBehaviour {

	Animator anim;
	AnimatorStateInfo asi;
	CapsuleCollider col;
	Collider deathCol;

	public float startTime;

	float tempHeight;
	float tempCenterY;
	float timer;
	int dropHash = Animator.StringToHash("Armature|SpikeDrop");
	int riseHash = Animator.StringToHash("Armature|SpikeRise");
	bool playDrop;
	bool playRise;
	bool startTimer;

	// Use this for initialization
	void Awake ()
	{
		anim = transform.parent.parent.parent.parent.GetComponent<Animator>();
		col = GetComponent<CapsuleCollider>();
		deathCol = transform.parent.GetComponent<BoxCollider>();
	}

	void Start()
	{
		tempHeight = col.height;
		tempCenterY = col.center.y;

		startTimer = false;
		Invoke("playDropAnimation", startTime);
	}

	// Update is called once per frame
	void Update ()
	{
		asi = anim.GetCurrentAnimatorStateInfo(0);
		if(startTimer)
			timer += Time.deltaTime;

		if (playDrop && timer > asi.length * (1 + (1 - asi.speed)))
		{
			deathCol.enabled = true;
			playDropAnimation();
		}
		else if(playRise && timer > asi.length * (1 + (1 - asi.speed)))
		{
			deathCol.enabled = false;
			playRiseAnimation();
		}

		if (asi.IsName("Armature|SpikeDrop"))
		{
			//Set the height of the collider
			col.height = tempHeight * asi.normalizedTime;
			//Set the center
			col.center = new Vector3(0, tempCenterY * asi.normalizedTime, 0);
		}
		else if (asi.IsName("Armature|SpikeRise"))
		{
			//Scale down collider
			col.height = tempHeight * (1 - asi.normalizedTime);
			//Set the center
			col.center = new Vector3(0, tempCenterY * (1 - asi.normalizedTime), 0);
		}
	}

	void playDropAnimation()
	{
		anim.Play(dropHash);
		playRise = true;
		playDrop = false;
		timer = 0;
		startTimer = true;
	}

	void playRiseAnimation()
	{
		anim.Play(riseHash);
		playDrop = true;
		playRise = false;
		timer = 0;
		startTimer = true;
	}

	void test()
	{
		Debug.Log("wtf");
	}
}