using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] float tiempoSpawn = 2f;
    private float currentTime;

    public MyQueue <GameObject> colaDeZombies = new MyQueue<GameObject>(); // Cola de zombies

    void Start()
    {
        // Inicializamos la cola con algunos zombies (por ejemplo, 2)
        for (int i = 0; i < 4; i++)
        {
            GameObject zombie = Instantiate(zombiePrefab, transform.position, transform.rotation);
            zombie.GetComponent<zombie>().SetSpawner(this); // <-- Llamás a SetSpawner
            colaDeZombies.Enqueue(zombie); // Los agregamos a la cola
            zombie.SetActive(false); // Muy importante: desactivarlos al principio
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= tiempoSpawn)
        {
            // Si hay zombies disponibles en la cola, los reutilizamos
            if (colaDeZombies.Count() > 0)
            {
                GameObject zombie = colaDeZombies.Dequeue(); // Extraemos el primer zombie de la cola
                zombie.SetActive(true); // Activamos el zombie

                // Generamos una posición Y aleatoria entre transform.position.y - 18 y transform.position.y - 7
                float randomY = Random.value > 0.5f ? -18f : -6.6f;

                // Actualizamos la posición del zombie
                zombie.transform.position = new Vector3(transform.position.x, randomY, transform.position.z);
                zombie.transform.rotation = transform.rotation; // Lo rotamos igual que el spawner
            }

            currentTime = 0f; // Reiniciamos el temporizador
        }
    }
}
