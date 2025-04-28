using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Torre"))
        {
            GameManager.Instance.DamageTower(damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall")) // Para que no siga volando para siempre
        {
            Destroy(gameObject);
        }
    }
}
