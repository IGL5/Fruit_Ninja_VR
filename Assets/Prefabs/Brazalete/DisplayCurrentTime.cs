using System;
using TMPro;
using UnityEngine;

public class DisplayCurrentTime : MonoBehaviour
{
    public TMP_Text timeText;

    private void Start()
    {
        // Invocamos la función UpdateTime cada segundo
        InvokeRepeating("UpdateTime", 0f, 1f);
    }

    private void UpdateTime()
    {
        // Obtenemos la hora actual
        DateTime currentTime = DateTime.Now;

        // Formateamos la hora y los minutos
        string timeString = currentTime.ToString("HH:mm");

        // Actualizamos el TextMeshPro con la hora y los minutos
        timeText.text = timeString;
    }
}
