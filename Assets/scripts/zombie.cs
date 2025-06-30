using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombie : MonoBehaviour
{
    [SerializeField] int vidaZombie;
    [SerializeField] int maximoVidaZ = 100;
    public float vel = 5f;
    Rigidbody2D rb2D;
    [SerializeField] private float TiempoEntreDaño = 2f;
    private float TiempoSigienteDaño=5;



    private Spawner spawner;

    public Puntaje puntaje;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        vidaZombie = maximoVidaZ;

        spawner = FindObjectOfType<Spawner>();
    }

    //movimiento zombie
    private void FixedUpdate()
    {
        rb2D.MovePosition(new Vector2(transform.position.x, transform.position.y) + Vector2.right * vel * Time.fixedDeltaTime);
    }

   
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("torre"))
        {
            TiempoSigienteDaño -= Time.deltaTime;

            GameManager.Instance.DamageTower(1);
        }
    }


    public void TomarDañoZ(int daño)
    {
        vidaZombie -= daño;
        if (vidaZombie <= 0) ZombieMuere();
    }
    public void SetSpawner(Spawner spawnerReference)
    {
        spawner = spawnerReference;
    }
    private void ZombieMuere()
    {
        gameObject.SetActive(false); // Desactivar el zombie en lugar de destruirlo
        spawner.GetComponent<Spawner>().colaDeZombies.Enqueue(gameObject); // Reincorporar al pool de zombies
        vidaZombie = 100;
        GameManager.Instance.SumarPuntos(50);
    }
}
