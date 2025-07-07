using System.Collections.Generic;
using UnityEngine;

public class zombie : MonoBehaviour
{
    [SerializeField] int maximoVidaZ = 100;
    [SerializeField] private float TiempoEntreDaño = 2f;
    [SerializeField] private float vel = 5f;

    private Animator animator;
    private bool atacandoTorre = false;
    private int vidaZombie;
    public int Vida => vidaZombie;

    private float TiempoSigienteDaño = 0f;
    private GameObject muroAnterior;

    private Rigidbody2D rb2D;
    private Spawner spawner;

    private List<int> ruta;
    private Dictionary<int, Vector2> posiciones;
    private int indiceActual = 0;

    private TorreScript torreActual;
    private float timerAtaqueTorre = 0f;

    [SerializeField] private float tiempoEntreAtaques = 1.0f;
    [SerializeField] private int dañoAlTorre = 10;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        vidaZombie = maximoVidaZ;
        spawner = Object.FindFirstObjectByType<Spawner>();
        animator = GetComponent<Animator>();

    }

    void FixedUpdate()
    {
        if (atacandoTorre && torreActual != null)
        {
            animator.SetBool("atacando", true); // está atacando la torre
            timerAtaqueTorre += Time.fixedDeltaTime;
            if (timerAtaqueTorre >= tiempoEntreAtaques)
            {
                GestorGlobal.DamageTower(dañoAlTorre);
                timerAtaqueTorre = 0f;
            }
            return;
        }

        if (ruta == null || indiceActual >= ruta.Count) return;

        int nodoActual = ruta[indiceActual];
        Vector2 destino = posiciones[nodoActual];

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
            animator.SetBool("atacando", true);
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
            // SIEMPRE que hay muroDetectado, estamos atacando visualmente
            animator.SetBool("atacando", true);
            return;
        }
      

        Vector2 nuevaPos = Vector2.MoveTowards(rb2D.position, destino, vel * Time.fixedDeltaTime);
        rb2D.MovePosition(nuevaPos);

        if (nuevaPos == destino)
        {
            indiceActual++;
            if (indiceActual >= ruta.Count)
            {
                LlegarATorre();
            }
        }
        animator.SetBool("atacando", false);

    }

    public void SetRutaManual(List<int> ruta, Dictionary<int, Vector2> posiciones)
    {
        this.ruta = ruta;
        this.posiciones = posiciones;
        indiceActual = 0;
    }

    private void LlegarATorre()
    {
        GestorGlobal.DamageTower(vidaZombie);
        ReiniciarZombie();
    }

    public void TomarDañoZ(int daño)
    {
        vidaZombie -= daño;
        if (vidaZombie <= 0) ZombieMuere();
    }

    public void SetVelocidadBase(float nuevaVelocidad)
    {
        vel = nuevaVelocidad;
    }

    private void ZombieMuere()
    {
        GestorGlobal.SumarPuntos(50);
        GestorGlobal.SumarMonedas(100);
        ReiniciarZombie();
        GestorGlobal.EnemigoDerrotado();
    }

    private void ReiniciarZombie()
    {
        animator.SetBool("atacando", false);

        atacandoTorre = false;
        vidaZombie = maximoVidaZ;
        ruta = null;
        indiceActual = 0;
        gameObject.SetActive(false);
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
            atacandoTorre = true;
            if (torreActual == null)
            {
                torreActual = other.GetComponent<TorreScript>();
            }
        }
    }

    public void AumentarVelocidad(float extra)
    {
        vel += extra;
    }
}





