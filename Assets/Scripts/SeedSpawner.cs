using UnityEngine;

public class SeedSpawner : MonoBehaviour
{
    public GameObject semillaSolar;
    public GameObject semillaLunar;
    public GameObject semillaTormenta;

    public float spawnRate = 0.8f;
    private float spawnTimer;

    public float spawnRangeX = 10f;
    public float spawnY = 8f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null && (pc.gameOver || pc.victoria)) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate)
        {
            spawnTimer = 0f;
            SpawnSemilla();
        }
    }

    void SpawnSemilla()
    {
        float randomX = player.position.x + Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, player.position.y + spawnY, 0f);

        int random = Random.Range(0, 10);
        GameObject semilla;

        if (random < 2)
            semilla = semillaSolar;       // 20% Solar
        else if (random < 4)
            semilla = semillaLunar;       // 20% Lunar
        else
            semilla = semillaTormenta;    // 60% Tormenta

        if (semilla != null)
            Instantiate(semilla, spawnPos, Quaternion.identity);
    }
}