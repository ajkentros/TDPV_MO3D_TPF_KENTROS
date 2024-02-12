using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float verticalForce;                   // referencia a la velocidad vertical
    [SerializeField] private float gravityValue;                    // referencia a la gravedad del juego
    [SerializeField] private float timerestartDelay = 0.5f;         // tiempo = 1 sg de retraso en reiniciar la escena
    [SerializeField] private GameObject torus;                      // referencia al torus
    [SerializeField] private ParticleSystem playerParticles;        // referencia a las partículas del player cuando colisiona
    [SerializeField] private ParticleSystem playerParticlesShump;   // referencia a las partículas del player cuando salta
    [SerializeField] private GameManager gameManager;               // referencia al GameManager

    private ParticleSystem playerParticlesShumpInstance;            // referencia a la instancia actual del sistema de partículas de salto

    private Color[] colorPlayer;                    // referencia a los colores del player 
    private Color currentColorPlayer;               // referencia al color del player actual
    private Renderer playerRenderer;                // referencia del Renderer del player para cambiar el material
    private List<int> detectedTorusIDs;             // referencia a la lista de torus en escena
    private bool hasJumpSoundPlayed = false;        // referencia para saber si ya se reprodujo el sonido de salto

    Rigidbody playerRigidBody;      // referencia al rigidbody del player
   



    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();        // obtiene el rigidbody del player
        playerRenderer = GetComponent<Renderer>();          // obtener el Renderer del player
        Physics.gravity = new Vector3(0, gravityValue, 0);  // cambiar la gravedad
        detectedTorusIDs = new List<int>();

        InitializeColorPlayer();     // llama a la función para inicializar el arreglo de colores

        ChangeColorPlayer();        // selecciona un color inicial al player
    }

    // Update is called once per frame
    void Update()
    {
        /*  si clic en la tecla space => llama al método Shump()
         *  sino => restablece la variable de control de reproducción de sonido en el próximo salto    
         */
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shump();

        }
        else
        {
            hasJumpSoundPlayed = false;
        }
    }

    // Gestiona el salto del player
    private void Shump()
    {
        /*  frena al player
         *  define un vector fuerza con los valores deseados de movimiento
         *  aplica la fuerza definida al player
         *  si la variable de salto = true =>
         *      si la instancia de partículas no es nula =>
         *          destruye la instancia anterior de las partículas de salto si existe
         *          si el prefab de partículas no es nulo =>
         *              instancia el prefabd e partículas en la escena y jerarquía
         *              reproduce el sonido de salto
         *              el sonido de salto se ha reproducido en este salto, cambia l avariable hasJumpSoundPlayed = true
         */
        playerRigidBody.velocity = Vector3.zero;
        Vector3 vectorForce = new(0, verticalForce, 0);
        playerRigidBody.AddForce(vectorForce);

        if (!hasJumpSoundPlayed)
        {
            if (playerParticlesShumpInstance != null)
            {
                Destroy(playerParticlesShumpInstance.gameObject);
            }

            if (playerParticlesShump != null)
            {
                playerParticlesShumpInstance = Instantiate(playerParticlesShump, transform.position, Quaternion.identity);
                playerParticlesShumpInstance.Play();
            }

            hasJumpSoundPlayed = true;
        }
    }

    // Gestiona los colores que puede tomar el player
    void InitializeColorPlayer()
    {
        /* si no hay torus no está asignado => mensaje
         * inicializa el arreglo de colores
         * verifica si el hijo tiene un MeshRenderer
         * si el children no es nulo => accede al material compartido en lugar del material directo
         * sino tiene MeshRenderer => asigna un color por defecto (blanco)
        */
        if (torus == null)
        {
            Debug.LogError("No se ha asignado ningún torus al PlayerControl");
            return;
        }

        colorPlayer = new Color[torus.transform.childCount];

        for (int i = 0; i < torus.transform.childCount; i++)
        {
            Transform child = torus.transform.GetChild(i);

            
            if (child.TryGetComponent<MeshRenderer>(out var childRenderer))
            {
                colorPlayer[i] = childRenderer.sharedMaterial.color;
            }
            else
            {
                colorPlayer[i] = Color.white;
            }
        }
    }

    // Gestiona el cambio de color del player
    private void ChangeColorPlayer()
    {
        /*  obtiene un núemro aleatorio entre 0 y la longuitud del arreglo
         *  asigna a la variable color actual del player con el color del arreglo de colores
         *  cambia el color del material del player
         */

        int randomNumber = Random.Range(0, colorPlayer.Length);
        currentColorPlayer = colorPlayer[randomNumber];
        playerRenderer.material.color = currentColorPlayer;


    }
    //Gestiona las colisiones
    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("Colisionó con " + collision.gameObject.tag);

        /* gestiona colisión con el PlayerColorChanger
         * gestiona colisión con el PlataformEnd
         * gestiona colisión con el torus
         */

        CollidePlayerColorChanger(collision);

        ColliderPlataformEnd(collision);

        ColliderTorus(collision);
        

    }

    // Gestiona la colision del player con el PlayerColorChanger
    private void CollidePlayerColorChanger(Collider collision)
    {
        //si el objeto colisionado es PlayerColorChanger => cambio color del player + destruye el PlayerColorChanger y retorna
        
        if (collision.gameObject.CompareTag("PlayerColorChanger"))
        {
            ChangeColorPlayer();
            Destroy(collision.gameObject);
            return;
        }
    }

    // Gestiona el cambio de escena
    private void ColliderPlataformEnd(Collider collision)
    {
        // si el objeto colisionado es PlataformEnd => desactiva e player + instancia el sistema de partìculas + carga siguiente escena 
 
        if (collision.gameObject.CompareTag("PlataformEnd"))
        {
            gameManager.AddScore(GetTotalTorusPoints());
            gameObject.SetActive(false);
            Instantiate(playerParticles, transform.position, Quaternion.identity);
            Invoke(nameof(LoadNextScene), timerestartDelay + .5f);
            return;
        }
    }

    // Gestiona la colisión con el torus
    private void ColliderTorus(Collider collision)
    {

        /*  si la colisión es con un objeto con el tag "Torus" =>
         *      asigna al hijo del torus que ha colisionado con player el MeshRenderer
         *      si el el renderer del hijo del torus no es nulo =>
         *          obtiene el color del hijo del torus con el que se ha colisionado
         *          si el color del jugador coincide con el color del hijo del torus => 
         *              desactiva el Player
         *              instancia el efecto de partículas
         *              reinicia la escena
         */

        if (collision.gameObject.CompareTag("Torus"))
        {
            MeshRenderer torusChildRenderer = collision.GetComponentInChildren<MeshRenderer>();

            if (torusChildRenderer != null)
            {
                Color torusChildColor = torusChildRenderer.sharedMaterial.color;

                if (currentColorPlayer != torusChildColor)
                {
                    gameManager.DecreaseScore(GetTorusPoints());
                    gameObject.SetActive(false);
                    Instantiate(playerParticles, transform.position, Quaternion.identity);
                    Invoke(nameof(RestarScene), timerestartDelay);
                }
            }
        }

    }

    // Gestiona la carga de la siguiente escena
    private void LoadNextScene()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        gameManager.LoadScene(activeSceneIndex);
    }

    // Gestiona la carga de la escena actual
    private void RestarScene()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        gameManager.LoadScene(activeSceneIndex);
    }

    // Obtener puntos del toroide
    private int GetTorusPoints()
    {
        if (torus != null)
        {
            if (torus.TryGetComponent<TorusRotation>(out var torusRotation))
            {
                return torusRotation.GetPointsTorus();
            }
        }
        return 0;
    }

    // Obtener puntos de todos los torus en la escena
    private int GetTotalTorusPoints()
    {
        /*
         * inicializa la variable totalPoints = 0
         * define una lista torusObject de GameObject con el tag "Torus" en la escena
         * por cada torusObject encontrado =>
         *  obtiene el componente TorusRotation del GameObject actual
         *  si se encuentra el componente TorusRotation no nulo =>
         *      sumar sus puntos a totalPoints
         *  retorna los puntos      
         */

        int totalPoints = 0;
        
        GameObject[] torusObjects = GameObject.FindGameObjectsWithTag("Torus");

        foreach (GameObject torusObject in torusObjects)
        {
            
            if (torusObject.TryGetComponent<TorusRotation>(out var torusRotation))
            {
                totalPoints += torusRotation.GetPointsTorus();
            }
        }

        return totalPoints;
    }
}
