using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  turretScript : MonoBehaviour
{
    public float Range;
    public Transform target;
    bool Detected= false;
    Vector2 Direction;
    private string mensaje="AAAAAAA";
    //public GameObject prefabBullet;

    void Start()
    {
        
    }


    void Update()
    {
        Vector2 targetPos = target.position;

        Direction=targetPos- (Vector2)transform.position;

        RaycastHit2D  rayInfo = Physics2D.Raycast(transform.position,Direction,Range);
        if (rayInfo)
        {
            if (rayInfo.collider.gameObject.tag =="zombie")
            {
                if (Detected == false)
                {
                    Detected = true;
                    Debug.Log(mensaje);


                }
            }
            else
            {
                if (Detected == true)
                {
                    Detected = false;
                }
            }
        }   
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }








}
