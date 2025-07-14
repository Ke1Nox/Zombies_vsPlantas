using Assets.scripts.TorretaEspecial;
using UnityEngine;

public class BalaTorreEspecial : MonoBehaviour
{
     private float da�o;
    
    [SerializeField] private float tiempoVida = 2f;

    void Start()
    {
        Destroy(gameObject, tiempoVida); // autodestruir si no impacta
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si impact� con un objeto que implemente IDa�oable
        IDa�oable enemigo = collision.GetComponent<IDa�oable>();
        if (enemigo != null)
        {
            if (collision.TryGetComponent<zombie>(out var z))
            {
                z.TomarDa�oZ((int)da�o);
            }
            else if (collision.TryGetComponent<ZombieInteligente>(out var zi))
            {
                zi.TomarDa�oZ((int)da�o);
            }

            Destroy(gameObject); // destruir bala al impactar
        }
    }
    public void SetDa�o(float valor)
    {
        da�o = valor;
    }

}

