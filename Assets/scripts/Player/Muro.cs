using UnityEngine;

public class Muro : MonoBehaviour
{
    public int vida = 100;

    public void TomarDa�o(int da�o)
    {
        vida -= da�o;
        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }
}

