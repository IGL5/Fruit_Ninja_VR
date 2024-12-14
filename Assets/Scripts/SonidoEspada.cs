using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SonidoEspada : MonoBehaviour
{
    private AudioSource audioSource; // Componente AudioSource para reproducir el sonido
    private XRGrabInteractable grabInteractable; // El componente XRGrabInteractable del objeto

    void Start()
    {
        // Obtén el componente XRGrabInteractable
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();

        // Suscríbete al evento onSelectEntered
        grabInteractable.onSelectEntered.AddListener(ReproducirSonidoAgarrar);

    }

    // Método que se llama cuando el objeto es agarrado
    private void ReproducirSonidoAgarrar(XRBaseInteractor interactor)
    {
        audioSource.Play();
    }
}

