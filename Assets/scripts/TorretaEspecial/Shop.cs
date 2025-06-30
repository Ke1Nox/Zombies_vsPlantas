using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject prefabTorreEspecial;
    public GameObject prefabMuro;

    public Spawner spawner; //  referencia al Spawner

    public int idNodo = 1; // el nodo donde querés colocar el muro (podés cambiarlo o hacer que el jugador lo seleccione)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (GameManager.Instance.GastarMonedas(50))
            {
                Instantiate(prefabTorreEspecial, transform.position, Quaternion.identity);
                Debug.Log("spáwn torre");
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
                int nodoDisponible = BuscarUltimoNodoLibre();
                if (nodoDisponible != -1)
                {
                    Vector2 pos = spawner.posicionesNodos[nodoDisponible];
                    Instantiate(prefabMuro, pos, Quaternion.identity);
                }
                else
                {
                    Debug.Log("No se encontró un nodo libre para colocar el muro");
                }
            }
            else
            {
                Debug.Log("No hay suficientes monedas");
            }
        }

        int BuscarUltimoNodoLibre()
        {
            // Asumimos que hay 3 carriles, cada uno con N nodos
            int nodosPorCarril = 27; // mismo valor que uses en Spawner
            int cantidadCarriles = 3;

            for (int carril = 0; carril < cantidadCarriles; carril++)
            {
                int baseNodo = 1 + carril * nodosPorCarril;
                int ultimo = baseNodo + nodosPorCarril - 1;

                // Buscamos desde el final hacia el principio
                for (int i = ultimo; i >= baseNodo; i--)
                {
                    Vector2 pos = spawner.posicionesNodos[i];
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 0.2f);

                    bool ocupado = false;
                    foreach (Collider2D col in colliders)
                    {
                        if (col.CompareTag("muro") || col.CompareTag("torre"))
                        {
                            ocupado = true;
                            break;
                        }
                    }

                    if (!ocupado)
                    {
                        return i;
                    }
                }
            }

            return -1; // no hay espacio
        }
    }
}
