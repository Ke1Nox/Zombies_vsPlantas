using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VidaTorre : MonoBehaviour
{
    private int vida;
    private TextMeshProUGUI textMesh;


    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }
    public void TomarVida(int daño)
    {
        vida -= daño;
    }
    // Update is called once per frame
    void Update()
    {
        textMesh.text = vida.ToString("0");
    }

    public void RestarVidaTorre(int puntosEntrada)
    {
        vida -= puntosEntrada;
    }


}
