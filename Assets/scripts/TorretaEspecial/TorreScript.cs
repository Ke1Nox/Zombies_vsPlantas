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
        OnGetDamange += RecibirDa�o;
    }

    private void RecibirDa�o(int da�o)
    {
        vida -= da�o;
        Debug.Log($"Torre recibi� {da�o} de da�o. Vida restante: {vida}");

        if (vida <= 0)
        {
            Debug.Log("�La torre fue destruida!");
            // Pod�s agregar l�gica para game over, explosi�n, animaci�n, etc.
        }
    }
}


