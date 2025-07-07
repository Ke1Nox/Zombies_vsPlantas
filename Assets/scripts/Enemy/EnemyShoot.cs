using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 2f;
    public float stopXPosition = 0f; // Mitad del mapa (por ejemplo, en X=0)

    [Header("Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float minFireDelay = 1f; // Mínimo tiempo para disparar
    public float maxFireDelay = 3f; // Máximo tiempo para disparar
    public float chargeTime = 1f;   // Tiempo de "carga" antes de disparar

    private bool isCharging = false;
    private float nextFireTime = 0f;

    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    void HandleMovement()
    {
        if (!isCharging && transform.position.x > stopXPosition)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
    }

    void HandleShooting()
    {
        if (Time.time >= nextFireTime && !isCharging)
        {
            StartCoroutine(ChargeAndShoot());
        }
    }

    System.Collections.IEnumerator ChargeAndShoot()
    {
        isCharging = true;

        // Podés poner acá una animación de "cargando" si querés
        yield return new WaitForSeconds(chargeTime);

        Shoot();

        nextFireTime = Time.time + Random.Range(minFireDelay, maxFireDelay);
        isCharging = false;
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
