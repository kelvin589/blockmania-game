using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedObject : MonoBehaviour
{
    [SerializeField]
    private LeverManager[] levers;
    [SerializeField]
    [Tooltip("The gameobject to show or hide when lever(s) are active or not")]
    private GameObject block;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocked()) {
            block.SetActive(true);
        } else {
            block.SetActive(false);
        }
    }

    bool IsLocked()
    {
        foreach (LeverManager lever in levers) {
            if (!lever.IsActive()) {
                return true;
            }
        }
        return false;
    }
}
