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

    private bool esperandoRuta = false;

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

        // Si está esperando una nueva ruta, seguir intentando
        if (esperandoRuta)
        {
            RecalcularRutaEsquivando();
            return;
        }

        // Detectar muro en el siguiente nodo
        Collider2D[] colisiones = Physics2D.OverlapCircleAll(destino, 0.25f);

        foreach (Collider2D col in colisiones)
        {
            Debug.Log($"Detectado tag: {col.tag} en nodo {ruta[indiceActual]}");
            if (col.CompareTag("muro"))
            {
                Debug.Log("Zombie inteligente detectó un muro. Intentando esquivar...");
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
        Debug.Log("Entró a RecalcularRutaEsquivando()");

        HashSet<int> nodosBloqueados = new HashSet<int>();
        foreach (var par in posiciones)
        {
            Collider2D[] colisiones = Physics2D.OverlapCircleAll(par.Value, 0.25f);
            foreach (var col in colisiones)
            {
                if (col.CompareTag("muro"))
                {
                    Debug.Log($"Nodo bloqueado: {par.Key} en posición {par.Value}");
                    nodosBloqueados.Add(par.Key);
                    break;
                }
            }
        }

        Dijkstra dijkstra = new Dijkstra(spawner.grafo, nodosBloqueados);

        int nodosPorCarril = spawner.nodosPorCarril;
        int carriles = spawner.cantidadCarriles;

        for (int c = 0; c < carriles; c++)
        {
            Debug.Log($"Probando cambio al carril {c}...");
            int nuevoInicio = -1;

            for (int i = 0; i < nodosPorCarril; i++)
            {
                int nodo = 1 + c * nodosPorCarril + i;
                if (!nodosBloqueados.Contains(nodo))
                {
                    nuevoInicio = nodo;
                    break;
                }
            }

            if (nuevoInicio == -1) continue;

            int destino = 1 + c * nodosPorCarril + (nodosPorCarril - 1);
            if (nodosBloqueados.Contains(destino)) continue;

            List<int> nuevaRuta = dijkstra.CalcularCamino(nuevoInicio, destino);

            if (nuevaRuta.Count > 1)
            {
                Debug.Log($"Ruta encontrada al carril {c} con {nuevaRuta.Count} nodos.");
                ruta = nuevaRuta;
                posiciones = spawner.posicionesNodos;
                indiceActual = 0;
                
                esperandoRuta = false;
                Debug.Log("Zombie inteligente: cambió de carril.");
                return;
            }
            Debug.Log("No se encontró ruta en ningún carril.");
            esperandoRuta = true;
          
        }

        
    }

    public void SetSpawner(Spawner spawnerReference)
    {
        spawner = spawnerReference;
    }

    public void SetRutaManual(List<int> ruta, Dictionary<int, Vector2> posiciones)
    {
        this.ruta = ruta;
        this.posiciones = posiciones;
        indiceActual = 0;
        esperandoRuta = false;
    }

    private void LlegarATorre()
    {
        GameManager.Instance.DamageTower(vidaZombie);
        gameObject.SetActive(false);
        spawner.colaDeZombies.Enqueue(gameObject);
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
        vidaZombie = 100;
        GameManager.Instance.SumarPuntos(50);
        GameManager.Instance.SumarMonedas(100);
    }
}

