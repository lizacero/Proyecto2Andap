using UnityEngine;
using Photon.Pun;

public class Puerta : MonoBehaviour, Interactuable
{
    [SerializeField] private GameObject texto;
    [SerializeField] private float distanciaInteraccion = 1f;


    private Transform player;
    private bool isPlayerNear = false;
    private bool puertaAbierta = false;
    private float distance;

    void Start()
    {
        texto.SetActive(false);
        // Buscar el jugador local
        BuscarJugadorLocal();
    }
    
    void Update()
    {
        // Si no tenemos referencia al jugador, intentar encontrarlo
        if (player == null)
        {
            BuscarJugadorLocal();
            return;
        }
        
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
    
    private void BuscarJugadorLocal()
    {
        // Buscar todos los jugadores en la escena
        GameObject[] jugadores = GameObject.FindGameObjectsWithTag("Player");
        
        
        // Buscar el jugador local (el que tiene PhotonView.IsMine = true)
        foreach (GameObject jugador in jugadores)
        {
            PhotonView pv = jugador.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                player = jugador.transform;
                return;
            }
        }
        
        // Si no encontramos el jugador local, usar el primer jugador encontrado
        if (jugadores.Length > 0)
        {
            player = jugadores[0].transform;
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
        

    }
}
