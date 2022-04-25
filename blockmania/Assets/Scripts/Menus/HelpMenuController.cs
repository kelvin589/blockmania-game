using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void Awake()
    {
        // Reappear cursor due to FPSController Behaviour
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ButtonHandlerBack()
    {
        StartCoroutine(SceneHelper.LoadGameSceneAsync(ScenesEnum.MainMenu));
    }
}
