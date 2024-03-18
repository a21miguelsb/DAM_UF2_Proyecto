using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] GameObject secureExitPanel;
    [SerializeField] AudioClip music;
    private AudioSource musicSource;

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.clip = music;
            musicSource.Play(); 
        }
    }
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Pantalla1");
    }

    public void SecureExit()
    {
        secureExitPanel.SetActive(true);
    }

    public void CancelExit()
    {
        secureExitPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
