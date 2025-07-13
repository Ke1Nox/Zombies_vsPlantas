using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;

public class MostrarRankingUI : MonoBehaviour
{
    public TextMeshProUGUI textoRanking;
    public int cantidadTop = 5;
    public TextMeshProUGUI textoRecordEndless;

    private void Start()
    {
        string pathRanking = Application.persistentDataPath + "/ranking.json";

        if (!File.Exists(pathRanking))
        {
            textoRanking.text = "No hay ranking aún.";
            return;
        }

        string json = File.ReadAllText(pathRanking);
        ContenedorRanking datos = JsonUtility.FromJson<ContenedorRanking>(json);


        if (datos.ranking == null || datos.ranking.Count == 0)
        {
            textoRanking.text = "No hay puntajes guardados.";
            return;
        }

        textoRanking.text = "TOP ENDLESS:\n";

        // Ordenar por puntaje descendente
        datos.ranking.Sort((a, b) => b.puntaje.CompareTo(a.puntaje));

        int cantidadMostrar = Mathf.Min(cantidadTop, datos.ranking.Count);
        for (int i = 0; i < cantidadMostrar; i++)
        {
            textoRanking.text += $"{i + 1}. {datos.ranking[i].nombre}: {datos.ranking[i].puntaje}\n";
        }

        if (datos.ranking != null && datos.ranking.Count > 0)
        {
            datos.ranking.Sort((a, b) => b.puntaje.CompareTo(a.puntaje));
            var mejor = datos.ranking[0];

            textoRecordEndless.text = $"RÉCORD ENDLESS: {mejor.puntaje} PUNTOS ({mejor.nombre})";
        }
        else
        {
            textoRecordEndless.text = "RÉCORD ENDLESS: Sin registros aún.";
        }

    }
}

