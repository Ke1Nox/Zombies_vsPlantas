using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject zombieComun;
    [SerializeField] GameObject zombieInteligente;
    [SerializeField] float tiempoSpawn = 2f;
    [SerializeField] private bool modoInfinito = false;

    private float currentTime;

    private int zombiesSpawneadosEstaOleada = 0;
    private float intervaloSpawn = 1.5f;
    private float tiempoProximoSpawn = 0f;
    private int zombiesPorOleada = 5;

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
        if (modoInfinito)
        {
            currentTime += Time.deltaTime;

            int oleada = GameManagerEndless.Instance?.NumeroOleada ?? 1;
            zombiesPorOleada = GameManagerEndless.Instance?.EnemigosVivos ?? (5 + oleada * 2);

            if (zombiesSpawneadosEstaOleada < zombiesPorOleada && currentTime >= tiempoProximoSpawn)
            {
                if (colaDeZombies.Count() <= 5)
                    CrearZombies(10);

                SpawnZombieRandom();
                zombiesSpawneadosEstaOleada++;
                tiempoProximoSpawn = currentTime + intervaloSpawn;
            }
        }
        else
        {
            currentTime += Time.deltaTime;

            if (currentTime >= tiempoSpawn && colaDeZombies.Count() > 0)
            {
                SpawnZombieRandom();
            }
        }
    }

    public void SpawnearZombies(int cantidad)
    {
        if (posicionesNodos == null || posicionesNodos.Count == 0)
            InicializarGrafo();

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

        zombiesSpawneadosEstaOleada = 0;
    }
    private float ObtenerVelocidadPorOleada(int oleada)
    {
        if (oleada < 5) return 5f;           // Tipo 1
        else if (oleada < 10) return 6.5f;   // Tipo 2
        else if (oleada < 15) return 8f;     // Tipo 3
        else if (oleada < 20) return 9.5f;   // Tipo 4
        else return 11f;                     // Tipo 5
    }

    private void CrearZombies(int cantidad)
    {
        int oleada = GameManagerEndless.Instance?.NumeroOleada ?? 1;
        float probabilidadInteligente = Mathf.Clamp01(oleada * 0.05f);

        for (int i = 0; i < cantidad; i++)
        {
            bool esInteligente = Random.value < probabilidadInteligente;

            GameObject z = esInteligente ? Instantiate(zombieInteligente) : Instantiate(zombieComun);

            if (z.TryGetComponent<zombie>(out var zc))
                zc.SetSpawner(this);
            else if (z.TryGetComponent<ZombieInteligente>(out var zi))
                zi.SetSpawner(this);

            z.SetActive(false);
            colaDeZombies.Enqueue(z);
        }
    }

    private void SpawnZombieRandom()
    {
        GameObject zombie = colaDeZombies.Dequeue();

        List<int> carrilesDisponibles = Enumerable.Range(0, cantidadCarriles).OrderBy(x => Random.value).ToList();
        int nodoInicio = -1;
        int carrilElegido = -1;

        foreach (int carril in carrilesDisponibles)
        {
            for (int i = 0; i < nodosPorCarril; i++)
            {
                int nodo = 1 + i * cantidadCarriles + carril;
                if (!posicionesNodos.ContainsKey(nodo)) continue;

                Vector2 pos = posicionesNodos[nodo];
                Collider2D[] colisiones = Physics2D.OverlapCircleAll(pos, 0.25f);
                bool ocupado = colisiones.Any(c => c.CompareTag("muro") || c.CompareTag("torre"));

                if (!ocupado)
                {
                    nodoInicio = nodo;
                    carrilElegido = carril;
                    break;
                }
            }
            if (nodoInicio != -1) break;
        }

        if (nodoInicio == -1)
        {
            zombie.SetActive(false);
            colaDeZombies.Enqueue(zombie);
            return;
        }

        int nodoFinal = 1 + (nodosPorCarril - 1) * cantidadCarriles + carrilElegido;
        zombie.transform.position = posicionesNodos[nodoInicio];
        zombie.transform.rotation = Quaternion.identity;
        zombie.SetActive(true);

        int oleada = GameManagerEndless.Instance?.NumeroOleada ?? 1;
        float velocidadFija = ObtenerVelocidadPorOleada(oleada);

        if (zombie.TryGetComponent<zombie>(out var zombieComun))
        {
            var dijkstra = new Dijkstra(grafo);
            zombieComun.SetRutaManual(dijkstra.CalcularCamino(nodoInicio, nodoFinal), posicionesNodos);
            zombieComun.SetVelocidadBase(velocidadFija);
        }
        else if (zombie.TryGetComponent<ZombieInteligente>(out var zombieInteligente))
        {
            var dijkstra = new Dijkstra(grafo);
            zombieInteligente.SetRutaManual(dijkstra.CalcularCamino(nodoInicio, nodoFinal), posicionesNodos);
            zombieInteligente.SetVelocidadBase(velocidadFija);
        }

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
    }

    private void OnDrawGizmos()
    {
        if (grafo == null || posicionesNodos == null)
            return;

        HashSet<int> vertices = grafo.Vertices();

        foreach (int origen in vertices)
        {
            foreach (int destino in vertices)
            {
                if (grafo.ExisteArista(origen, destino))
                {
                    if (!posicionesNodos.ContainsKey(origen) || !posicionesNodos.ContainsKey(destino))
                        continue;

                    Vector2 posOrigen = posicionesNodos[origen];
                    Vector2 posDestino = posicionesNodos[destino];

                    // Diferenciar verticales y horizontales por el eje Y
                    if (Mathf.Approximately(posOrigen.y, posDestino.y))
                        Gizmos.color = Color.red; // misma fila → horizontal
                    else
                        Gizmos.color = Color.green; // distinta fila → vertical

                    Gizmos.DrawLine(posOrigen, posDestino);
                }
            }

            // Dibujar un punto en el nodo
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(posicionesNodos[origen], 0.1f);
        }
    }


}


