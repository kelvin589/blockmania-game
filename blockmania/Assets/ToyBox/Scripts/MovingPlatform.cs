using UnityEngine;
using System.Collections;

public class MovingPlatform : Photon.MonoBehaviour
{

    public bool upDown;

    public bool sideSide;

    private float timer;
    private int state = 0; //0 - move1, 1 - pause, 2- move2, 3 - pause2

    public float waitTime = 2;

    public float time = 2;

    public float speed = 2;

    private Vector3 correctPlatformPos;
    private Quaternion correctPlatformRot;

    void Awake()
    {
        correctPlatformPos = transform.position;
        correctPlatformRot = transform.rotation;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this platform: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if (stream.isReading)
        {
            // Network platform, receive data
            correctPlatformPos = (Vector3)stream.ReceiveNext();
            correctPlatformRot = (Quaternion)stream.ReceiveNext();
        }
    }

    // Use this for initialization
    void Start()
    {
        state = 0;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.inRoom && !photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, correctPlatformPos, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Slerp(transform.rotation, correctPlatformRot, Time.deltaTime * 10f);
            return;
        }

        if (sideSide == true)
        {
            if (state == 0)
            {
                timer += Time.deltaTime;
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                if (timer >= time)
                {
                    timer = 0;
                    state = 1;
                }
            }
            if (state == 1)
            {
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    timer = 0;
                    state = 2;
                }
            }
            if (state == 2)
            {
                timer += Time.deltaTime;
                transform.Translate(Vector3.forward * Time.deltaTime * -speed);
                if (timer >= time)
                {
                    timer = 0;
                    state = 3;
                }
            }
            if (state == 3)
            {
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    timer = 0;
                    state = 0;
                }
            }
        }

        if (upDown == true)
        {
            if (state == 0)
            {
                timer += Time.deltaTime;
                transform.Translate(Vector3.up * Time.deltaTime * speed);
                if (timer >= time)
                {
                    timer = 0;
                    state = 1;
                }
            }
            if (state == 1)
            {
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    timer = 0;
                    state = 2;
                }
            }
            if (state == 2)
            {
                timer += Time.deltaTime;
                transform.Translate(Vector3.up * Time.deltaTime * -speed);
                if (timer >= time)
                {
                    timer = 0;
                    state = 3;
                }
            }
            if (state == 3)
            {
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    timer = 0;
                    state = 0;
                }
            }
        }

    }
}

/*
Original script from https://assetstore.unity.com/packages/3d/environments/block-world-68107

using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public bool upDown;

	public bool sideSide;

	private float timer;
	private int state = 0; //0 - move1, 1 - pause, 2- move2, 3 - pause2

	public float waitTime = 2;

	public float time = 2;

	public float speed = 2;

	// Use this for initialization
	void Start () {
		state = 0;
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if (sideSide == true) {
			if (state == 0) {
				timer += Time.deltaTime;
				transform.Translate (Vector3.forward * Time.deltaTime * speed);
				if (timer >= time) {
					timer = 0;
					state = 1;
				}
			}
			if (state == 1) {
				timer += Time.deltaTime;
				if (timer >= waitTime) {
					timer = 0;
					state = 2;
				}
			}
			if (state == 2) {
				timer += Time.deltaTime;
				transform.Translate (Vector3.forward * Time.deltaTime * -speed);
				if (timer >= time) {
					timer = 0;
					state = 3;
				}
			}
			if (state == 3) {
				timer += Time.deltaTime;
				if (timer >= waitTime) {
					timer = 0;
					state = 0;
				}
			}
		} 

		if (upDown == true) {
				if (state == 0) {
					timer += Time.deltaTime;
					transform.Translate (Vector3.up * Time.deltaTime * speed);
					if (timer >= time) {
						timer = 0;
						state = 1;
					}
				}
				if (state == 1) {
					timer += Time.deltaTime;
					if (timer >= waitTime) {
						timer = 0;
						state = 2;
					}
				}
				if (state == 2) {
					timer += Time.deltaTime;
					transform.Translate (Vector3.up * Time.deltaTime * -speed);
					if (timer >= time) {
						timer = 0;
						state = 3;
					}
				}
				if (state == 3) {
					timer += Time.deltaTime;
					if (timer >= waitTime) {
						timer = 0;
						state = 0;
					}
				}
			}

	}
}
*/
