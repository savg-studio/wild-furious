using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private const string ALTERNATIVE_AUDIO_FILE = "https://savgs.xoanweb.com/wild-furious/demo/main_menu_song.ogg";

    private static MusicController instance;

    [SerializeField] private AudioSource audioSource = null;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void Play()
    {
        if (audioSource != null)
        {
            if (audioSource.clip != null)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                Debug.Log("No clip");
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Stop()
    {
        if (audioSource != null && audioSource.isPlaying) audioSource.Stop();
        Destroy(gameObject);
    }
}
