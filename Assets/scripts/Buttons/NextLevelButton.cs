using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class NextLevelButton : MonoBehaviour
{
    [Header("UI Selector de Modo")]
    public GameObject panelModoSelector;
    public TextMeshProUGUI textoRecordEndless;

    private string pathRecord;

    private void Start()
    {
        if (panelModoSelector != null)
            panelModoSelector.SetActive(false);
    }


    // BOTONES NORMALES

    public void IrAlSiguienteNivel()
    {
        SceneManager.LoadScene(3); // lvl2
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentLevel = 2;
        }
    }

    public void Lvl1()
    {
        SceneManager.LoadScene(1);
    }

    public void VolverAlMenuWin()
    {
        SceneManager.LoadScene(0);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentLevel = 1;
            GameManager.Instance.puntos = 0;
            GameManager.Instance.vidaTorre = 1000;
        }
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene(0);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentLevel = 1;
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

    // NUEVOS BOTONES

    public void MostrarSelectorDeModo()
    {
        if (panelModoSelector != null)
            panelModoSelector.SetActive(true);
    }

    public void CargarModoNormal()
    {
        SceneManager.LoadScene(1); // juego
    }

    public void CargarHowToPlay()
    {
        SceneManager.LoadScene(6); // juego
    }

    public void CargarNivel2()
    {
        SceneManager.LoadScene(7); // juego
    }

    public void CargarModoEndless()
    {
        SceneManager.LoadScene("ModoEndless"); 
    }

    public void CerrarSelectorDeModo()
    {
        if (panelModoSelector != null)
            panelModoSelector.SetActive(false);
    }
}
