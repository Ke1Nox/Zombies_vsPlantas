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
                Debug.Log("Spawnéo torre");
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

                    // Desplazamos visualmente solo el muro, sin romper la lógica del nodo
                    Vector2 desplazamiento = new Vector2(-0.4f, 0f);
                    Instantiate(prefabMuro, pos + desplazamiento, Quaternion.identity);
                }
                else
                {
                    Debug.Log("No se encontró ningún nodo disponible para el muro");
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

        // CENTRAR búsqueda alrededor de columna 30 (x ≈ 6)
        int columnaCentral = 30;
        int rango = 5;

        for (int offset = 0; offset <= rango; offset++)
        {
            int[] columnas = { columnaCentral - offset, columnaCentral + offset };

            foreach (int columna in columnas)
            {
                if (columna < 5 || columna >= nodosPorCarril) continue;

                for (int carril = 0; carril < cantidadCarriles; carril++)
                {
                    int nodo = 1 + carril * nodosPorCarril + columna;
                    Vector2 pos = spawner.posicionesNodos[nodo];

                    if (!EstaOcupado(pos))
                        return nodo;
                }
            }
        }
        return -1; // todos los carriles están llenos
    }

    bool EstaOcupado(Vector2 pos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 0.25f);
        foreach (var col in colliders)
        {
            if (col.CompareTag("muro") || col.CompareTag("torre"))
                return true;
        }
        return false;
    }
}
