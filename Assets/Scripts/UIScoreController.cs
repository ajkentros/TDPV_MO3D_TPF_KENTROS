using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.UI;

public class UIScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sceneLevelNumber;      // referencia al texto del nivel
    [SerializeField] private TextMeshProUGUI sceneScoreNumber;      // referencia al texto del score
    [SerializeField] private TextMeshProUGUI velocityNumberTorus;   // referencia al texto de la velocidad del torus
    [SerializeField] private TextMeshProUGUI scaleNumberTorus;      // referencia al texto de la scala del torus
    [SerializeField] private TextMeshProUGUI pointsNumberTorus;     // referencia al texto de los puntos que da el torus
 
    [SerializeField] private GameManager gameManager;               // referencia al GameManager
    [SerializeField] private List<TorusRotation> allTorusObjects; // Lista de todos los torus en la escena
    [SerializeField] private GameObject panelParametersTorus;       // referencia al torus

    private Camera mainCamera;                                      // referencia a la c�mara paraa obtener propiedades
    private TorusRotation currentTorus;                             // Torus actual m�s cercano a la c�mara
    private readonly int endScene = 0;                              // referencia del n�mero de la escena final

    void Start()
    {
        // verifica si el componente TextMeshProUGUI est� asignado
        // obtiene el n�mero de la escena actual
        // asigna el n�mero de la escena en el TextMeshProUGUI sceneLevelNumber
        // obtiene una instancia de la c�mara principal

        if (sceneLevelNumber == null)
        {
            Debug.LogError("No se ha asignado TextMeshProUGUI en el UIController");
            return;
        }

        int sceneNumber = SceneManager.GetActiveScene().buildIndex;

        sceneLevelNumber.text = sceneNumber.ToString();

        mainCamera = Camera.main; 
    }

    private void Update()
    {
        // imprime en pantalla el scrore
        sceneScoreNumber.text = gameManager.Score().ToString();

        // inicializa closestDistance = infinito positivo (asegura que la primera distancia que se compara con otros valores sea > que cualquier otro valor)
        float closestDistance = Mathf.Infinity;

        /*  por cada torus en la lista de torus de la escena =>
         *      crea un vector3 screenPoint = con la posici�n del torus en el mundo, se convierte la posici�n de mundo (torus.transform.position) a coordenadas de pantalla con Camera.WorldToScreenPoint (pixeles)
         *      si el torus est� dentro de los l�mites de la pantalla =>
         *          verifica si las coordenadas x y y del punto de la pantalla est�n dentro de los l�mites de la pantalla (entre 0 y el ancho/altura de la pantalla) y si la coordenada > 0, el objeto est� delante de la c�mara
         *          calcula la distancia entre la c�mara y el torus y se guarda en la variable distance
         *          si el torus est� m�s cerca que el anterior m�s cercano => 
         *              actualiza el torus actual m�s cercano
         */
        foreach (var torus in allTorusObjects)
        {
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(torus.transform.position);

            // Verifica si el torus est� dentro de la pantalla
            if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < Screen.width && screenPoint.y > 0 && screenPoint.y < Screen.height)
            {
                float distance = Vector3.Distance(torus.transform.position, mainCamera.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    currentTorus = torus;
                }
            }
        }

        // muestra los par�metros del torus m�s cercano
        if (currentTorus != null)
        {
            /*
             * muestra en UIParametersTorus: velocidad ; escala ; puntos asignados a ese torus en particular
             */
            velocityNumberTorus.text = currentTorus.GetRotateTorus().ToString();
            scaleNumberTorus.text = currentTorus.GetScaleTorus().ToString();
            pointsNumberTorus.text = currentTorus.GetPointsTorus().ToString();

            // ajusta y muestra la posici�n en el eje Y del panel para que coincida con la posici�n del Torus en la pantalla
            // el panel del torus queda fija en la posici�n del torus, y el torus se mueve con el desplazamiento de la c�mara
            Vector3 torusScreenPosition = mainCamera.WorldToScreenPoint(currentTorus.transform.position);
            Vector3 newPositionPanelParametersTorus = panelParametersTorus.transform.position;
            newPositionPanelParametersTorus.y = torusScreenPosition.y;
            panelParametersTorus.transform.position = newPositionPanelParametersTorus;
        }
    }

    // Gestiona el bot�n Return
    public void ReturnToMenu()
    {
        // clic en ReturnButton: pone el score = 0 y carga la esena = 0 (men� inicial)
        //Debug.Log("menu");
        gameManager.ResetScore();
        gameManager.LoadScene(endScene);

    }

}
