using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public Transform player;
    public List<GameObject> prefabs = new List<GameObject>();
    public GameObject powerup;
    public GameObject imagePrefab;
    public Transform canvasTransform;

    public float tiempoEntreDisparos;
    public float distancia = 10f; 
    public float g = -2.45f;
    public int vidas = 3;
    public int puntos = 0;

    //public TMP_Text textoVida;
    public TMP_Text puntuacion;

    //privadas
    private float tiempoTranscurrido;
    private bool gameOver = false;
    private bool powerupActivo = false;
	private float tiempoExtra = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        tiempoTranscurrido = 0;
        cambiaVidas(vidas);
        //textoVida.text = "VIDAS: " + vidas;
        puntuacion.text = "PTS: " + puntos;
    }

    // Update is called once per frame
    void Update()
    {
		if(!gameOver){
			if(tiempoTranscurrido >= tiempoEntreDisparos){
				lanzaNuevaFruta();
				tiempoTranscurrido = 0;
			}
				tiempoTranscurrido = tiempoTranscurrido+ Time.deltaTime;
				//Debug.Log(tiempoTranscurrido);
				// Acelero la cadencia de frutas
				if(tiempoEntreDisparos > 0.5f){
					tiempoEntreDisparos = tiempoEntreDisparos - Time.deltaTime/50;
				}
				// si va a 60 fps  una fruta cada 10 secs aprox
				if(vidas < 3 && Random.Range(0,1000) == 1 && powerupActivo == false){
					Instantiate(powerup, new Vector3(0,0, 0), powerup.transform.rotation);
					powerupActivo = true;
				}
			}
		else{// Al perder se le dan 10 segundos al player y se le envia de nuevo al menú
			cambiaVidas(0);
			tiempoExtra += Time.deltaTime;
			if(tiempoExtra >= 5f){
				SceneManager.LoadScene("MenuInicio");
			}
		}
        
        
    }
    void lanzaNuevaFruta(){
        
        float angle = Random.Range(0.0f, 2 * Mathf.PI); // Ángulo aleatorio en radianes

        var x = player.position.x + distancia * Mathf.Cos(angle);
        var z = player.position.z + distancia * Mathf.Sin(angle);
        var fruta = prefabs[Random.Range(0,prefabs.Count)];
        var frutaInstanciada = Instantiate(fruta, new Vector3(x,0.5f, z), fruta.transform.rotation);
        frutaInstanciada.GetComponent<Rigidbody>().velocity = CalculaVelocidad(frutaInstanciada.transform.position, player.position, 1.5f);
        frutaInstanciada.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-10,10),Random.Range(-10,10),Random.Range(-10,10));
        
    }

    Vector3 CalculaVelocidad(Vector3 puntoInicio, Vector3 puntoFin, float alturaMax)
    {
        // Diferencias de posición
        float displacementX = puntoFin.x - puntoInicio.x;
        float displacementY = puntoFin.y - puntoInicio.y;
        float displacementZ = puntoFin.z - puntoInicio.z;

        displacementX = displacementX *1.2f;
        displacementZ = displacementZ *1.2f;

        // Tiempo estimado para alcanzar el destino
        float tiempo = Mathf.Sqrt(-2 * alturaMax / g) + Mathf.Sqrt(2 * (displacementY - alturaMax) / g);

        // Velocidad en X y Z (horizontal)
        float velocityX = displacementX / tiempo;
        float velocityZ = displacementZ / tiempo;

        // Velocidad en Y (vertical)
        float velocityY = Mathf.Sqrt(-2 * g * alturaMax);
        return new Vector3(velocityX, velocityY, velocityZ);
    }
    public void quitaVida(){
        if (gameOver) return;
        vidas--;
        Debug.Log("Vidas: "+ vidas);
        if(vidas == 0){
            Debug.Log("Game Over");
            gameOver = true;
        }
        cambiaVidas(vidas);
        //textoVida.text = "VIDAS: " + vidas;
    }
    public void sumaVida(){
		if(vidas <3){
			vidas++;
			powerupActivo = false;
			cambiaVidas(vidas);
		}
        //textoVida.text = "VIDAS: " + vidas;
    }
    public void sumaPunto(){
        puntos+= 100;
        Debug.Log("Puntos: "+puntos);
        puntuacion.text = "PTS: " + puntos;
        // Poner aqui para cambiar lo que hagamos para mostrar los puntos 
    }


    
    // Función para vaciar el Canvas de imágenes anteriores
    void VaciarCanvas()
    {
        foreach (Transform hijo in canvasTransform)
        {
            Destroy(hijo.gameObject); // Destruir todos los objetos hijos del Canvas
        }
    }
    
        // Llamar a esta función para cambiar el número de vidas y actualizar el Canvas
    public void cambiaVidas(int vida)
    {
        vidas = vida;

        // Primero, vaciamos el Canvas de cualquier imagen previa
        VaciarCanvas();

        // Luego agregamos las imágenes según el número de vidas
        for (int i = 0; i < vidas; i++)
        {
            // Instanciamos el prefab de la imagen de vida en el Canvas
            GameObject nuevaImagen = Instantiate(imagePrefab, canvasTransform);

            // Si es necesario, ajusta la posición o el tamaño de las imágenes
            nuevaImagen.GetComponent<RectTransform>().anchoredPosition = new Vector2((i-1) * 300, 0); // Ejemplo de distribución horizontal
        }
    }
}
