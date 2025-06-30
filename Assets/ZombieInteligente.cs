using System.Collections.Generic;
using UnityEngine;

public class ZombieInteligente : MonoBehaviour
{
    [SerializeField] private int vidaZombie = 100;
    [SerializeField] private float velocidad = 5f;

    private Rigidbody2D rb2D;
    private Spawner spawner;

    private List<int> ruta;
    private Dictionary<int, Vector2> posiciones;
    private int indiceActual = 0;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spawner = Object.FindFirstObjectByType<Spawner>();
    }

    private void FixedUpdate()
    {
        if (ruta == null || indiceActual >= ruta.Count)
            return;

        int nodoActual = ruta[indiceActual];
        Vector2 destino = posiciones[nodoActual];

        // Detectar muro en el destino
        Collider2D[] colisiones = Physics2D.OverlapCircleAll(destino, 0.25f);

        foreach (Collider2D col in colisiones)
        {
            if (col.CompareTag("muro"))
            {
                RecalcularRutaEsquivando();
                return;
            }
        }

        // Movimiento normal
        Vector2 direccion = (destino - rb2D.position).normalized;
        rb2D.MovePosition(rb2D.position + direccion * velocidad * Time.fixedDeltaTime);

        if (Vector2.Distance(transform.position, destino) < 0.1f)
        {
            indiceActual++;
            if (indiceActual >= ruta.Count)
            {
                LlegarATorre();
            }
        }
    }

    private void RecalcularRutaEsquivando()
    {
        HashSet<int> nodosBloqueados = new HashSet<int>();
        foreach (var par in posiciones)
        {
            Collider2D[] colisiones = Physics2D.OverlapCircleAll(par.Value, 0.25f);
            foreach (var col in colisiones)
            {
                if (col.CompareTag("muro"))
                {
                    nodosBloqueados.Add(par.Key);
                    break;
                }
            }
        }

        Dijkstra dijkstra = new Dijkstra(spawner.grafo, nodosBloqueados);

        int nodosPorCarril = spawner.nodosPorCarril;
        int carriles = spawner.cantidadCarriles;

        //  Intentar rutas desde este carril y otros carriles
        for (int c = 0; c < carriles; c++)
        {
            int nuevoInicio = -1;

            // Buscar un nodo válido de inicio en este carril
            for (int i = 0; i < nodosPorCarril; i++)
            {
                int nodo = 1 + c * nodosPorCarril + i;
                if (!nodosBloqueados.Contains(nodo))
                {
                    nuevoInicio = nodo;
                    break;
                }
            }

            if (nuevoInicio == -1) continue; // todo el carril bloqueado

            // Buscar nodo final en el carril
            int destino = 1 + c * nodosPorCarril + (nodosPorCarril - 1);

            if (nodosBloqueados.Contains(destino)) continue;

            List<int> nuevaRuta = dijkstra.CalcularCamino(nuevoInicio, destino);

            if (nuevaRuta.Count > 1)
            {
                ruta = nuevaRuta;
                indiceActual = 0;
                transform.position = posiciones[nuevaRuta[0]]; // Mover al nuevo carril
                Debug.Log("Zombie inteligente: cambio de carril para esquivar muro");
                return;
            }
        }

        Debug.Log("Zombie inteligente: sin ruta alternativa, queda trabado");
    }
    public void SetSpawner(Spawner spawnerReference)
    {
        spawner = spawnerReference;
    }
    private void LlegarATorre()
    {
        GameManager.Instance.DamageTower(vidaZombie);
        gameObject.SetActive(false);
        spawner.colaDeZombies.Enqueue(gameObject);
    }

    public void SetRutaConGrafo(GrafoMA grafo, int origen, int destino, Dictionary<int, Vector2> posicionesNodos)
    {
        posiciones = posicionesNodos;
        HashSet<int> nodosBloqueados = new HashSet<int>();

        foreach (var par in posicionesNodos)
        {
            Collider2D[] colisiones = Physics2D.OverlapCircleAll(par.Value, 0.25f);
            foreach (var col in colisiones)
            {
                if (col.CompareTag("muro"))
                {
                    nodosBloqueados.Add(par.Key);
                    break;
                }
            }
        }

        Dijkstra dijkstra = new Dijkstra(grafo, nodosBloqueados);
        ruta = dijkstra.CalcularCamino(origen, destino);
        indiceActual = 0;
    }

    public void TomarDañoZ(int daño)
    {
        vidaZombie -= daño;
        if (vidaZombie <= 0)
            Morir();
    }

    private void Morir()
    {
        gameObject.SetActive(false);
        spawner.colaDeZombies.Enqueue(gameObject);
        vidaZombie = 100; // o el valor original
        GameManager.Instance.SumarPuntos(50);
        GameManager.Instance.SumarMonedas(100);
    }
}
