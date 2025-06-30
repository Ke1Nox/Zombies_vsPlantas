using UnityEngine;

public class Muro : MonoBehaviour
{
    public int vida = 100;

    public void TomarDaño(int daño)
    {
        vida -= daño;
        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }
}

