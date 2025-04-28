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
        // Inicializa el pool con balas desactivadas
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
                Debug.Log("Se obtuvo una bala del pool. Pila actual: " + pool.Count + ", Balas activas: " + activeBalas);
                return bala;
            }
            else
            {
                Debug.Log("No hay balas disponibles en el pool.");
                return null;  // Si no hay balas en el pool y ya alcanzamos el límite de balas activas, no creamos más
            }
        }
        else
        {
            Debug.Log("Se alcanzó el máximo de balas activas.");
            return null;  // No se puede obtener una nueva bala si ya hay balas activas en el límite
        }
    }

    public void DevolverBala(BalaPJ bala)
    {
        bala.gameObject.SetActive(false);
        pool.Push(bala);
        activeBalas--;  // Disminuimos el contador de balas activas
        Debug.Log("Se devolvió una bala al pool. Pila actual: " + pool.Count + ", Balas activas: " + activeBalas);
    }


    public bool HayBalasDisponibles()
    {
        return !pool.IsEmpty;
    }
}
