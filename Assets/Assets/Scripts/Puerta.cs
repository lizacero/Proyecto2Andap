using UnityEngine;

public class Puerta : MonoBehaviour, Interactuable
{
    [SerializeField] private GameObject texto;
    [SerializeField] private float distanciaInteraccion = 1f;
    [SerializeField] private Sprite spritePuertaCerrada;
    [SerializeField] private Sprite spritePuertaAbierta;

    private Transform player;
    private bool isPlayerNear = false;
    private bool puertaAbierta = false;
    private float distance;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        texto.SetActive(false);
    }
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.position);
        if (puertaAbierta == false)
        {
            if (distance <= distanciaInteraccion)
            {
                if (!isPlayerNear)
                {
                    texto.SetActive(true);
                    isPlayerNear = true;
                }
            }
            else
            {
                if (isPlayerNear)
                {
                    texto.SetActive(false);
                    isPlayerNear = false;
                }
            }
        }
    }
    public void Interactuar()
    {
        puertaAbierta = true;
        texto.SetActive(false);
        Debug.Log("Abrir puerta");
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        int layerPuerta = LayerMask.NameToLayer("Interactuable");
        gameObject.layer = layerPuerta;
        
        // Cambiar sprite a puerta abierta
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spritePuertaAbierta != null)
        {
            spriteRenderer.sprite = spritePuertaAbierta;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer no encontrado o spritePuertaAbierta no asignado");
        }
    }
}
