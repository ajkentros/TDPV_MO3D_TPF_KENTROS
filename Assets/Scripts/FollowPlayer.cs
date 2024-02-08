using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;


    private Vector3 offsetCamera;

    private void Start()
    {
        // Calcula el offset inicial entre la c�mara y el jugador
        offsetCamera = transform.position - playerTransform.position;
    }

    void Update()
    {
        // Calcula la nueva posici�n de la c�mara
        Vector3 targetPosition = playerTransform.position + offsetCamera;

        // Aplica la nueva posici�n solo si el jugador est� por encima de la c�mara
        if (playerTransform.position.y > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, targetPosition.y, transform.position.z);
        }
        else
        {
            // Aplica la nueva posici�n incluso si el jugador est� por debajo de la c�mara
            transform.position = new Vector3(transform.position.x, targetPosition.y, transform.position.z);
        }
    }
}
