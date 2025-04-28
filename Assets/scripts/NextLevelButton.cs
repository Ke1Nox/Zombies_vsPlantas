using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    public void IrAlSiguienteNivel()
    {
        SceneManager.LoadScene(3); // lvl2 es la escena 4
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentLevel = 2; // Cambiamos el nivel actual en el GameManager
        }
    }
    public void Lvl1()
    {
        SceneManager.LoadScene(1); // nivel 1

    }
    public void VolverAlMenuWin()
    {
        SceneManager.LoadScene(0); // Menu

        
            GameManager.Instance.currentLevel = 1;
            GameManager.Instance.puntos = 0;
            GameManager.Instance.vidaTorre = 1000;

        
    }
    public void VolverAlMenu()
    {
        SceneManager.LoadScene(0); 
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentLevel = 1; // Cambiamos el nivel actual en el GameManager
        }
    }
    public void Reiniciar()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReiniciarNivel();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}

