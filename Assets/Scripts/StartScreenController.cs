using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStartScreen : MonoBehaviour
{
    [SerializeField] private GameObject UIMenu;
    [SerializeField] private GameObject UICredits;
    [SerializeField] private GameObject UIHelp;

    void Start()
    {
        // gestiona la activación de UIMenu y desactivación de UICredits y UIHelp
        UIMenu.SetActive(true);
        UICredits.SetActive(false);
        UIHelp.SetActive(false);
    }

    // Gestiona clic en StartGameButton
    public void StartGame()
    {
        // activa la escena 1
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex + 1);
    }

    // Gestiona clic en GameCreditButton
    public void ShowCredits()
    {
        // activa menú de créditos
        UIMenu.SetActive(false);
        UICredits.SetActive(true);
        UIHelp.SetActive(false);
    }

    // Gestiona clic en HelpButton
    public void ShowHelp()
    {
        // activa menú de ayuda
        UIMenu.SetActive(false);
        UICredits.SetActive(false);
        UIHelp.SetActive(true);
    }

    // gestiona clic en boton de Return
    public void ReturnToMenu()
    {
        // activa el retorno
        UIMenu.SetActive(true);
        UICredits.SetActive(false);
        UIHelp.SetActive(false);
    }
}
