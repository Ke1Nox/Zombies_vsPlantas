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
        if (esperandoRuta)
        {
            RecalcularRutaEsquivando();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("muro"))
        {
            Debug.Log("Colisioné físicamente con un muro. Activando recálculo de ruta.");
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

        int carrilActual = (nodoActual - 1) / nodosPorCarril;

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



        esperandoRuta = true;
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

        // Reposicionar al zombie en el primer nodo si es válido
        if (ruta != null && ruta.Count > 0 && posiciones.ContainsKey(ruta[0]))
        {
            transform.position = posiciones[ruta[0]];
        }
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
