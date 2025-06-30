using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombie : MonoBehaviour
{
    [SerializeField] int vidaZombie;
    [SerializeField] int maximoVidaZ = 100;
    public float vel = 5f;
    Rigidbody2D rb2D;
    [SerializeField] private float TiempoEntreDaño = 2f;
    private float TiempoSigienteDaño = 0;
    public int Vida => vidaZombie;

    private Spawner spawner;
    public Puntaje puntaje;

    private List<int> ruta;
    private Dictionary<int, Vector2> posiciones;
    private int indiceActual = 0;

    // NUEVO: ¿Es zombie inteligente?
    [SerializeField] bool esInteligente = false;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        vidaZombie = maximoVidaZ;
        spawner = Object.FindFirstObjectByType<Spawner>();
    }

    void FixedUpdate()
    {
        if (ruta != null && indiceActual < ruta.Count)
        {
            int nodoActual = ruta[indiceActual];
            Vector2 destino = posiciones[nodoActual];

            // ?? Detectar si hay un muro en el nodo siguiente
            Collider2D[] colisiones = Physics2D.OverlapCircleAll(destino, 0.2f);
            bool hayMuro = false;
            GameObject muroDetectado = null;

            foreach (Collider2D col in colisiones)
            {
                if (col.CompareTag("muro"))
                {
                    hayMuro = true;
                    muroDetectado = col.gameObject;
                    break;
                }
            }

            if (hayMuro)
            {
                if (esInteligente)
                {
                    Dijkstra dijkstra = new Dijkstra(spawner.grafo);

                    int nodoActual1 = ruta[indiceActual];
                    int nodosPorCarril = 28; // Asegurate que sea el mismo número que en Spawner
                    int cantidadCarriles = 3;
                    int destinoFinal = ruta[ruta.Count - 1];

                    for (int carril = 0; carril < cantidadCarriles; carril++)
                    {
                        int nodoDestino = 1 + carril * nodosPorCarril + (nodosPorCarril - 1); // último nodo del carril

                        if (nodoDestino == destinoFinal) continue;

                        List<int> nuevaRuta = dijkstra.CalcularCamino(nodoActual1, nodoDestino);

                        if (nuevaRuta.Count > 1)
                        {
                            ruta = nuevaRuta;
                            indiceActual = 0;
                            return;
                        }
                    }

                    return; // no encontró rutas ? se quedaa
                }
                else
                {
                    // Zombie común ataca el muro
                    if (TiempoSigienteDaño <= 0 && muroDetectado != null)
                    {
                        Muro muro = muroDetectado.GetComponent<Muro>();
                        if (muro != null)
                        {
                            muro.TomarDaño(20); // DAÑO APLICADO
                            TiempoSigienteDaño = TiempoEntreDaño;
                        }
                    }
                    else
                    {
                        TiempoSigienteDaño -= Time.deltaTime;
                    }
                    return;
                }
            }

            // Movimiento normal
            Vector2 direccion = (destino - (Vector2)transform.position).normalized;
            rb2D.MovePosition(rb2D.position + direccion * vel * Time.fixedDeltaTime);

            if (Vector2.Distance(transform.position, destino) < 0.1f)
            {
                indiceActual++;
            }

            if (indiceActual >= ruta.Count)
            {
                LlegarATorre();
            }
        }
    }

    private void LlegarATorre()
    {
        GameManager.Instance.DamageTower(vidaZombie);
        gameObject.SetActive(false);
        spawner.colaDeZombies.Enqueue(gameObject);
        vidaZombie = maximoVidaZ;
    }

    public void TomarDañoZ(int daño)
    {
        vidaZombie -= daño;
        if (vidaZombie <= 0) ZombieMuere();
    }

    public void SetSpawner(Spawner spawnerReference)
    {
        spawner = spawnerReference;
    }

    private void ZombieMuere()
    {
        gameObject.SetActive(false);
        spawner.colaDeZombies.Enqueue(gameObject);
        vidaZombie = maximoVidaZ;
        GameManager.Instance.SumarPuntos(50);
        GameManager.Instance.SumarMonedas(100);
    }

    public void SetRutaConGrafo(GrafoMA grafo, int origen, int destino, Dictionary<int, Vector2> posicionesNodos)
    {
        Dijkstra dijkstra = new Dijkstra(grafo);
        ruta = dijkstra.CalcularCamino(origen, destino);
        posiciones = posicionesNodos;
        indiceActual = 0;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("torre"))
        {
            TiempoSigienteDaño -= Time.deltaTime;
            GameManager.Instance.DamageTower(1);
        }
    }
}


