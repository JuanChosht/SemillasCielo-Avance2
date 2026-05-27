using UnityEngine;

public class Seed : MonoBehaviour
{
    public enum TipoSemilla { Solar, Lunar, Tormenta }
    public TipoSemilla tipo;
    public float duracionPowerUp = 2f;
    public GameObject corvusAgresivoPrefab;

    void Start()
    {
        Destroy(gameObject, 6f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerController pc = col.GetComponent<PlayerController>();
            if (pc != null)
            {
                if (tipo == TipoSemilla.Solar)
                    pc.AplicarVelocidad(duracionPowerUp);
                else if (tipo == TipoSemilla.Lunar)
                    pc.AplicarCongelamiento(duracionPowerUp);
                else if (tipo == TipoSemilla.Tormenta)
                {
                    pc.AplicarSonidoTormenta();
                    InvocarCorvusAgresivo();
                }
            }
            Destroy(gameObject);
        }
    }

    void InvocarCorvusAgresivo()
    {
        if (corvusAgresivoPrefab != null)
        {
            Vector3 spawnPos = new Vector3(
                transform.position.x + 3f,
                transform.position.y + 5f,
                0f
            );
            Instantiate(corvusAgresivoPrefab, spawnPos, Quaternion.identity);
        }
    }
}