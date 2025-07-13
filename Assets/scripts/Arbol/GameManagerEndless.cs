using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

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

    public int EnemigosVivos => enemigosVivos;
    private int enemigosVivos = 0;
    private bool esperandoSiguienteOleada = false;

    public int vidaTorre = 1000;
    public TorreScript torreScript;

    public event System.Action<int> OnDañoTorre;
    public event System.Action OnNuevaOleada;
    public int NumeroOleada => numeroOleada;

    private Spawner spawner;

    private string pathRanking;
    private string nombreJugador => PlayerPrefs.GetString("nombreJugador", "Anonimo");

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        Debug.Log("Nombre cargado: " + nombreJugador);
        torreScript = Object.FindFirstObjectByType<TorreScript>();
        spawner = Object.FindFirstObjectByType<Spawner>();
        pathRanking = Application.persistentDataPath + "/ranking.json";

        CargarRanking();

        ActualizarUI();
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
        enemigosVivos = enemigosEnEstaOleada;
        esperandoSiguienteOleada = false;

        if (spawner != null)
        {
            spawner.SpawnearZombies(enemigosEnEstaOleada);
        }
    }

    public void SumarPuntos(int puntos)
    {
        puntuacionActual += puntos;
    }

    public void EnemigoDerrotado()
    {
        enemigosVivos--;

        if (enemigosVivos <= 0 && !esperandoSiguienteOleada)
        {
            esperandoSiguienteOleada = true;
            Invoke(nameof(FinalizarOleada), 0.5f); // esperamos medio segundo antes de iniciar la próxima
        }
    }
    private void FinalizarOleada()
    {
        // Guardamos el puntaje solo al final de la oleada
        PuntajeJugador nuevo = new PuntajeJugador(nombreJugador, puntuacionActual);
        arbol.Insertar(nuevo);
        GuardarRanking();

        Debug.Log($"Oleada {numeroOleada} completada. Puntaje registrado: {puntuacionActual}");

        Invoke(nameof(IniciarSiguienteOleada), tiempoEntreOleadas);
    }


    private void GuardarRanking()
    {
        List<PuntajeJugador> top = arbol.ObtenerTop(10);
        string json = JsonUtility.ToJson(new ContenedorRanking(top));
        File.WriteAllText(pathRanking, json);
    }

    private void CargarRanking()
    {
        if (File.Exists(pathRanking))
        {
            string json = File.ReadAllText(pathRanking);
            ContenedorRanking datos = JsonUtility.FromJson<ContenedorRanking>(json);
            foreach (var p in datos.ranking)
                arbol.Insertar(p);
        }
    }

    public void DamageTower(int daño)
    {
        OnDañoTorre?.Invoke(vidaTorre);

        if (vidaTorre <= 0) return;

        vidaTorre -= daño;

        if (vidaTorre <= 0)
        {
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

    // Extra: obtener el top para mostrar en UI
    public List<PuntajeJugador> ObtenerTopRanking(int cantidad)
    {
        return arbol.ObtenerTop(cantidad);
    }

  

}


