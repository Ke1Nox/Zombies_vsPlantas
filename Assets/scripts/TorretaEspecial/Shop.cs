using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject prefabTorreEspecial;
    public GameObject prefabMuro;

    public Spawner spawner;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (GameManager.Instance.GastarMonedas(50))
            {
                Instantiate(prefabTorreEspecial, transform.position, Quaternion.identity);
                Debug.Log("Spawn�o torre");
            }
            else
            {
                Debug.Log("No hay suficientes monedas");
            }
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (GameManager.Instance.GastarMonedas(50))
            {
                int nodoDisponible = BuscarNodoParaMuro();
                if (nodoDisponible != -1)
                {
                    Vector2 pos = spawner.posicionesNodos[nodoDisponible];

                    // Desplazamos visualmente solo el muro, sin romper la l�gica del nodo
                    Vector2 desplazamiento = new Vector2(-0.4f, 0f);
                    Instantiate(prefabMuro, pos + desplazamiento, Quaternion.identity);
                }
                else
                {
                    Debug.Log("No se encontr� ning�n nodo disponible para el muro");
                }
            }
            else
            {
                Debug.Log("No hay suficientes monedas");
            }
        }

    }

    int BuscarNodoParaMuro()
    {
        int nodosPorCarril = spawner.nodosPorCarril;
        int cantidadCarriles = spawner.cantidadCarriles;

        // Probar desde el �ltimo nodo de cada carril hacia el primero (zona m�s peligrosa)
        for (int columna = nodosPorCarril - 1; columna >= 5; columna--)
        {
            for (int carril = 0; carril < cantidadCarriles; carril++)
            {
                int nodo = 1 + carril * nodosPorCarril + columna;
                Vector2 pos = spawner.posicionesNodos[nodo];

                if (!EstaOcupado(pos))
                {
                    return nodo;
                }
            }
        }

        return -1; // todos los carriles est�n llenos
    }

    bool EstaOcupado(Vector2 pos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 0.25f);
        foreach (var col in colliders)
        {
            if (col.CompareTag("muro") || col.CompareTag("torre") || col.CompareTag("obstaculo"))
                return true;
        }
        return false;
    }
}
