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
    public Sprite spritePowerUp;

    public int vidas = 3;
    private bool golpeado = false;
    public float invencibilidad = 0.4f;
    private float invTimer = 0f;
    private float walkTimer = 0f;
    public float walkFrameRate = 0.15f;
    private int walkFrame = 0;
    private bool enPowerUp = false;

    public TextMeshProUGUI textVidas;
    public GameObject panelGameOver;
    public GameObject panelVictoria;

    public bool gameOver = false;
    public bool victoria = false;

    [Header("Sonidos")]
    public AudioClip sonidoGolpe;
    public AudioClip sonidoVictoria;
    public AudioClip sonidoGameOver;
    public AudioClip sonidoPowerUp;
    public AudioClip sonidoTormenta;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        ActualizarVidas();
    }

    void Update()
    {
        if (gameOver) return;

        if (transform.position.y < -8f)
            MostrarGameOver();

        if (transform.position.x >= 48.66f && !victoria && !gameOver)
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
        else if (enPowerUp)
        {
            sr.sprite = spritePowerUp;
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
        CameraFollow.instance.Shake();
        if (sonidoGolpe != null) audioSource.PlayOneShot(sonidoGolpe);
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
        AudioSource musicaFondo = Camera.main.GetComponent<AudioSource>();
        if (musicaFondo != null) musicaFondo.Stop();
        if (sonidoGameOver != null) audioSource.PlayOneShot(sonidoGameOver);
        if (panelGameOver != null)
            panelGameOver.SetActive(true);
    }

    void MostrarVictoria()
    {
        victoria = true;
        rb.linearVelocity = Vector2.zero;
        AudioSource musicaFondo = Camera.main.GetComponent<AudioSource>();
        if (musicaFondo != null) musicaFondo.Stop();
        if (sonidoVictoria != null) audioSource.PlayOneShot(sonidoVictoria);
        if (panelVictoria != null)
            panelVictoria.SetActive(true);
    }

    public void AplicarVelocidad(float duracion)
    {
        if (sonidoPowerUp != null) audioSource.PlayOneShot(sonidoPowerUp);
        StartCoroutine(PowerUpVelocidad(duracion));
    }

    public void AplicarCongelamiento(float duracion)
    {
        if (sonidoPowerUp != null) audioSource.PlayOneShot(sonidoPowerUp);
        StartCoroutine(CongelarCorvus(duracion));
    }

    public void AplicarSonidoTormenta()
    {
        if (sonidoTormenta != null) audioSource.PlayOneShot(sonidoTormenta);
        StartCoroutine(MostrarSpriteTormenta());
    }

    private System.Collections.IEnumerator MostrarSpriteTormenta()
    {
        enPowerUp = true;
        yield return new WaitForSeconds(0.5f);
        enPowerUp = false;
    }

    private System.Collections.IEnumerator PowerUpVelocidad(float duracion)
    {
        enPowerUp = true;
        moveSpeed *= 1.4f;
        yield return new WaitForSeconds(0.5f);
        enPowerUp = false;
        yield return new WaitForSeconds(duracion - 0.5f);
        moveSpeed /= 1.4f;
    }

    private System.Collections.IEnumerator CongelarCorvus(float duracion)
    {
        enPowerUp = true;
        CorvusController[] cuervos = FindObjectsByType<CorvusController>(FindObjectsSortMode.None);
        foreach (CorvusController c in cuervos)
            c.enabled = false;

        yield return new WaitForSeconds(0.5f);
        enPowerUp = false;

        yield return new WaitForSeconds(duracion - 0.5f);
        foreach (CorvusController c in cuervos)
            c.enabled = true;
    }
}