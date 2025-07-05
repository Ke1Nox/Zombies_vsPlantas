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

                    GameObject muro = Instantiate(prefabMuro, pos, Quaternion.identity);
                    muro.tag = "muro";

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
        int columnaInicial = 30;
        int rangoColumnas = 10;

        // Obtener todos los muros existentes
        GameObject[] muros = GameObject.FindGameObjectsWithTag("muro");

        if (muros.Length >= 2)
        {
            Debug.Log("Ya hay 2 muros colocados");
            return -1;
        }

        // Obtener carriles ocupados por los muros existentes
        HashSet<int> carrilesOcupados = new HashSet<int>();
        foreach (GameObject muro in muros)
        {
            Vector2 pos = muro.transform.position;

            // Buscar en qué carril está
            for (int carril = 0; carril < cantidadCarriles; carril++)
            {
                for (int offset = 0; offset <= rangoColumnas; offset++)
                {
                    int columna = columnaInicial - offset;
                    if (columna < 0) break;

                    int nodo = 1 + columna * cantidadCarriles + carril;
                    if (!spawner.posicionesNodos.ContainsKey(nodo)) continue;

                    Vector2 nodoPos = spawner.posicionesNodos[nodo];
                    if (Vector2.Distance(pos, nodoPos) < 0.1f)
                    {
                        carrilesOcupados.Add(carril);
                        break;
                    }
                }
            }
        }

        // Buscar un carril libre
        for (int carril = 0; carril < cantidadCarriles; carril++)
        {
            if (carrilesOcupados.Contains(carril)) continue;

            for (int offset = 0; offset <= rangoColumnas; offset++)
            {
                int columna = columnaInicial - offset;
                if (columna < 0) break;

                int nodo = 1 + columna * cantidadCarriles + carril;
                if (!spawner.posicionesNodos.ContainsKey(nodo)) continue;

                Vector2 pos = spawner.posicionesNodos[nodo];

                if (!EstaOcupado(pos))
                    return nodo;
            }
        }

        return -1;
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
