using Assets.scripts.TorretaEspecial;
using UnityEngine;

public class BalaTorreEspecial : MonoBehaviour
{
     private float daño;
    
    [SerializeField] private float tiempoVida = 2f;

    void Start()
    {
        Destroy(gameObject, tiempoVida); // autodestruir si no impacta
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si impactó con un objeto que implemente IDañoable
        IDañoable enemigo = collision.GetComponent<IDañoable>();
        if (enemigo != null)
        {
            if (collision.TryGetComponent<zombie>(out var z))
            {
                z.TomarDañoZ((int)daño);
            }
            else if (collision.TryGetComponent<ZombieInteligente>(out var zi))
            {
                zi.TomarDañoZ((int)daño);
            }

            Destroy(gameObject); // destruir bala al impactar
        }
    }
    public void SetDaño(float valor)
    {
        daño = valor;
    }

}

