using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;


    private Vector3 offsetCamera;

    private void Start()
    {
        // Calcula el offset inicial entre la cámara y el jugador
        offsetCamera = transform.position - playerTransform.position;
    }

    void Update()
    {
        // Calcula la nueva posición de la cámara
        Vector3 targetPosition = playerTransform.position + offsetCamera;

        // Aplica la nueva posición solo si el jugador está por encima de la cámara
        if (playerTransform.position.y > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, targetPosition.y, transform.position.z);
        }
        else
        {
            // Aplica la nueva posición incluso si el jugador está por debajo de la cámara
            transform.position = new Vector3(transform.position.x, targetPosition.y, transform.position.z);
        }
    }
}
