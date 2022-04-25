using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObjectBase : Photon.MonoBehaviour
{
    public abstract void OnInteraction();
}
