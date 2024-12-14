using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotacionFruta : MonoBehaviour
{
    // Velocidad de rotación, configurable desde el Inspector
    public float velocidadRotacion = 50f;
    
    private float rotacionY = 0f;

    // Update is called once per frame
    void Update () {
        // Incrementamos el ángulo de rotación en el eje Y
        rotacionY += velocidadRotacion * Time.deltaTime;

        // Creamos un nuevo Quaternion que representa la rotación sobre el eje Y
        transform.rotation = Quaternion.Euler(0f, rotacionY, 0f);
    }
}