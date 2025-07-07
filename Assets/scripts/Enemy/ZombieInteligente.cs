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
                GestorGlobal.DamageTower(dañoAlTorre);
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

        Vector2 nuevaPos = Vector2.MoveTowards(rb2D.position, destino, velocidad * Time.fixedDeltaTime);
        rb2D.MovePosition(nuevaPos);

        if (nuevaPos == destino)
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
            esperandoRuta = true;
        }
    }

    private void RecalcularRutaEsquivando()
    {
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

        int carriles = spawner.cantidadCarriles;
        int nodoActual = ruta[indiceActual];
        int columnaX = (nodoActual - 1) / carriles;
        int carrilActual = (nodoActual - 1) % carriles;

        List<int> ordenCarriles = new List<int>();
        if (carrilActual < carriles - 1) ordenCarriles.Add(carrilActual + 1);
        if (carrilActual > 0) ordenCarriles.Add(carrilActual - 1);

        foreach (int nuevoCarril in ordenCarriles)
        {
            int nuevoInicio = 1 + columnaX * carriles + nuevoCarril;
            int destino = 1 + (spawner.nodosPorCarril - 1) * carriles + nuevoCarril;

            if (nodosBloqueados.Contains(nuevoInicio) || nodosBloqueados.Contains(destino)) continue;

            List<int> nuevaRuta = dijkstra.CalcularCamino(nuevoInicio, destino);

            if (nuevaRuta.Count > 1 && nuevaRuta.TrueForAll(n => !nodosBloqueados.Contains(n)))
            {
                ruta = nuevaRuta;
                posiciones = spawner.posicionesNodos;
                indiceActual = 0;
                esperandoRuta = false;
                return;
            }
        }
    }

    public void SetSpawner(Spawner spawnerReference) => spawner = spawnerReference;

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
    public void SetVelocidadBase(float nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
    }

    private void LlegarATorre()
    {
        GestorGlobal.DamageTower(vidaZombie);
        ReiniciarZombie();
    }

    private void Morir()
    {
        ReiniciarZombie();
        vidaZombie = 100;
        GestorGlobal.SumarPuntos(50);
        GestorGlobal.SumarMonedas(100);
        GestorGlobal.EnemigoDerrotado();
    }

    public void TomarDañoZ(int daño)
    {
        vidaZombie -= daño;
        if (vidaZombie <= 0) Morir();
    }

    private void ReiniciarZombie()
    {
        atacandoTorre = false;
        torreActual = null;
        timerAtaqueTorre = 0f;
        gameObject.SetActive(false);
        spawner.colaDeZombies.Enqueue(gameObject);
    }

    public void AumentarVelocidad(float extra)
    {
        velocidad += extra;
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

