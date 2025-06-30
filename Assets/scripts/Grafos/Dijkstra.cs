using System.Collections.Generic;
using UnityEngine;

public class Dijkstra
{
    private GrafoMA grafo;
    private Dictionary<int, int> distancias;
    private Dictionary<int, int> anteriores;
    private HashSet<int> visitados;

    private HashSet<int> nodosBloqueados;

    public Dijkstra(GrafoMA grafo, HashSet<int> nodosBloqueados = null)
    {
        this.grafo = grafo;
        this.nodosBloqueados = nodosBloqueados ?? new HashSet<int>();
    }

    public Dijkstra(GrafoMA grafo)
    {
        this.grafo = grafo;
    }

    public List<int> CalcularCamino(int origen, int destino)
    {
        if (nodosBloqueados.Contains(origen) || nodosBloqueados.Contains(destino))
            return new List<int>(); // no hay ruta válida si el origen o destino están bloqueados
        distancias = new Dictionary<int, int>();
        anteriores = new Dictionary<int, int>();
        visitados = new HashSet<int>();

        HashSet<int> vertices = grafo.Vertices();

        foreach (int v in vertices)
        {
            distancias[v] = int.MaxValue;
            anteriores[v] = -1;
        }

        distancias[origen] = 0;

        while (visitados.Count < vertices.Count)
        {
            int actual = ObtenerVerticeMenorDistancia();
            if (actual == -1 || actual == destino) break;

            visitados.Add(actual);

            foreach (int vecino in vertices)
            {
                if (grafo.ExisteArista(actual, vecino) &&
        !visitados.Contains(vecino) &&
        !nodosBloqueados.Contains(vecino)) //  Ignora nodo bloqueado
                {
                    int nuevaDist = distancias[actual] + grafo.PesoArista(actual, vecino);
                    if (nuevaDist < distancias[vecino])
                    {
                        distancias[vecino] = nuevaDist;
                        anteriores[vecino] = actual;
                    }
                }
            }
        }

        return ReconstruirCamino(origen, destino);
    }

    private int ObtenerVerticeMenorDistancia()
    {
        int menorDist = int.MaxValue;
        int verticeMenor = -1;

        foreach (var par in distancias)
        {
            if (!visitados.Contains(par.Key) && par.Value < menorDist)
            {
                menorDist = par.Value;
                verticeMenor = par.Key;
            }
        }

        return verticeMenor;
    }

    private List<int> ReconstruirCamino(int origen, int destino)
    {
        List<int> camino = new List<int>();
        int actual = destino;

        while (actual != -1)
        {
            camino.Insert(0, actual);
            actual = anteriores[actual];
        }

        if (camino[0] != origen)
        {
            // No hay camino
            return new List<int>();
        }

        return camino;
    }
}

