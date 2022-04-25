using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        // Only take heart if the other player is mine
        if (PhotonNetwork.inRoom && !other.gameObject.GetComponent<PhotonView>().isMine) return;
        if (other.tag == "Player")
        {
            print("Attack player");
            print(other.transform);
            HeartUIManager.Instance.RemoveLife();
            //audioSource.Play();
        }
    }
}
