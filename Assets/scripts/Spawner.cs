using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] GameObject zombiePrefab;
    [SerializeField] float tiempoSpawn = 2f;
    private float currentTime;

    public MyQueue<GameObject> colaDeZombies = new MyQueue<GameObject>();

    public GrafoMA grafo;

    public Dictionary<int, Vector2> posicionesNodos;

    [SerializeField] private int cantidadCarriles = 3;
    [SerializeField] private int nodosPorCarril = 28;
    [SerializeField] float xInicial = -50f; // donde empiezan

    void Start()
    {
        InicializarGrafo();

        for (int i = 0; i < 6; i++)
        {
            GameObject zombie = Instantiate(zombiePrefab);
            zombie.GetComponent<zombie>().SetSpawner(this);
            colaDeZombies.Enqueue(zombie);
            zombie.SetActive(false);
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= tiempoSpawn && colaDeZombies.Count() > 0)
        {
            GameObject zombie = colaDeZombies.Dequeue();
            zombie.SetActive(true);

            // Elegir carril aleatorio: 0 = arriba, 1 = medio, 2 = abajo
            int carril = Random.Range(0, cantidadCarriles);
            int nodoInicio = 1 + carril * nodosPorCarril;
            int nodoFinal = nodoInicio + nodosPorCarril - 1;

            zombie.transform.position = posicionesNodos[nodoInicio];
            zombie.GetComponent<zombie>().SetRutaConGrafo(grafo, nodoInicio, nodoFinal, posicionesNodos);

            currentTime = 0f;
        }
    }

    void InicializarGrafo()
    {
        grafo = new GrafoMA();
        grafo.InicializarGrafo();
        posicionesNodos = new Dictionary<int, Vector2>();

        float[] posicionesY = { -9.85f, -17.15f, -24.31f }; // Posiciones Y de los carriles

        int id = 1;
        float espaciado = 1.9f;  // separación entre nodos

        for (int carril = 0; carril < cantidadCarriles; carril++)
        {
            float y = posicionesY[carril];

            for (int i = 0; i < nodosPorCarril; i++)
            {
                grafo.AgregarVertice(id);
                posicionesNodos[id] = new Vector2(xInicial + (i * espaciado), y);

                if (i > 0)
                {
                    grafo.AgregarArista(id - 1, id, 1);
                }

                // Conexiones verticales entre carriles cada 5 nodos
                if (i % 5 == 0 && carril < cantidadCarriles - 1)
                {
                    int nodoActual = id;
                    int nodoInferior = id + nodosPorCarril;
                    grafo.AgregarArista(nodoActual, nodoInferior, 1);
                    grafo.AgregarArista(nodoInferior, nodoActual, 1);
                }

                id++;
            }
        }
    }
}


