using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject zombieComun;
    [SerializeField] GameObject zombieInteligente;
    [SerializeField] float tiempoSpawn = 2f;
    private float currentTime;

    public MyQueue<GameObject> colaDeZombies = new MyQueue<GameObject>();

    public GrafoMA grafo;
    public Dictionary<int, Vector2> posicionesNodos;

    public int cantidadCarriles = 3;
    public int nodosPorCarril = 48;
    [SerializeField] float xInicial = -50f;

    void Start()
    {
        InicializarGrafo();

        for (int i = 0; i < 3; i++)
        {
            GameObject z1 = Instantiate(zombieComun);
            z1.GetComponent<zombie>().SetSpawner(this);
            colaDeZombies.Enqueue(z1);
            z1.SetActive(false);

            GameObject z2 = Instantiate(zombieInteligente);
            z2.GetComponent<ZombieInteligente>()?.SetSpawner(this);
            colaDeZombies.Enqueue(z2);
            z2.SetActive(false);
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= tiempoSpawn && colaDeZombies.Count() > 0)
        {
            GameObject zombie = colaDeZombies.Dequeue();
            zombie.SetActive(true);

            int carril = Random.Range(0, cantidadCarriles);
            int nodoInicio = -1;

            // Buscar primer nodo libre en el carril
            for (int i = 0; i < nodosPorCarril; i++)
            {
                int nodo = 1 + carril * nodosPorCarril + i;
                Vector2 pos = posicionesNodos[nodo];

                Collider2D[] colisiones = Physics2D.OverlapCircleAll(pos, 0.25f);
                bool ocupado = false;

                foreach (var c in colisiones)
                {
                    if (c.CompareTag("muro") || c.CompareTag("torre"))
                    {
                        ocupado = true;
                        break;
                    }
                }

                if (!ocupado)
                {
                    nodoInicio = nodo;
                    break;
                }
            }

            if (nodoInicio == -1)
            {
                Debug.LogWarning($"No hay nodo libre en carril {carril}, no se spawnea zombie.");
                zombie.SetActive(false);
                colaDeZombies.Enqueue(zombie);
                return;
            }

            int nodoFinal = nodoInicio + (nodosPorCarril - (nodoInicio % nodosPorCarril)) - 1;

            zombie.transform.position = posicionesNodos[nodoInicio];

            if (zombie.TryGetComponent<zombie>(out var zombieComun))
            {
                zombieComun.SetRutaConGrafo(grafo, nodoInicio, nodoFinal, posicionesNodos);
            }
            else if (zombie.TryGetComponent<ZombieInteligente>(out var zombieInteligente))
            {
                zombieInteligente.SetRutaConGrafo(grafo, nodoInicio, nodoFinal, posicionesNodos);
            }

            currentTime = 0f;
        }
    }

    void InicializarGrafo()
    {
        grafo = new GrafoMA();
        grafo.InicializarGrafo();
        posicionesNodos = new Dictionary<int, Vector2>();

        float[] posicionesY = { -9.85f, -17.15f, -24.31f };
        int id = 1;
        float espaciado = 1.9f;

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




