using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Puntaje : MonoBehaviour
{
    private float Puntos;
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        
    }

    private void Update()
    {
        Puntos = GameManagerEndless.Instance.puntuacionActual;

        textMesh.text = Puntos.ToString("0");
    }
    
}