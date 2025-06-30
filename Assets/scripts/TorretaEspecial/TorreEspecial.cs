using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorreEspecial : MonoBehaviour
{
    public float tiempoVida = 10f;
    public float tiempoDisparo = 1.5f;
    public float rango = 10f;
    public GameObject balaPrefab;
    public Transform puntoDisparo;
    [SerializeField] float velBala;

    private float tiempoDisparoActual;

    void Start()
    {
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
        List<zombie> enemigos = ObtenerEnemigosEnRango();

        if (enemigos.Count == 0) return;

        QuickSort(enemigos, 0, enemigos.Count - 1);

        zombie objetivo = enemigos[0]; // M�s d�bil (menos vida)
        Vector3 dir = (objetivo.transform.position - transform.position).normalized;

        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
        Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = dir * velBala;
        }
        else
        {
            Debug.LogWarning("La bala no tiene Rigidbody2D asignado.");
        }

    }

    List<zombie> ObtenerEnemigosEnRango()
    {
        zombie[] todos = Object.FindObjectsByType<zombie>(FindObjectsSortMode.None);

        List<zombie> enRango = new List<zombie>();

        foreach (zombie z in todos)
        {
            if (Vector3.Distance(transform.position, z.transform.position) <= rango)
            {
                enRango.Add(z);
            }
        }

        return enRango;
    }

    void QuickSort(List<zombie> lista, int izquierda, int derecha)
    {
        if (izquierda >= derecha) return;

        int pivote = Particionar(lista, izquierda, derecha);

        QuickSort(lista, izquierda, pivote - 1);
        QuickSort(lista, pivote + 1, derecha);
    }

    int Particionar(List<zombie> lista, int izquierda, int derecha)
    {
        float pivoteValor = lista[derecha].Vida; // menor vida
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
