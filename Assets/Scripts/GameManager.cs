using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;        // referencia Singleton al GameManager
    private int sceneNumber = 0;  // referencia a la variable que indica la escena de incio = menú
    private int score = 0;                       // referencia a la variable Score (puntos acumulados)
    private int indexAudio;                      // referencia a la pista de audio a reproducir



    private void Awake()
    {
        // Garantiza que solo haya una instancia del GameManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Carga la escena inicial (0) = menú
    private void Start()
    {
        LoadScene(sceneNumber);
    }
    
    // Carga una escena
    public void LoadScene(int _sceneNumber)
    {
        sceneNumber = _sceneNumber;
        UpdateAudioClips();
        SceneManager.LoadScene(sceneNumber); 
        
    }

    // Retorna el score acumulado
    public int Score()
    {
        // Carga una escena según el nombre proporcionado
        return score;
    }

    // Adiciona los puntos logrados al Score
    public void AddScore(int points)
    {
        score += points;
        indexAudio = 3;
        AudioManager.instance.PlayMusic(AudioManager.instance.audioClips[indexAudio]);
        
    }

    // Decrementa los puntos perdidos al Score
    public void DecreaseScore(int points)
    {
        score -= points;
        indexAudio = 2;
        AudioManager.instance.PlayMusic(AudioManager.instance.audioClips[indexAudio]);
    }

    // Restablece a cero el Score
    public void ResetScore()
    {
        score = 0;
    }

    private void UpdateAudioClips()
    {
        if(sceneNumber == 0)
        {
            indexAudio = 0;

        }else if (sceneNumber >=1 && sceneNumber <= 5)
        {
            indexAudio = 1;

        }else if(sceneNumber == 6)
        {
            indexAudio = 4;
        }
        AudioManager.instance.PlayMusic(AudioManager.instance.audioClips[indexAudio]);
    }
}
