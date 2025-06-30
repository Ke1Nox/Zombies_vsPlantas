using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int vidaTorre = 1000;
    public float puntos = 0;
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
    }

    private void Update()
    {
        CheckLevelProgress();
    }

    void CheckLevelProgress()
    {
        if (currentLevel == 1 && puntos >= 50)
        {
           LoadScene(2);// Ganas Nivel 1 → Win1
        }
        else if (currentLevel == 2 && puntos >= 50)
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


