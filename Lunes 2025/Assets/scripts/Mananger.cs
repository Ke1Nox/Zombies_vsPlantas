using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class Mananger : MonoBehaviour
{
    private int vidatorre;
    public TorreScript torreScript;
    public static float puntos;
    public GameObject turret;
    private bool Currentlevel = false;

    private void Start()
    {
        torreScript.OnGetDamange += CheckDead;
        turret.SetActive(false);
    }

    private void Update()
    {
        if (Currentlevel == false &&  puntos >= 500) {

            SceneManager.LoadScene(3);
            puntos = 0;
            Currentlevel = true;
        }
        if (Currentlevel == true && puntos >= 1000)
        {

            SceneManager.LoadScene(5);
            puntos = 0;
        }
    }

    private void CheckDead(int vida)
    {
        if (vida <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }

    //botones
    public void BotonFinVolver()
    {
        SceneManager.LoadScene(1);
    }

    public void BotonFinAMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void BotonMenuJugar()
    {
        SceneManager.LoadScene(1);
    }

    public void BotonWinJugar()
    {
        SceneManager.LoadScene(4);
        
    }

    public void BotonMenuSalir()
    {
        Debug.Log("SALIR");
        Application.Quit();
    }


}
