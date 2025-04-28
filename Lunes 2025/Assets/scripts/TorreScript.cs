using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class TorreScript : MonoBehaviour
{
    public Action<int> OnGetDamange;
    public int vida = 3000;
    [SerializeField] int vidaMax = 3000;

    private TextMeshProUGUI textMesh;

    void Start()
    {
        vida = vidaMax;

    }


    private void Update()
    {
        
    }

        private void OnCollisionEnter2D(Collision2D other){
        if (other.collider.CompareTag("zombie"))
        {
            Debug.Log("zombie entro");
        }
    }



    public void TomarDañoT(int daño)
    {
        vida -= daño;
        OnGetDamange?.Invoke(vida);
        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }
}
