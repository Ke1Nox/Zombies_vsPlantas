using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TorreScript : MonoBehaviour
{
    public Action<int> OnGetDamange;
    public int vida = 1000;

    private void Awake()
    {
        OnGetDamange += RecibirDaño;
    }

    private void RecibirDaño(int daño)
    {
        vida -= daño;
        Debug.Log($"Torre recibió {daño} de daño. Vida restante: {vida}");

        if (vida <= 0)
        {
            Debug.Log("¡La torre fue destruida!");
            // Podés agregar lógica para game over, explosión, animación, etc.
        }
    }
}


