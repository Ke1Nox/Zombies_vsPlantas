using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompradorDeTorre : MonoBehaviour
{
    public GameObject prefabTorreEspecial;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // o el botón que quieras
        {
            if (GameManager.Instance.GastarMonedas(50))
            {
                Instantiate(prefabTorreEspecial, transform.position, Quaternion.identity);
                Debug.Log("spáwn torre");
            }
            else
            {
                Debug.Log("No hay suficientes monedas");
            }
        }
    }
}
