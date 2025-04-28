using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    public void IrAlSiguienteNivel()
    {
        SceneManager.LoadScene(4); // lvl2 es la escena 4
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentLevel = 2; // Cambiamos el nivel actual en el GameManager
        }
    }
    public void Lvl1()
    {
        SceneManager.LoadScene(1); // nivel 1

        
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene(0); // lvl2 es la escena 4
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentLevel = 1; // Cambiamos el nivel actual en el GameManager
        }
    }
}

