using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaPool : MonoBehaviour
{
    [SerializeField] private BalaPJ balaPrefab;
    [SerializeField] private int cantidadInicial = 5; // N�mero inicial de balas
    [SerializeField] private int maxActiveBalas = 5;  // M�ximo de balas activas

    private MyStack<BalaPJ> pool = new MyStack<BalaPJ>();
    public static int activeBalas = 0;  // Para llevar el registro de cu�ntas balas est�n activas
    private float recargaTime = 5f; // Tiempo de recarga (1 segundo)
    private float currentRecargaTime = 0f;

    void Start()
    {
        activeBalas = 0; // Reinicia el contador al cargar la escena

        for (int i = 0; i < cantidadInicial; i++)
        {
            BalaPJ bala = Instantiate(balaPrefab, transform);
            bala.gameObject.SetActive(false);
            bala.SetPool(this);
            pool.Push(bala);
        }
    }

    void Update()
    {
        // L�gica de recarga
        currentRecargaTime += Time.deltaTime;
        if (currentRecargaTime >= recargaTime)
        {
            
            currentRecargaTime = 0f; // Reset del temporizador
        }
    }


    public BalaPJ ObtenerBala()
    {
        // Verifica si el n�mero de balas activas es menor al m�ximo
        if (activeBalas < maxActiveBalas)
        {
            if (!pool.IsEmpty)
            {
                BalaPJ bala = pool.Pop();
                bala.gameObject.SetActive(true);
                activeBalas++;  // Aumentamos el contador de balas activas
                
                return bala;
            }
            else
            {
             
                return null;  // Si no hay balas en el pool y ya alcanzamos el l�mite de balas activas, no creamos m�s
            }
        }
        else
        {
            
            return null;  // No se puede obtener una nueva bala si ya hay balas activas en el l�mite
        }
    }

    public void DevolverBala(BalaPJ bala)
    {
        bala.gameObject.SetActive(false);
        pool.Push(bala);
        activeBalas--;  // Disminuimos el contador de balas activas
       
    }

    public bool HayBalasDisponibles()
    {
        return !pool.IsEmpty;
    }
}
