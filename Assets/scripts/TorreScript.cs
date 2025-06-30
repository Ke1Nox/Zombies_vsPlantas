using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TorreScript : MonoBehaviour
{
    public Action<int> OnGetDamange;
    public int vida = 1000;
    //[SerializeField] int vidaMax = 1000;
    [SerializeField] float tiempoEntreDa�os = 1f; // Tiempo en segundos entre golpes
    private float timerDa�o;

    private void Update()
    {
        timerDa�o += Time.deltaTime; // Aumenta el temporizador cada frame
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("zombie"))
        {
            if (timerDa�o >= tiempoEntreDa�os)
            {
                int da�o = 10; // Da�o que hace el zombie
                OnGetDamange?.Invoke(da�o);
                timerDa�o = 0f; // Reseteamos el contador para que espere de nuevo
            }
        }
    }

}

