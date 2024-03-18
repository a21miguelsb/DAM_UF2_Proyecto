using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioClip AudioClip;
    private AudioSource AudioSource;
    public static GameManager Instance;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();

    }
    private void Update()
    {
        if (!AudioSource.isPlaying)
        {
            AudioSource.clip = AudioClip;
            AudioSource.Play();
            AudioSource.volume = 0.3f;
        }
        
    }
    public void Awake()
    {
        if(GameManager.Instance == null)
        {
            GameManager.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
