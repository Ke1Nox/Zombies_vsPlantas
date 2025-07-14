using Assets.scripts.TorretaEspecial;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TorreEspecial : MonoBehaviour
{
    public float tiempoVida = 10f;
    public float tiempoDisparo = 1.5f;
    public float rango = 10f;
    public GameObject balaPrefab;
    public Transform puntoDisparo;
    [SerializeField] float velBala;
    [SerializeField] float DañoBala;
    private BalaTorreEspecial bala;



    private float tiempoDisparoActual;

    void Start()
    {
        bala = new BalaTorreEspecial();
        Destroy(gameObject, tiempoVida); // Se destruye sola
    }

    void Update()
    {
        tiempoDisparoActual -= Time.deltaTime;

        if (tiempoDisparoActual <= 0)
        {
            Atacar();
            tiempoDisparoActual = tiempoDisparo;
        }
    }

    void Atacar()
    {
        List<IDañoable> enemigos = ObtenerEnemigosEnRango();

        if (enemigos.Count == 0) return;

        QuickSort(enemigos, 0, enemigos.Count - 1);

        IDañoable objetivo = enemigos[0]; // El de menor vida
        Vector3 dir = (objetivo.transform.position - transform.position).normalized;

        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
        bala.GetComponent<BalaTorreEspecial>()?.SetDaño(DañoBala);

        Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = dir * velBala;
        }
    }


    List<IDañoable> ObtenerEnemigosEnRango()
    {
        List<IDañoable> enRango = new List<IDañoable>();

        IDañoable[] enemigos = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IDañoable>()
            .ToArray();

        foreach (IDañoable enemigo in enemigos)
        {
            if (Vector3.Distance(transform.position, enemigo.transform.position) <= rango)
            {
                enRango.Add(enemigo);
            }
        }

        return enRango;
    }


    void QuickSort(List<IDañoable> lista, int izquierda, int derecha)
    {
        if (izquierda >= derecha) return;

        int pivote = Particionar(lista, izquierda, derecha);

        QuickSort(lista, izquierda, pivote - 1);
        QuickSort(lista, pivote + 1, derecha);
    }

    int Particionar(List<IDañoable> lista, int izquierda, int derecha)
    {
        float pivoteValor = lista[derecha].Vida;
        int i = izquierda - 1;

        for (int j = izquierda; j < derecha; j++)
        {
            if (lista[j].Vida < pivoteValor)
            {
                i++;
                (lista[i], lista[j]) = (lista[j], lista[i]);
            }
        }

        (lista[i + 1], lista[derecha]) = (lista[derecha], lista[i + 1]);
        return i + 1;
    }

}
