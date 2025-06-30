using UnityEngine;

public class BalaPJ : MonoBehaviour
{
    [SerializeField] private float speed = -30f;
    private float lifeTime = 2f;
    private float currentTime;

    private BalaPool pool;  // Referencia al pool

    public void SetPool(BalaPool balaPool)
    {
        pool = balaPool;
    }

    void OnEnable()
    {
        currentTime = 0f;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        transform.Translate(speed * Time.deltaTime, 0, 0);

        if (currentTime >= lifeTime)
        {
            pool.DevolverBala(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("zombie"))
        {
            // Intenta dañar un zombie común
            var zombieComun = collision.GetComponent<zombie>();
            if (zombieComun != null)
            {
                zombieComun.TomarDañoZ(25);
            }
            else
            {
                // Si no es común, probá si es inteligente
                var zombieInteligente = collision.GetComponent<ZombieInteligente>();
                if (zombieInteligente != null)
                {
                    zombieInteligente.TomarDañoZ(25);
                }
            }

            pool.DevolverBala(this);
        }
    }
}

