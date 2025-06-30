using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VidaTorre : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();

        ActualizarVida(GameManager.Instance.vidaTorre); // Mostrar vida inicial

        if (GameManager.Instance.torreScript != null)
        {
            GameManager.Instance.torreScript.OnGetDamange += ActualizarVidaEvent; // Escuchamos cuando recibe da�o
        }
    }

    private void ActualizarVidaEvent(int da�o)
    {
        ActualizarVida(GameManager.Instance.vidaTorre);
    }

    private void ActualizarVida(int vidaActual)
    {
        textMesh.text = vidaActual.ToString("0");
    }
}
