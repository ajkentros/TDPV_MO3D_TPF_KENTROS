using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class UIStartScreenController : MonoBehaviour
{
    public GameObject UIMenu;
    public GameObject UICredits;
    public GameObject UIHelp;

    [SerializeField] private GameManager gameManager;

    void Start()
    {
        // Al inicio, solo mostramos el men� principal
        UIMenu.SetActive(true);
        UICredits.SetActive(false);
        UIHelp.SetActive(false);
    }

    // Activa y gestiona el inicio del juego
    public void StartGame()
    {
        // Cuando se hace clic en StartGameButton: resetea el scrore + inicia la escena 1 = Level1
        gameManager.ResetScore();
        int sceneNumber = SceneManager.GetActiveScene().buildIndex + 1;
        gameManager.LoadScene(sceneNumber);
    }

    // Activa y gestiona la pantalla de cr�ditos
    public void ShowCredits()
    {
        // Cuando se hace clic en GameCreditButton: se muestran los cr�ditos
        UIMenu.SetActive(false);
        UICredits.SetActive(true);
        UIHelp.SetActive(false);
    }

    // Activa y gestiona la pantalla de Help
    public void ShowHelp()
    {
        // Cuando se hace clic en HelpButton: se muestra la ayuda
        UIMenu.SetActive(false);
        UICredits.SetActive(false);
        UIHelp.SetActive(true);
    }
    
    // Gestiona el bot�n Return
    public void ReturnToMenu()
    {
        // Cuando se hace clic en ReturnButton: resetea el scrore + inicia la escena 0 = Men� de inicio del juego
        gameManager.ResetScore();
        int sceneNumber = 0;
        gameManager.LoadScene(sceneNumber);
    }
}