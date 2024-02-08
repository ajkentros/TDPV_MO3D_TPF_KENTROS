using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;



public class UIEndScreenController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI scoreTotal;      // referencia al texto del score

    private readonly int endScene = 0;
    public void ReturnToMenu()
    {
        // Este método se llama cuando se hace clic en ReturnButton y pone el score = 0
        
        gameManager.ResetScore();
        gameManager.LoadScene(endScene);

    }

    private void Update()
    {
        scoreTotal.text = gameManager.Score().ToString();
    }
}