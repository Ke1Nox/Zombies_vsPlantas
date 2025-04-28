using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretHD : MonoBehaviour
{
    [SerializeField] GameObject zombie;
    [SerializeField] float range;
    Rigidbody2D rb;
    Vector3 zombieposition;
    [SerializeField] private GameObject gun;
    public GameObject prefabBullet;
    [SerializeField] private float fireRate = 1f;
    private float currentTime;
    public GameObject shootPoint;
    [SerializeField] LayerMask layers;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, range, layers);

        foreach (var collider in cols)
        {
            zombieposition = collider.transform.position;
            gun.transform.up = gun.transform.position - zombieposition;
            if (currentTime >= fireRate)
            {

                Instantiate(prefabBullet, shootPoint.transform.position, shootPoint.transform.rotation);
                currentTime = 0;
            }

        }


    }

}




