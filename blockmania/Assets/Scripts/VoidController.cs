using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidController : MonoBehaviour
{
    private HeartUIManager heartUIManager;
    private GameObject respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        heartUIManager = GameObject.FindWithTag("HeartUIManager").GetComponent<HeartUIManager>();
        respawnPoint = GameObject.FindWithTag("RespawnPoint");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log($"RespawnPoint(X: {respawnPoint.transform.position.x}, Y: {respawnPoint.transform.position.y}, Z: {respawnPoint.transform.position.z})");
            Debug.Log($"Player(X: {other.transform.position.x}, Y: {other.transform.position.y}, Z: {other.transform.position.z})");

            heartUIManager.RemoveLife();
            // https://answers.unity.com/questions/1614287/teleporting-character-issue-with-transformposition.html
            // In-built FPSController has CharacterController which has its own internal sense of position and velocity
            CharacterController characterController = other.gameObject.GetComponent<CharacterController>();
            characterController.enabled = false;
            other.gameObject.transform.position = respawnPoint.transform.position;
            characterController.enabled = true;
        }
    }
}
