using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportActivator : MonoBehaviour
{
	// Componente Ray Interactor del Teleport
	[SerializeField] private XRRayInteractor _teleportRayInteractor;
	// Interpreta los valores de un controlador de entrada utilizando acciones del sistema de entrada
	private ActionBasedController _teleportActionBasedController;

	private void Awake()
	{
		// Referencia al componente RayInteractor del Teleport
		_teleportActionBasedController = _teleportRayInteractor.GetComponent<ActionBasedController>();
	}

	// Se utiliza para activar la función de teletransporte.
	public void ActivateTeleport()
	{
		TeleportControllerEnabled(true);
	}

	// Se utiliza para desactivar la función de teletransporte con un retraso de 0.1 segundos.
	public void DeactivateTeleport()
	{
		//  Programa la ejecución de otro método después de un retraso específico
		Invoke(nameof(WaitAndDeactivate), 0.1f);
	}

	private void WaitAndDeactivate()
	{
		TeleportControllerEnabled(false);
	}


	private void TeleportControllerEnabled(bool value)
	{
		_teleportRayInteractor.enabled = value;
		_teleportActionBasedController.enableInputActions = value;
	}
}
