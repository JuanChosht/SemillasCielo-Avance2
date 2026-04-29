using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private SpriteRenderer sr;

    public Sprite spriteIdle;
    public Sprite spriteWalk1;
    public Sprite spriteHit;
    public Sprite spriteVictoria1;

    public int vidas = 3;
    private bool golpeado = false;
    public float invencibilidad = 0.4f;
    private float invTimer = 0f;
    private float walkTimer = 0f;
    public float walkFrameRate = 0.15f;
    private int walkFrame = 0;

    public TextMeshProUGUI textVidas;
    public GameObject panelGameOver;
    public GameObject panelVictoria;

    public bool gameOver = false;
    public bool victoria = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ActualizarVidas();
    }

    void Update()
    {
        if (gameOver) return;

        // Detectar zona de victoria por posición
        if (transform.position.x >= 35.08f && !victoria && !gameOver)
            MostrarVictoria();

        if (victoria)
        {
            sr.sprite = spriteVictoria1;
            return;
        }

        float input = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(input * moveSpeed, rb.linearVelocity.y);

        if (input < 0) sr.flipX = true;
        if (input > 0) sr.flipX = false;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }

        if (golpeado)
        {
            sr.sprite = spriteHit;
            invTimer += Time.deltaTime;
            if (invTimer >= invencibilidad)
            {
                golpeado = false;
                invTimer = 0f;
            }
        }
        else if (input != 0)
        {
            walkTimer += Time.deltaTime;
            if (walkTimer >= walkFrameRate)
            {
                walkTimer = 0f;
                walkFrame = (walkFrame + 1) % 2;
                sr.sprite = walkFrame == 0 ? spriteWalk1 : spriteIdle;
            }
        }
        else
        {
            sr.sprite = spriteIdle;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    public void RecibirGolpe()
    {
        if (golpeado || gameOver || victoria) return;
        vidas--;
        golpeado = true;
        ActualizarVidas();
        if (vidas <= 0)
            MostrarGameOver();
    }

    void ActualizarVidas()
    {
        if (textVidas != null)
            textVidas.text = "Vidas: " + vidas;
    }

    void MostrarGameOver()
    {
        gameOver = true;
        rb.linearVelocity = Vector2.zero;
        if (panelGameOver != null)
            panelGameOver.SetActive(true);
    }

    void MostrarVictoria()
    {
        victoria = true;
        rb.linearVelocity = Vector2.zero;
        if (panelVictoria != null)
            panelVictoria.SetActive(true);
    }
}