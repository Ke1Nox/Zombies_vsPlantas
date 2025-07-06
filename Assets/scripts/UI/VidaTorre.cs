using TMPro;
using UnityEngine;
using System.Collections;

public class VidaTorre : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private int vidaAnterior;
    private Coroutine parpadeo;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        vidaAnterior = GestorGlobal.VidaTorre;
        textMesh.text = $"vida: {vidaAnterior}";

        if (GameManagerEndless.Instance != null)
            GameManagerEndless.Instance.OnDa�oTorre += ActualizarVida;
    }

    private void OnDestroy()
    {
        if (GameManagerEndless.Instance != null)
            GameManagerEndless.Instance.OnDa�oTorre -= ActualizarVida;
    }

    private void ActualizarVida(int nuevaVida)
    {
        textMesh.text = $"vida: {nuevaVida}";

        if (nuevaVida < vidaAnterior)
        {
            if (parpadeo != null) StopCoroutine(parpadeo);
            parpadeo = StartCoroutine(ParpadearRojo());
        }

        vidaAnterior = nuevaVida;
    }


    IEnumerator ParpadearRojo()
    {
        Color original = textMesh.color;

        // Hacemos un parpadeo m�s fuerte
        textMesh.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        textMesh.color = original;
        yield return new WaitForSeconds(0.05f);

        textMesh.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        textMesh.color = original;
    }
}





