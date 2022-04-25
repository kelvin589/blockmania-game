using UnityEngine;
using System.Collections;

public class AniActivate : MonoBehaviour {

	[Tooltip("Press E within range to activate Animation")]
	public bool hasBeenHit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// OnTriggerStay rather than OnTriggerEnter to detect input every frame after entering
	void OnTriggerStay(Collider other)
	{
		if (hasBeenHit == false) {
			if (other.tag == "Player" && Input.GetKey(KeyCode.E)) {
				GetComponent<Animator> ().enabled = true;
				hasBeenHit = true;
			}
		}
	}
}

/*
Original script from https://assetstore.unity.com/packages/3d/environments/block-world-68107

using UnityEngine;
using System.Collections;

public class AniActivate : MonoBehaviour {

	[Tooltip("Press E within range to activate Animation")]
	public bool hasBeenHit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (hasBeenHit == false) {
			if (other.tag == "Player") {
				GetComponent<Animator> ().enabled = true;
				hasBeenHit = true;
			}
		}
	}
}
*/
