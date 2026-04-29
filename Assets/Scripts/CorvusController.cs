using UnityEngine;

public class CorvusController : MonoBehaviour
{
    public float speed = 4f;
    public float attackRange = 15f;
    public float fireRate = 4f;
    public GameObject projectilePrefab;
    public int cantidadMisiles = 5;
    public float spread = 25f;

    private float fireTimer;
    private Transform player;
    private SpriteRenderer sr;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        // Parar si el jugador ganó o perdió
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null && (pc.victoria || pc.gameOver)) return;

        Vector3 target = new Vector3(player.position.x,
                                     player.position.y + 3f,
                                     transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position,
                                                  target,
                                                  speed * Time.deltaTime);

        if (player.position.x < transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;

        fireTimer += Time.deltaTime;
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist < attackRange && fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0;
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null) return;

        for (int i = 0; i < cantidadMisiles; i++)
        {
            float angulo = (i - (cantidadMisiles - 1) / 2f) * spread;
            Quaternion rotacion = Quaternion.Euler(0, 0, angulo);
            Instantiate(projectilePrefab, transform.position, rotacion);
        }
    }
}