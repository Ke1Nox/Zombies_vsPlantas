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
            torreScript.OnGetDamange += CheckDead;

        if (turret != null)
            turret.SetActive(false);
    }

    private void Update()
    {
        CheckLevelProgress();
    }

    void CheckLevelProgress()
    {
        if (currentLevel == 1 && puntos >= 50)
        {
           LoadScene(3);// Ganas Nivel 1 → Win1
        }
        else if (currentLevel == 2 && puntos >= 1000)
        {
            LoadScene(5); // Ganas Nivel 2 → Win2 (Victoria Final)
        }
    }

    void LoadScene(int sceneIndex)
    {
        puntos = 0;
        SceneManager.LoadScene(sceneIndex);
    }

    public void CheckDead(int daño)
    {
        vidaTorre -= 10;
        if (vidaTorre <= 0)
        {
            SceneManager.LoadScene(2); // Escena Fail (Game Over)
        }
    }

    public void SumarPuntos(float cantidad)
    {
        puntos += cantidad;
    }
}


