using System.Collections.Generic;
using UnityEngine;

public class zombie : MonoBehaviour
{
    [SerializeField] int maximoVidaZ = 100;
    [SerializeField] private float TiempoEntreDaño = 2f;
    [SerializeField] private float vel = 5f;

    private int vidaZombie;
    public int Vida => vidaZombie;
    private float TiempoSigienteDaño = 0f;
    private GameObject muroAnterior;

    private Rigidbody2D rb2D;
    private Spawner spawner;

    private List<int> ruta;
    private Dictionary<int, Vector2> posiciones;
    private int indiceActual = 0;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        vidaZombie = maximoVidaZ;
        spawner = Object.FindFirstObjectByType<Spawner>();
    }

    void FixedUpdate()
    {
        if (ruta == null || indiceActual >= ruta.Count) return;

        int nodoActual = ruta[indiceActual];
        Vector2 destino = posiciones[nodoActual];

        // Detectar muro en destino
        Collider2D[] colisiones = Physics2D.OverlapCircleAll(destino, 0.25f);
        GameObject muroDetectado = null;

        foreach (var col in colisiones)
        {
            if (col.CompareTag("muro"))
            {
                muroDetectado = col.gameObject;
                break;
            }
        }

        if (muroDetectado != null)
        {
            // Atacar muro si es nuevo
            if (muroDetectado != muroAnterior)
            {
                TiempoSigienteDaño = 0f;
                muroAnterior = muroDetectado;
            }

            if (TiempoSigienteDaño <= 0f)
            {
                Muro muro = muroDetectado.GetComponent<Muro>();
                if (muro != null)
                {
                    muro.TomarDaño(40);
                    TiempoSigienteDaño = TiempoEntreDaño;
                }
            }
            else
            {
                TiempoSigienteDaño -= Time.deltaTime;
            }

            return;
        }

        // Movimiento normal
        Vector2 direccion = (destino - rb2D.position).normalized;
        rb2D.MovePosition(rb2D.position + direccion * vel * Time.fixedDeltaTime);

        if (Vector2.Distance(rb2D.position, destino) < 0.1f)
        {
            indiceActual++;
            if (indiceActual >= ruta.Count)
            {
                LlegarATorre();
            }
        }
    }

    private void LlegarATorre()
    {
        GameManager.Instance.DamageTower(vidaZombie);
        ReiniciarZombie();
    }

    public void TomarDañoZ(int daño)
    {
        vidaZombie -= daño;
        if (vidaZombie <= 0) ZombieMuere();
    }

    private void ZombieMuere()
    {
        GameManager.Instance.SumarPuntos(50);
        GameManager.Instance.SumarMonedas(100);
        ReiniciarZombie();
    }

    private void ReiniciarZombie()
    {
        gameObject.SetActive(false);
        vidaZombie = maximoVidaZ;
        spawner.colaDeZombies.Enqueue(gameObject);
    }

    public void SetSpawner(Spawner spawnerReference)
    {
        spawner = spawnerReference;
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("torre"))
        {
            TiempoSigienteDaño -= Time.deltaTime;
            GameManager.Instance.DamageTower(1);
        }
    }
}



