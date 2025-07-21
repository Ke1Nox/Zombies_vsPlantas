using UnityEngine;

public class SpawnerVolador : MonoBehaviour
{
    public GameObject enemigoVoladorPrefab;
    public float intervaloSpawn = 5f;
    public Vector2 posicionSpawn = new Vector2(-50f, 6f);

    private float tiempoSiguienteSpawn;

    void Update()
    {
        if (Time.time >= tiempoSiguienteSpawn)
        {
            SpawnVolador();
            tiempoSiguienteSpawn = Time.time + intervaloSpawn;
        }
    }

    void SpawnVolador()
    {
        Instantiate(enemigoVoladorPrefab, posicionSpawn, Quaternion.identity);
    }
}