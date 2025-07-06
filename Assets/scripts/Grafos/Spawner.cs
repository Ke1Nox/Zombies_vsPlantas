using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject zombieComun;
    [SerializeField] GameObject zombieInteligente;
    [SerializeField] float tiempoSpawn = 2f;
    [SerializeField] private bool modoInfinito = false;

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
        CrearZombies(20); // zombies iniciales
    }

    void Update()
    {
        if (modoInfinito) return;

        currentTime += Time.deltaTime;

        if (currentTime >= tiempoSpawn && colaDeZombies.Count() > 0)
        {
            SpawnZombieRandom();
        }
    }

    public void SpawnearZombies(int cantidad)
    {
        if (posicionesNodos == null || posicionesNodos.Count == 0)
        {
            Debug.LogWarning("posicionesNodos está vacío. Reintentando InicializarGrafo().");
            InicializarGrafo();
        }

        Debug.Log($"Oleada requiere: {cantidad} zombies. Zombies disponibles: {colaDeZombies.Count()}");

        if (colaDeZombies.Count() < cantidad)
        {
            int faltan = cantidad - colaDeZombies.Count();
            CrearZombies(Mathf.CeilToInt(faltan / 2f));
        }

        for (int i = 0; i < cantidad; i++)
        {
            if (colaDeZombies.Count() > 0)
                SpawnZombieRandom();
        }
    }

    private void CrearZombies(int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            GameObject z1 = Instantiate(zombieComun);
            z1.GetComponent<zombie>().SetSpawner(this);
            z1.SetActive(false);
            colaDeZombies.Enqueue(z1);

            GameObject z2 = Instantiate(zombieInteligente);
            z2.GetComponent<ZombieInteligente>()?.SetSpawner(this);
            z2.SetActive(false);
            colaDeZombies.Enqueue(z2);
        }
    }

    private void SpawnZombieRandom()
    {
        GameObject zombie = colaDeZombies.Dequeue();

        int carril = Random.Range(0, cantidadCarriles);
        int nodoInicio = -1;

        for (int i = 0; i < nodosPorCarril; i++)
        {
            int nodo = 1 + i * cantidadCarriles + carril;

            if (posicionesNodos == null || !posicionesNodos.ContainsKey(nodo))
            {
                Debug.LogWarning("Nodo inválido o posicionesNodos no inicializado.");
                continue;
            }

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

        if (!posicionesNodos.ContainsKey(nodoInicio))
        {
            Debug.LogError("posicionesNodos no contiene nodoInicio válido: " + nodoInicio);
            return;
        }

        int nodoFinal = 1 + (nodosPorCarril - 1) * cantidadCarriles + carril;

        zombie.transform.position = posicionesNodos[nodoInicio];
        zombie.transform.rotation = Quaternion.identity;
        zombie.SetActive(true);

        if (zombie.TryGetComponent<zombie>(out var zombieComun))
        {
            Dijkstra dijkstra = new Dijkstra(grafo);
            List<int> ruta = dijkstra.CalcularCamino(nodoInicio, nodoFinal);
            zombieComun.SetRutaManual(ruta, posicionesNodos);
        }
        else if (zombie.TryGetComponent<ZombieInteligente>(out var zombieInteligente))
        {
            Dijkstra dijkstra = new Dijkstra(grafo);
            List<int> ruta = dijkstra.CalcularCamino(nodoInicio, nodoFinal);
            zombieInteligente.SetRutaManual(ruta, posicionesNodos);
        }

        Debug.Log($"Zombie spawneado en nodo {nodoInicio}. Restan en cola: {colaDeZombies.Count()}");
        currentTime = 0f;
    }

    public void InicializarGrafo()
    {
        grafo = new GrafoMA();
        grafo.InicializarGrafo();
        posicionesNodos = new Dictionary<int, Vector2>();

        float[] posicionesY = { -9.85f, -17.15f, -24.31f };
        float espaciado = 1.9f;
        int id = 1;

        for (int i = 0; i < nodosPorCarril; i++)
        {
            for (int carril = 0; carril < cantidadCarriles; carril++)
            {
                float y = posicionesY[carril];
                grafo.AgregarVertice(id);
                posicionesNodos[id] = new Vector2(xInicial + (i * espaciado), y);

                if (i > 0)
                {
                    grafo.AgregarArista(id - cantidadCarriles, id, 1);
                    grafo.AgregarArista(id, id - cantidadCarriles, 1);
                }

                if (carril > 0)
                {
                    grafo.AgregarArista(id, id - 1, 1);
                    grafo.AgregarArista(id - 1, id, 1);
                }

                id++;
            }
        }

        Debug.Log("Grafo inicializado con " + posicionesNodos.Count + " posiciones.");
    }

    void OnDrawGizmos()
    {
        if (posicionesNodos == null || grafo == null) return;

        Gizmos.color = Color.gray;
        foreach (var par in posicionesNodos)
        {
            Gizmos.DrawSphere(par.Value, 0.1f);
        }

        foreach (var origen in posicionesNodos.Keys)
        {
            foreach (var destino in posicionesNodos.Keys)
            {
                if (!grafo.ExisteArista(origen, destino)) continue;

                Vector2 origenPos = posicionesNodos[origen];
                Vector2 destinoPos = posicionesNodos[destino];

                Gizmos.color = Mathf.Approximately(origenPos.y, destinoPos.y) ? Color.green : Color.cyan;
                Gizmos.DrawLine(origenPos, destinoPos);
            }
        }

        Gizmos.color = Color.red;
        foreach (var par in posicionesNodos)
        {
            Collider2D[] col = Physics2D.OverlapCircleAll(par.Value, 0.25f);
            foreach (var c in col)
            {
                if (c.GetComponent<Muro>() != null)
                {
                    Gizmos.DrawWireSphere(par.Value, 0.25f);
                    break;
                }
            }
        }
    }
}


