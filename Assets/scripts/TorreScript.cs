using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TorreScript : MonoBehaviour
{
    public Action<int> OnGetDamange;
    public int vida = 1000;
    //[SerializeField] int vidaMax = 1000;
    [SerializeField] float tiempoEntreDaños = 1f; // Tiempo en segundos entre golpes
    private float timerDaño;

    private void Update()
    {
        timerDaño += Time.deltaTime; // Aumenta el temporizador cada frame
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("zombie"))
        {
            if (timerDaño >= tiempoEntreDaños)
            {
                int daño = 10; // Daño que hace el zombie
                OnGetDamange?.Invoke(daño);
                timerDaño = 0f; // Reseteamos el contador para que espere de nuevo
            }
        }
    }

}

