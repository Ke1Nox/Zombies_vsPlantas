using UnityEngine;
using System.Collections;

public class BalaPJ : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    private float initialSpeed;
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
        speed = initialSpeed = 100f; // Reiniciar velocidad
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
            // Da�o a zombie com�n
            var zombieComun = collision.GetComponent<zombie>();
            if (zombieComun != null)
            {
                zombieComun.TomarDa�oZ(25);
            }
            else
            {
                // Da�o a zombie inteligente
                var zombieInteligente = collision.GetComponent<ZombieInteligente>();
                if (zombieInteligente != null)
                {
                    zombieInteligente.TomarDa�oZ(25);
                }
            }

            StartCoroutine(EsperarYDevolver());
        }
    }

    private IEnumerator EsperarYDevolver()
    {
        speed = 0f; // Opcional: frena la bala tras impacto
        yield return new WaitForSeconds(1f);
        pool.DevolverBala(this);
    }
}
