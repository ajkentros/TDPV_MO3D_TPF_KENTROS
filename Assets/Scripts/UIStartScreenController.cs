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
        // Al inicio, solo mostramos el menú principal
        UIMenu.SetActive(true);
        UICredits.SetActive(false);
        UIHelp.SetActive(false);
    }

    public void StartGame()
    {
        // Cuando se hace clic en StartGameButton, activamos la escena 1
        gameManager.ResetScore();
        int sceneNumber = SceneManager.GetActiveScene().buildIndex + 1;
        gameManager.LoadScene(sceneNumber);
    }

    public void ShowCredits()
    {
        // Cuando se hace clic en GameCreditButton, mostramos los créditos
        UIMenu.SetActive(false);
        UICredits.SetActive(true);
        UIHelp.SetActive(false);
    }

    public void ShowHelp()
    {
        // Cuando se hace clic en HelpButton, mostramos la ayuda
        UIMenu.SetActive(false);
        UICredits.SetActive(false);
        UIHelp.SetActive(true);
    }

    public void ReturnToMenu()
    {
        gameManager.ResetScore();
        int sceneNumber = 0;
        gameManager.LoadScene(sceneNumber);
    }
}