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
            // Intenta da�ar un zombie com�n
            var zombieComun = collision.GetComponent<zombie>();
            if (zombieComun != null)
            {
                zombieComun.TomarDa�oZ(25);
            }
            else
            {
                // Si no es com�n, prob� si es inteligente
                var zombieInteligente = collision.GetComponent<ZombieInteligente>();
                if (zombieInteligente != null)
                {
                    zombieInteligente.TomarDa�oZ(25);
                }
            }

            pool.DevolverBala(this);
        }
    }
}

