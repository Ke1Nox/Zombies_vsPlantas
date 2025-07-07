using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Flechas : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        int maxFlechas = 5; // Igual al valor en BalaPool
        int flechasUsadas = BalaPool.activeBalas;
        int flechasDisponibles = Mathf.Clamp(maxFlechas - flechasUsadas, 0, maxFlechas);

        textMesh.text = $"Flechas: {flechasDisponibles}/{maxFlechas}";
    }
}

