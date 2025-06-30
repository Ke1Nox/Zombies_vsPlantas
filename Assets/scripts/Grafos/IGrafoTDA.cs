using System.Collections.Generic;
using UnityEngine;

public interface IGrafoTDA
{
    void InicializarGrafo();
    void AgregarVertice(int v);
    void EliminarVertice(int v);
    HashSet<int> Vertices();
    void AgregarArista(int v1, int v2, int peso);
    void EliminarArista(int v1, int v2);
    bool ExisteArista(int v1, int v2);
    int PesoArista(int v1, int v2);
}

public class GrafoMA : IGrafoTDA
{
    private int[,] MAdy;
    private int[] Etiqs;
    private int cantNodos;
    private int n = 200;

    public void InicializarGrafo()
    {
        MAdy = new int[n, n];
        Etiqs = new int[n];
        cantNodos = 0;
    }

    public void AgregarVertice(int v)
    {
        if (!ExisteVertice(v))
        {
            Etiqs[cantNodos] = v;
            for (int i = 0; i <= cantNodos; i++)
            {
                MAdy[cantNodos, i] = 0;
                MAdy[i, cantNodos] = 0;
            }
            cantNodos++;
        }
    }

    public void EliminarVertice(int v)
    {
        int index = ObtenerIndice(v);
        if (index != -1)
        {
            cantNodos--;
            for (int i = 0; i < cantNodos; i++)
            {
                MAdy[i, index] = MAdy[i, cantNodos];
                MAdy[index, i] = MAdy[cantNodos, i];
            }

            Etiqs[index] = Etiqs[cantNodos];

            for (int i = 0; i < cantNodos; i++)
            {
                MAdy[cantNodos, i] = 0;
                MAdy[i, cantNodos] = 0;
            }
        }
    }

    public HashSet<int> Vertices()
    {
        HashSet<int> vertices = new HashSet<int>();
        for (int i = 0; i < cantNodos; i++)
        {
            vertices.Add(Etiqs[i]);
        }
        return vertices;
    }

    public void AgregarArista(int v1, int v2, int peso)
    {
        int i = ObtenerIndice(v1);
        int j = ObtenerIndice(v2);
        if (i != -1 && j != -1)
        {
            MAdy[i, j] = peso;
        }
    }

    public void EliminarArista(int v1, int v2)
    {
        int i = ObtenerIndice(v1);
        int j = ObtenerIndice(v2);
        if (i != -1 && j != -1)
        {
            MAdy[i, j] = 0;
        }
    }

    public bool ExisteArista(int v1, int v2)
    {
        int i = ObtenerIndice(v1);
        int j = ObtenerIndice(v2);
        return (i != -1 && j != -1 && MAdy[i, j] != 0);
    }

    public int PesoArista(int v1, int v2)
    {
        int i = ObtenerIndice(v1);
        int j = ObtenerIndice(v2);
        if (i != -1 && j != -1)
        {
            return MAdy[i, j];
        }
        return 0;
    }

    private int ObtenerIndice(int v)
    {
        for (int i = 0; i < cantNodos; i++)
        {
            if (Etiqs[i] == v)
            {
                return i;
            }
        }
        return -1;
    }

    private bool ExisteVertice(int v)
    {
        return ObtenerIndice(v) != -1;
    }
}
