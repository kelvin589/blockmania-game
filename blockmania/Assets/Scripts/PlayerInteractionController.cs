using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInteractionController : MonoBehaviour
{
    public Image crosshairDefault;
    public Image crosshairSelected;
    public Text crosshairText;

    void Awake()
    {
        ToggleSelectedCursor(false);
    }

    public void ToggleSelectedCursor(bool showSelectedCursor)
    {
        crosshairDefault.enabled = !showSelectedCursor;
        crosshairSelected.enabled = showSelectedCursor;
        crosshairText.enabled = showSelectedCursor;
    }

    void Update()
    {
        CollectRaycast();
    }

    // Collect item
    void CollectRaycast()
    {
        Vector3 centreOfScreen = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        float distanceToFireRay = 3;
        Ray centreOfScreenRay = Camera.main.ScreenPointToRay(centreOfScreen);
        RaycastHit hit;
        if (Physics.Raycast(centreOfScreenRay, out hit, distanceToFireRay))
        {
            InteractiveObjectBase interactiveObjectBase = hit.transform.gameObject.GetComponent<InteractiveObjectBase>();
            ToggleSelectedCursor(interactiveObjectBase != null);
            if (interactiveObjectBase != null && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Raycast hit: " + hit.transform.name);
                interactiveObjectBase.OnInteraction();
            }
        }
        else
        {
            ToggleSelectedCursor(false);
        }
    }

    // Move with platform
    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.tag == "Platform")
    //     {
    //         transform.SetParent(other.transform, true);
    //     }
    // }

    // void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.tag == "Platform")
    //     {
    //         transform.parent = null;
    //     }
    // }
}
