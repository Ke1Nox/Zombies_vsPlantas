using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class vidaatorre : MonoBehaviour
{
    private int vidaa;
    private TextMeshProUGUI textMesh;


    public TorreScript torreScript;


    private void Start()
    {
        

        textMesh = GetComponent<TextMeshProUGUI>();

    }


    private void Update()
    {

        vidaa= torreScript.vida;
        textMesh.text = vidaa.ToString("0");
    }
}
/*
    public void RestarVidaTorreText(int vidaentrada)
    {
        vidaa -= vidaentrada;

    }

}
*/
/*
public void RestarVidaTexto(int VidaTomadaTxt)
{
    vidaa -= VidaTomadaTxt;

}

}
*/
//vidaatorre