using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;        // referencia Singleton a AudioManager

    public List<AudioClip> audioClips;          // referencia a los audios a utilizar en el juego
                                                                 
    private AudioSource audioSource;            // referencia a la fuente de audio

    private void Awake()
    {
        // Garantiza que solo haya una instancia del AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Instancia audioSource y habilita el loop de Source
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.loop = true;
    }

    // Reproduce audioSource
    public void PlayMusic(AudioClip music)
    {
        audioSource.clip = music;
        audioSource.Play();
    }

    // Detiene la reproducción del audio
    public void StopMusic()
    {
        audioSource.Stop();
    }
}
