using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armaPJ : MonoBehaviour
{
    private Vector3 objetivo;
    public Camera camara;

    public GameObject shootPoint;

    public BalaPool balaPool;

    private bool puedeDisparar = true;
    private bool enRecarga = false;
    private float tiempoRecarga = 1f; // mismo que en BalaPool
    private float tiempoRecargando = 0f;

    // animacion
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0) && puedeDisparar && !enRecarga)
        {
            // Controlar la dirección del disparo
            objetivo = camara.ScreenToWorldPoint(Input.mousePosition);
            float grados = Mathf.Atan2(objetivo.y - transform.position.y, objetivo.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, grados);

            // Intentar disparar
            BalaPJ bala = balaPool.ObtenerBala();
            if (bala != null)
            {
                bala.transform.position = shootPoint.transform.position;
                bala.transform.rotation = shootPoint.transform.rotation;

                // Activamos la animación de disparo solo si se disparó
                animator.SetBool("disparo", true);
            }


            else
            {
                Debug.Log("sin balas: esperando a recargar.");
                puedeDisparar = false;
                enRecarga = true;
                tiempoRecargando += 1f; // Empezamos a contar
            }
        }

        else
        {
            // Desactivamos la animación si no estamos disparando
            animator.SetBool("disparo", false);
        }

        if (enRecarga)
        {
            tiempoRecargando += Time.deltaTime;
            if (tiempoRecargando >= tiempoRecarga)
            {
                Debug.Log("Recarga completa.");
                puedeDisparar = true;
                enRecarga = false;
            }
        }
    }
}

