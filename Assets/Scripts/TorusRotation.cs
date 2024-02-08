using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorusRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeedTorus = 100f;       // referencia a la velocida del rotaci�n del torus
    [SerializeField] private float initialScale = 1.2f;             // Escala inicial del toroide
    [SerializeField] private int points = 2;                        // Escala inicial del toroide

    // M�todo Start se llama antes del primer frame de actualizaci�n
    void Start()
    {
        // Asignar la velocidad de rotaci�n inicial
        RotateTorus(rotationSpeedTorus);

        // Inicializar la escala del toroide
        SetTorusScale(initialScale);
    }

    // Update is called once per frame
    void Update()
    {
        // Rotar el toroide
        RotateTorus(rotationSpeedTorus);

    }

    // Gestiona la rotaci�n del toroide
    private void RotateTorus(float rotationSpeedTorus)
    {
        // establece la rotaci�n del torus por frame
        Vector3 speed = new(rotationSpeedTorus * Time.deltaTime, 0, 0);
        transform.Rotate(speed);
    }

    // Gestiona la escala inicial del torus
    private void SetTorusScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public int GetPointsTorus()
    {
        return points;
    }

    public float GetScaleTorus()
    {
        return initialScale;
    }

    public float GetRotateTorus()
    {
        return rotationSpeedTorus;
    }
}

