using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyPiezas : MonoBehaviour
{
    float tiempo = 0;
    float tiempoMaximo;
    // Start is called before the first frame update
    void Start()
    {
        tiempoMaximo = Random.Range(7.0f,10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;
        if(tiempo >= tiempoMaximo){
            if(transform.localScale.x <= 0.1f){
                Destroy(gameObject);
            }
            else{
                // Hacer pequeño poco a poco
                transform.localScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f, transform.localScale.z - 0.1f);
            }
        }

    }
}
