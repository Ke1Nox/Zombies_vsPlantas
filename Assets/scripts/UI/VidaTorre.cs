using TMPro;
using UnityEngine;
using System.Collections;

using TMPro;
using UnityEngine;

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
    }

    private void Update()
    {
        int vidaActual = GestorGlobal.VidaTorre;

        if (vidaActual != vidaAnterior)
        {
            ActualizarVida(vidaActual);
        }
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

        textMesh.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        textMesh.color = original;
        yield return new WaitForSeconds(0.05f);

        textMesh.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        textMesh.color = original;
    }
}






