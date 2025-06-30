using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrafoTest : MonoBehaviour
{
    void Start()
    {
        GrafoMA grafo = new GrafoMA();
        grafo.InicializarGrafo();

        // Agregar nodos (pueden ser posiciones en tu mapa)
        grafo.AgregarVertice(1);
        grafo.AgregarVertice(2);
        grafo.AgregarVertice(3);

        // Conectar nodos con aristas
        grafo.AgregarArista(1, 2, 5); // de 1 a 2, peso 5
        grafo.AgregarArista(2, 3, 2); // de 2 a 3, peso 2

        // Probar si existen
        Debug.Log("Existe arista 1->2: " + grafo.ExisteArista(1, 2));
        Debug.Log("Peso de 2->3: " + grafo.PesoArista(2, 3));

        // Mostrar todos los vértices
        foreach (int v in grafo.Vertices())
        {
            Debug.Log("Vertice: " + v);
        }

        Dijkstra dijkstra = new Dijkstra(grafo);
        List<int> camino = dijkstra.CalcularCamino(1, 3);

        Debug.Log("Camino de 1 a 3:");
        foreach (int nodo in camino)
        {
            Debug.Log(nodo);
        }
    }

}

