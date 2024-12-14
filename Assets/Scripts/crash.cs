using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crash : MonoBehaviour
{
    // Públicas
    public GameObject frutaRota;
    // Privadas
    private float tiempo = 0;
    private GameObject gameManager;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private bool b = false;
    private Rigidbody rb;
    private AudioSource audioSource;
    private AudioSource romperse;
    private ParticleSystem humo;
    private ParticleSystem gotas;
    private float tiempoParticulas = 0;
    
    private void Start(){
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager");

        audioSource = transform.Find("Audio Source").GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.6f, 1.3f);

        romperse = transform.Find("romperse").GetComponent<AudioSource>();
        romperse.pitch = Random.Range(0.8f, 1.2f);

        humo = transform.Find("Humo").GetComponent<ParticleSystem>();
        humo.transform.parent = null;

        gotas = transform.Find("gotas").GetComponent<ParticleSystem>();
        gotas.Stop();
    }
    public void Update()
    {
        if (tiempoParticulas >= 0.5f)
        {
            humo.Stop();
        }
        if(b){
            tiempo += Time.deltaTime;
            if(tiempo >= 8){
                Destroy(gameObject);
                Destroy(humo.gameObject);
            }
        }
        tiempoParticulas += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        gotas.transform.parent = null;
        romperse.Play();
        gotas.Play();
		if (gameManager != null) {
			if (collision.gameObject.layer ==LayerMask.NameToLayer("suelo")){
				if (gameManager.GetComponent<GameManager>() != null) 
				{
					gameManager.GetComponent<GameManager>().quitaVida();
				}
				else {
					gameManager.GetComponent<GameManagerAgua>().quitaVida();
				}
			}
			else if(collision.gameObject.layer == LayerMask.NameToLayer("espada")){
				if (gameManager.GetComponent<GameManager>() != null)
				{
					gameManager.GetComponent<GameManager>().sumaPunto();
				}
				else {
					gameManager.GetComponent<GameManagerAgua>().sumaPunto();
				}
			}
		}
        b = true;
        meshRenderer.enabled = false;
        frutaRota.SetActive(true);
        Rigidbody fR = frutaRota.GetComponent<Rigidbody>();
        fR.velocity = rb.velocity; // Transferir velocidad lineal
        fR.angularVelocity = rb.angularVelocity; // Transferir rotación angular

        Rigidbody[] rigidbodies = frutaRota.GetComponentsInChildren<Rigidbody>(); // Ver todas las piezas de la fruta
        foreach (Rigidbody piezarb in rigidbodies)
        {
            piezarb.velocity = rb.velocity;         // Transferir velocidad lineal
            piezarb.angularVelocity = rb.angularVelocity; // Transferir velocidad angular
        }

        meshCollider.enabled = false;
        
    }
}
