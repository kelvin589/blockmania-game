using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

    public void ButtonHandlerPlay()
    {
        StartCoroutine(PlaySoundAndLoadScene(ScenesEnum.LevelSelector));
    }

    public void ButtonHandlerSettings()
    {
        StartCoroutine(PlaySoundAndLoadScene(ScenesEnum.SettingsMenu));
    }

    public void ButtonHandlerHelp()
    {
        StartCoroutine(PlaySoundAndLoadScene(ScenesEnum.HelpMenu));
    }

    IEnumerator PlaySoundAndLoadScene(ScenesEnum scene) 
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        StartCoroutine(SceneHelper.LoadGameSceneAsync(scene));
    }

    public void ButtonHandlerQuit()
    {
        Application.Quit();
    }
}
