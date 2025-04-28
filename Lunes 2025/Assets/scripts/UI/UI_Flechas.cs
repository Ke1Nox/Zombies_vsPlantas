using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_Flechas : MonoBehaviour
{
    private int NumFlechas;
    private TextMeshProUGUI textMesh;



    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();

    }

  
    void Update()
    {
        NumFlechas = 2 -BalaPool.activeBalas;
        textMesh.text = NumFlechas.ToString("0");
    }
}
