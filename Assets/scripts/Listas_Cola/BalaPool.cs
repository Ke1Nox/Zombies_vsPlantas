using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaPool : MonoBehaviour
{
    [SerializeField] private BalaPJ balaPrefab;
    [SerializeField] private int cantidadInicial = 5; // Número inicial de balas
    [SerializeField] private int maxActiveBalas = 5;  // Máximo de balas activas

    private MyStack<BalaPJ> pool = new MyStack<BalaPJ>();
    public static int activeBalas = 0;  // Para llevar el registro de cuántas balas están activas
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
        // Lógica de recarga
        currentRecargaTime += Time.deltaTime;
        if (currentRecargaTime >= recargaTime)
        {
            
            currentRecargaTime = 0f; // Reset del temporizador
        }
    }


    public BalaPJ ObtenerBala()
    {
        // Verifica si el número de balas activas es menor al máximo
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
             
                return null;  // Si no hay balas en el pool y ya alcanzamos el límite de balas activas, no creamos más
            }
        }
        else
        {
            
            return null;  // No se puede obtener una nueva bala si ya hay balas activas en el límite
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
