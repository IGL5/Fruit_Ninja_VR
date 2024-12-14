using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Golpe : MonoBehaviour
{    
    //XRDirectInteractor 
    private XRGrabInteractable xrGrabbable;
    public LayerMask layer;

    // Start is called before the first frame update
    void Start()
    {
        xrGrabbable = GetComponent<XRGrabInteractable>();       
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void vibracion()
    {
        if (xrGrabbable.isSelected == true)
        {
            XRDirectInteractor xrcontroller = (XRDirectInteractor) xrGrabbable.firstInteractorSelecting;
            xrcontroller.SendHapticImpulse(0.5f, 0.1f);
        }        
    }

    private void OnCollisionEnter(Collision other){
        if(other.gameObject.layer == LayerMask.NameToLayer("fruit")){
            vibracion();
        }
    }

    
}
