using System.Collections.Generic;
using UnityEngine;

public class ZombieInteligente : MonoBehaviour
{
    [SerializeField] private int vidaZombie = 100;
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float tiempoReintento = 1.5f;
    [SerializeField] private float tiempoEntreAtaques = 0.2f;
    [SerializeField] private int dañoAlTorre = 10;

    private float tiempoProximoIntento = 0f;
    private float timerAtaqueTorre = 0f;

    private bool atacandoTorre = false;
    private TorreScript torreActual;

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
        if (atacandoTorre && torreActual != null)
        {
            timerAtaqueTorre += Time.fixedDeltaTime;
            if (timerAtaqueTorre >= tiempoEntreAtaques)
            {
                torreActual.OnGetDamange?.Invoke(dañoAlTorre);
                timerAtaqueTorre = 0f;
            }
            return;
        }

        if (esperandoRuta)
        {
            if (Time.time >= tiempoProximoIntento)
            {
                tiempoProximoIntento = Time.time + tiempoReintento;
                RecalcularRutaEsquivando();
            }
            return;
        }

        if (ruta == null || indiceActual >= ruta.Count)
            return;

        int nodoActual = ruta[indiceActual];
        Vector2 destino = posiciones[nodoActual];

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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("torre"))
        {
            atacandoTorre = true;
            if (torreActual == null)
            {
                torreActual = other.GetComponent<TorreScript>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("torre"))
        {
            atacandoTorre = false;
            torreActual = null;
            timerAtaqueTorre = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("muro"))
        {
            Debug.Log("Entré en un muro. Recalculando ruta.");
            esperandoRuta = true;
        }
    }

    private void RecalcularRutaEsquivando()
    {
        Debug.Log("Entró a RecalcularRutaEsquivando()");

        HashSet<int> nodosBloqueados = new HashSet<int>();
        foreach (var par in posiciones)
        {
            Collider2D[] colisiones = Physics2D.OverlapCircleAll(par.Value, 0.7f);
            foreach (var col in colisiones)
            {
                if (col.GetComponent<Muro>() != null)
                {
                    nodosBloqueados.Add(par.Key);
                    break;
                }
            }
        }

        Dijkstra dijkstra = new Dijkstra(spawner.grafo, nodosBloqueados);

        int nodosPorCarril = spawner.nodosPorCarril;
        int carriles = spawner.cantidadCarriles;

        int nodoActual = ruta[indiceActual];
        int columnaX = (nodoActual - 1) / carriles;
        int carrilActual = (nodoActual - 1) % spawner.cantidadCarriles;

        List<int> ordenCarriles = new List<int>();
        if (carrilActual < carriles - 1) ordenCarriles.Add(carrilActual + 1); // abajo
        if (carrilActual > 0) ordenCarriles.Add(carrilActual - 1); // arriba

        foreach (int nuevoCarril in ordenCarriles)
        {
            int nuevoInicio = 1 + columnaX * spawner.cantidadCarriles + nuevoCarril;
            int destino = 1 + (spawner.nodosPorCarril - 1) * spawner.cantidadCarriles + nuevoCarril;

            if (nodosBloqueados.Contains(nuevoInicio) || nodosBloqueados.Contains(destino)) continue;

            List<int> nuevaRuta = dijkstra.CalcularCamino(nuevoInicio, destino);

            if (nuevaRuta.Count > 1)
            {
                bool rutaValida = true;
                foreach (int nodo in nuevaRuta)
                {
                    if (nodosBloqueados.Contains(nodo))
                    {
                        rutaValida = false;
                        break;
                    }
                }

                if (rutaValida)
                {
                    Debug.Log($"Ruta válida encontrada: {nuevaRuta[0]} -> {nuevaRuta[^1]}");
                    ruta = nuevaRuta;
                    posiciones = spawner.posicionesNodos;
                    indiceActual = 0;
                    esperandoRuta = false;
                    return;
                }
            }
        }

        Debug.Log("No se encontró ruta en el mismo X hacia arriba o abajo.");
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

        if (ruta != null && ruta.Count > 0 && posiciones.ContainsKey(ruta[0]))
        {
            transform.position = posiciones[ruta[0]];
        }
    }

    private void LlegarATorre()
    {
        GameManager.Instance.DamageTower(vidaZombie);
        ReiniciarZombie();
    }

    private void Morir()
    {
        ReiniciarZombie();
        vidaZombie = 100;
        GameManager.Instance.SumarPuntos(50);
        GameManager.Instance.SumarMonedas(100);
    }

    public void TomarDañoZ(int daño)
    {
        vidaZombie -= daño;
        if (vidaZombie <= 0)
            Morir();
    }

    private void ReiniciarZombie()
    {
        atacandoTorre = false;
        torreActual = null;
        timerAtaqueTorre = 0f;
        gameObject.SetActive(false);
        spawner.colaDeZombies.Enqueue(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (ruta != null && indiceActual < ruta.Count && spawner != null)
        {
            int nodoActual = ruta[indiceActual];
            Vector2 destino = spawner.posicionesNodos[nodoActual];

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(destino, 1.0f);
        }
    }
}

