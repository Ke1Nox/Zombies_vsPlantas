using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject prefabTorreEspecial;
    public GameObject prefabMuro;

    private bool torreEspecialColocada = false;
    public Spawner spawner;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (torreEspecialColocada)
            {
                Debug.LogWarning("Ya colocaste una torre especial.");
                return;
            }

            if (GestorGlobal.GastarMonedas(50))
            {
                Instantiate(prefabTorreEspecial, transform.position, Quaternion.identity);
                torreEspecialColocada = true;
                Debug.Log("Torre especial colocada.");
            }
            else
            {
                Debug.LogWarning("No tenés suficientes monedas para la torreta.");
            }
        }


        // Colocar muro con Y
        if (Input.GetKeyDown(KeyCode.Y))
        {
            int nodoDisponible = BuscarNodoParaMuro();

            if (nodoDisponible != -1)
            {
                if (GestorGlobal.GastarMonedas(50))
                {
                    Vector2 pos = spawner.posicionesNodos[nodoDisponible];
                    GameObject muro = Instantiate(prefabMuro, pos, Quaternion.identity);
                    muro.tag = "muro";
                    Debug.Log("Muro colocado en nodo " + nodoDisponible);
                }
                else
                {
                    Debug.LogWarning("No tenés suficientes monedas para el muro.");
                }
            }
            else
            {
                Debug.LogWarning("No se encontró nodo válido para colocar un muro.");
            }
        }
    }

    int BuscarNodoParaMuro()
    {
        int nodosPorCarril = spawner.nodosPorCarril;
        int cantidadCarriles = spawner.cantidadCarriles;
        int columnaInicial = 30;
        int rangoColumnas = 10;

        GameObject[] muros = GameObject.FindGameObjectsWithTag("muro");

        if (muros.Length >= 2)
        {
            Debug.LogWarning("Ya hay 2 muros colocados.");
            return -1;
        }

        HashSet<int> carrilesOcupados = new HashSet<int>();
        foreach (GameObject muro in muros)
        {
            Vector2 pos = muro.transform.position;

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

