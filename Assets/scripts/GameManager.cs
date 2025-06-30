using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int vidaTorre = 1000;
    public float puntos = 0;

    public int monedas = 0; 
    public TextMeshProUGUI textoMonedas; // << Opcional, asignar desde el editor

    public int currentLevel = 1; // 1 = primer nivel, 2 = segundo nivel

    public TorreScript torreScript;
    public GameObject turret;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (torreScript != null)
        {
            torreScript.OnGetDamange += DamageTower;
        }
       
        ActualizarUI(); // << Actualiza UI al iniciar
    }

    private void Update()
    {
        CheckLevelProgress();
    }

    void CheckLevelProgress()
    {
        if (currentLevel == 1 && puntos >= 500)
        {
           LoadScene(2);// Ganas Nivel 1 → Win1
        }
        else if (currentLevel == 2 && puntos >= 500)
        {
            LoadScene(4); // Ganas Nivel 2 → Win2 (Victoria Final)
        }
    }

    void LoadScene(int sceneIndex)
    {
        puntos = 0;
        SceneManager.LoadScene(sceneIndex);
    }

    public void DamageTower(int daño)
    {
        vidaTorre -= daño;
        if (vidaTorre <= 0)
        {
            SceneManager.LoadScene(5); // Escena Fail (Game Over)
        }
    }

    public void SumarPuntos(float cantidad)
    {
        puntos += cantidad;
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

    void ActualizarUI()
    {
        if (textoMonedas != null)
            textoMonedas.text = "Monedas: " + monedas;
    }

    public void ReiniciarNivel()
    {
        if (currentLevel == 1)
        {
            SceneManager.LoadScene(1); // Nivel 1: "juego"
        }
        else if (currentLevel == 2)
        {
            SceneManager.LoadScene(3); // Nivel 2: "lvl2"
        }

        // Reiniciar valores si querés
        vidaTorre = 1000;
        puntos = 0;
    }
}


