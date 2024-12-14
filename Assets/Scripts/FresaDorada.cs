using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FresaDorada : MonoBehaviour
{
    // Start is called before the first frame update
    public float distancia = 10f;
    public Transform player;
    public AudioSource audioSource;
    public ParticleSystem explosion1;
    public ParticleSystem explosion2;


    private bool b = false;
    private float tiempo = 0;
    private GameObject gameManager;

    void Start()
    {
		gameManager = GameObject.Find("GameManager");
        player = GameObject.Find("Player").transform;
        transform.position = new Vector3(player.position.x + distancia, player.position.y+distancia, player.position.z);
        //bajar volumen
        audioSource.volume = 0.5f;
        explosion1.Stop();
        explosion2.Stop();
    }

    // Update is called once per frame
    void Update()
    {

        // Mover la fresa en circulos al rededor del player
        transform.RotateAround(player.position, Vector3.up, 50 * Time.deltaTime);
        // Cambiar la altura entre 0.5 y 1.5 de manera sinusoidal
        Vector3 newPosition = transform.position;
        newPosition.y = Mathf.Sin(Time.time)*0.5f+1f;
		if (GameObject.Find("GameManager").GetComponent<GameManager>() == null) {
			newPosition.y = newPosition.y + 2; 
		}
        transform.position = newPosition;

        if(b){
            if(tiempo >= 5f){
                Destroy(gameObject);
            }
            tiempo += Time.deltaTime;
        }
    }
     private void OnCollisionEnter(Collision other){
        if(other.gameObject.layer == LayerMask.NameToLayer("espada")){
            audioSource.Play();
            destruir();
        }
    }
    private void destruir(){
        b = true;
		if (gameManager.GetComponent<GameManager>() != null) {
			gameManager.GetComponent<GameManager>().sumaVida();
		}
		else {
			gameManager.GetComponent<GameManagerAgua>().sumaVida();
		}
        // desactivar hijos
        foreach(Transform child in transform){
            child.gameObject.SetActive(false);
        }
        // lanzar particulas
        transform.Find("Explosion").gameObject.SetActive(true);
        explosion1.Play();
        explosion2.Play();
        // desactivar collider
        GetComponent<Collider>().enabled = false;
    }
}

