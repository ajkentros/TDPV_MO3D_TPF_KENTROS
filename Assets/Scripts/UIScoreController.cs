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

    private Camera mainCamera;                                      // referencia a la cámara paraa obtener propiedades
    private TorusRotation currentTorus;                             // Torus actual más cercano a la cámara
    private readonly int endScene = 0;                              // referencia del número de la escena final

    void Start()
    {
        // verifica si el componente TextMeshProUGUI está asignado
        // obtiene el número de la escena actual
        // asigna el número de la escena en el TextMeshProUGUI sceneLevelNumber
        // obtiene una instancia de la cámara principal

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
         *      crea un vector3 screenPoint = con la posición del torus en el mundo, se convierte la posición de mundo (torus.transform.position) a coordenadas de pantalla con Camera.WorldToScreenPoint (pixeles)
         *      si el torus está dentro de los límites de la pantalla =>
         *          verifica si las coordenadas x y y del punto de la pantalla están dentro de los límites de la pantalla (entre 0 y el ancho/altura de la pantalla) y si la coordenada > 0, el objeto está delante de la cámara
         *          calcula la distancia entre la cámara y el torus y se guarda en la variable distance
         *          si el torus está más cerca que el anterior más cercano => 
         *              actualiza el torus actual más cercano
         */
        foreach (var torus in allTorusObjects)
        {
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(torus.transform.position);

            // Verifica si el torus está dentro de la pantalla
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

        // muestra los parámetros del torus más cercano
        if (currentTorus != null)
        {
            /*
             * muestra en UIParametersTorus: velocidad ; escala ; puntos asignados a ese torus en particular
             */
            velocityNumberTorus.text = currentTorus.GetRotateTorus().ToString();
            scaleNumberTorus.text = currentTorus.GetScaleTorus().ToString();
            pointsNumberTorus.text = currentTorus.GetPointsTorus().ToString();

            // ajusta y muestra la posición en el eje Y del panel para que coincida con la posición del Torus en la pantalla
            // el panel del torus queda fija en la posición del torus, y el torus se mueve con el desplazamiento de la cámara
            Vector3 torusScreenPosition = mainCamera.WorldToScreenPoint(currentTorus.transform.position);
            Vector3 newPositionPanelParametersTorus = panelParametersTorus.transform.position;
            newPositionPanelParametersTorus.y = torusScreenPosition.y;
            panelParametersTorus.transform.position = newPositionPanelParametersTorus;
        }
    }

    // Gestiona el botón Return
    public void ReturnToMenu()
    {
        // clic en ReturnButton: pone el score = 0 y carga la esena = 0 (menú inicial)
        //Debug.Log("menu");
        gameManager.ResetScore();
        gameManager.LoadScene(endScene);

    }

}
