using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject prefabBullet;
    private float fireRate = 3f;
    private float currentTime;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime>=fireRate) 
        {
            Instantiate(prefabBullet, transform.position, transform.rotation);
            currentTime = 0;
        }
    }
}
