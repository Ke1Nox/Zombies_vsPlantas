using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puntaje : MonoBehaviour
{
    private float Puntos;
    private TextMeshProUGUI textMesh;

    private GameManager gameManager;
    private GameManagerEndless gameManagerEndless;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();

        // Intentar obtener ambos managers
        gameManager = GameManager.Instance;
        gameManagerEndless = GameManagerEndless.Instance;
    }

    private void Update()
    {
        if (GameManagerEndless.Instance != null)
        {
            Puntos = GameManagerEndless.Instance.puntuacionActual;
        }
        else if (GameManager.Instance != null)
        {
            Puntos = GameManager.Instance.puntos;
        }

        textMesh.text = Puntos.ToString("0");
    }
}