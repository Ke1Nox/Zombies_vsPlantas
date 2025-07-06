using TMPro;
using UnityEngine;

public class OleadaUI : MonoBehaviour
{
    private TextMeshProUGUI textoOleada;

    private void Start()
    {
        textoOleada = GetComponent<TextMeshProUGUI>();
        ActualizarTexto(); // Mostrar oleada al iniciar
        GameManagerEndless.Instance.OnNuevaOleada += ActualizarTexto;
    }

    private void OnDestroy()
    {
        if (GameManagerEndless.Instance != null)
            GameManagerEndless.Instance.OnNuevaOleada -= ActualizarTexto;
    }

    private void ActualizarTexto()
    {
        textoOleada.text = "Oleada " + GameManagerEndless.Instance.NumeroOleada;
    }
}
