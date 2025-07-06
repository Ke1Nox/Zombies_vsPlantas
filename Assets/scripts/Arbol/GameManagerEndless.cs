using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerEndless : MonoBehaviour
{
    public static GameManagerEndless Instance;

    private ArbolPuntajes arbol = new ArbolPuntajes();
    public int puntuacionActual = 0;

    private int numeroOleada = 0;
    public int enemigosPorOleadaInicial = 2;
    public float tiempoEntreOleadas = 3f;
    public int monedas = 0;
    public TextMeshProUGUI textoMonedas;


    private int enemigosVivos = 0;
    private bool esperandoSiguienteOleada = false;

    public int vidaTorre = 1000;
    public TorreScript torreScript;


    public event System.Action<int> OnDañoTorre;

    public event System.Action OnNuevaOleada;
    public int NumeroOleada => numeroOleada;

    private Spawner spawner;

    private string pathRecord;
    private int mayorPuntajeGuardado = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        torreScript = Object.FindFirstObjectByType<TorreScript>();
        spawner = Object.FindFirstObjectByType<Spawner>(); // buscamos el spawner activo en la escena
        pathRecord = Application.persistentDataPath + "/record.txt";
        CargarPuntajeMaximo();
        Debug.Log("Record actual: " + mayorPuntajeGuardado);
        Debug.Log("Ruta del record.txt: " + Application.persistentDataPath);
        ;


        ActualizarUI(); // Muestra el valor inicial de monedas al iniciar
        IniciarSiguienteOleada();
    }

    private void Update()
    {
        if (enemigosVivos <= 0 && !esperandoSiguienteOleada)
        {
            esperandoSiguienteOleada = true;
            Invoke(nameof(IniciarSiguienteOleada), tiempoEntreOleadas);
        }
    }

    private void IniciarSiguienteOleada()
    {
        numeroOleada++;

        OnNuevaOleada?.Invoke();

        int enemigosEnEstaOleada = enemigosPorOleadaInicial + (numeroOleada - 1) * 2;

        Debug.Log("OLEADA " + numeroOleada + " - Enemigos: " + enemigosEnEstaOleada);
        esperandoSiguienteOleada = false;

        enemigosVivos = enemigosEnEstaOleada;

        //  Le pedimos al spawner que los genere
        if (spawner != null)
        {
            spawner.SpawnearZombies(enemigosEnEstaOleada);
        }
        else
        {
            Debug.LogError("Spawner no encontrado en la escena.");
        }
    }

    public void SumarPuntos(int puntos)
    {
        puntuacionActual += puntos;

        if (puntuacionActual > mayorPuntajeGuardado)
        {
            mayorPuntajeGuardado = puntuacionActual;
            GuardarPuntajeMaximo();
            Debug.Log("Nuevo récord guardado: " + mayorPuntajeGuardado);
        }
    }

    public void EnemigoDerrotado()
    {
        enemigosVivos--;

        if (enemigosVivos <= 0)
        {
            arbol.Insertar(puntuacionActual);
            Debug.Log("Oleada completada. Puntaje actual: " + puntuacionActual + " | Mayor en memoria: " + arbol.ObtenerMaximo());

            if (puntuacionActual > mayorPuntajeGuardado)
            {
                mayorPuntajeGuardado = puntuacionActual;
                GuardarPuntajeMaximo();
                Debug.Log("Nuevo récord guardado: " + mayorPuntajeGuardado);
            }
        }
    }

    private void GuardarPuntajeMaximo()
    {
        try
        {
            System.IO.File.WriteAllText(pathRecord, mayorPuntajeGuardado.ToString());
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al guardar el récord: " + e.Message);
        }
    }

    private void CargarPuntajeMaximo()
    {
        try
        {
            if (System.IO.File.Exists(pathRecord))
            {
                string contenido = System.IO.File.ReadAllText(pathRecord);
                if (int.TryParse(contenido, out int valor))
                {
                    mayorPuntajeGuardado = valor;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al cargar el récord: " + e.Message);
        }
    }


    public void DamageTower(int daño)
    {
        OnDañoTorre?.Invoke(vidaTorre);

        if (vidaTorre <= 0) return;

        vidaTorre -= daño;
        Debug.Log("Torre recibió " + daño + " de daño. Vida restante: " + vidaTorre);

        if (vidaTorre <= 0)
        {
            Debug.Log("¡La torre fue destruida! Cargando escena de derrota...");
            SceneManager.LoadScene(4);
        }
    }



    public void SumarMonedas(int cantidad)
    {
        monedas += cantidad;
        ActualizarUI();
    }

    public bool GastarMonedas(int cantidad)
    {
        if (monedas >= cantidad)
        {
            monedas -= cantidad;
            ActualizarUI();
            return true;
        }
        return false;
    }

    private void ActualizarUI()
    {
        if (textoMonedas != null)
            textoMonedas.text = "Monedas: " + monedas;
    }

}

