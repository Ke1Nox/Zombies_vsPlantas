using UnityEngine;

public class EnemigoVolador : MonoBehaviour
{
    public float velocidad = 2f;
    public int vida = 100;
    public int da�oATorre = 30;
    public int puntosPorMuerte = 5;

    private Transform objetivo;

    void Start()
    {
        objetivo = GestorGlobal.Torre?.transform;
    }

    void Update()
    {
        transform.position += Vector3.right * velocidad * Time.deltaTime;
    }

    public void RecibirDa�o(int da�o)
    {
        vida -= da�o;

        if (vida <= 0)
        {
            GestorGlobal.SumarPuntos(puntosPorMuerte);
            GestorGlobal.EnemigoDerrotado();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("torre"))
        {
            GestorGlobal.DamageTower(da�oATorre);
            GestorGlobal.EnemigoDerrotado();
            Destroy(gameObject);
        }
    }
}