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
    // Update is called once per frame
    void Update()
    {
        vida = GameManager.Instance.vidaTorre;

        textMesh.text = vida.ToString("0");
    }

    public void RestarVidaTorre(int puntosEntrada)
    {
        vida -= puntosEntrada;
    }


}
