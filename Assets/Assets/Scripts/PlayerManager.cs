using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable 
{
    private float inputH;
    private float inputV;
    private float velocidadInicial;
    private bool moviendo;
    private int llaves=0;
    private Vector3 puntoDestino;
    private Vector3 puntoInteraccion;
    private Vector3 ultimoInput;
    private Collider2D colliderDelante;
    private Animator anim;
    [SerializeField] private float velocidad;
    [SerializeField] private float radioInteraccion;
    [SerializeField] private LayerMask colisionable;
    // Variables para Photon
    public new PhotonView photonView;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool esJugadorLocal = false;
    //[SerializeField] private LayerMask interactuable;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Obtener componentes
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (photonView == null)
        {
            photonView = GetComponent<PhotonView>();
        }

        // Configurar según si es el jugador local o remoto
        if (photonView.IsMine)
        {
            ConfigurarJugadorLocal();
        }
        else
        {
            ConfigurarJugadorRemoto();
        }
    }

    private void ConfigurarJugadorLocal()
    {
        esJugadorLocal = true;
        Debug.Log("Configurando jugador local: " + PhotonNetwork.NickName);
        
        // Configurar color para distinguir al jugador local
        if (spriteRenderer != null)
        {
            //spriteRenderer.color = Color.green;
        }
        
        // Configurar la cámara para seguir al jugador local
        if (Camera.main != null)
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 0, -10);
        }
        Camera cam = GetComponentInChildren<Camera>(true); // true para incluir inactivos

        if (cam != null)
            cam.gameObject.SetActive(true); // Solo la cámara del jugador local está activa
    }

    private void ConfigurarJugadorRemoto()
    {
        esJugadorLocal = false;
        Debug.Log("Configurando jugador remoto");
        
        // Configurar color para distinguir al jugador remoto
        if (spriteRenderer != null)
        {
            //spriteRenderer.color = Color.red;
        }
        
        // Deshabilitar el control para jugadores remotos
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        Camera cam = GetComponentInChildren<Camera>(true);
        if (cam != null)
            cam.gameObject.SetActive(false); // Desactiva la cámara de los jugadores remotos
    }


    // Update is called once per frame
    void Update()
    {
        // Solo procesar input si somos el dueño del objeto
        if (!photonView.IsMine) return;
        if (inputV == 0)
        {
            inputH = Input.GetAxisRaw("Horizontal");
        }
        if (inputH == 0) 
        { 
            inputV = Input.GetAxisRaw("Vertical");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            LanzarInteraccion();
        }

        if (!moviendo && (inputH != 0 || inputV != 0))
        {
            anim.SetBool("andando", true);
            anim.SetFloat("inputH", inputH);
            anim.SetFloat("inputV", inputV);
            if(inputH == -1)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            ultimoInput = new Vector3(inputH, inputV, 0);
            puntoDestino = transform.position + ultimoInput;
            puntoInteraccion = puntoDestino;
            colliderDelante = LanzarCheck();
            if (!colliderDelante)
            {
                StartCoroutine(Mover());
            }
        }
        else if (inputH == 0 && inputV == 0)
        {
            anim.SetBool("andando", false);
        }
    }

    IEnumerator Mover()
    {
        moviendo = true;
        while (transform.position != puntoDestino)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino, velocidad * Time.deltaTime);
            yield return null;
        }
        puntoInteraccion = transform.position + ultimoInput;
        moviendo = false;
    }
    private Collider2D LanzarCheck()
    {
        return Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion, colisionable);
    }

    private void LanzarInteraccion()
    {
        colliderDelante = LanzarCheck();
        if (colliderDelante && colliderDelante.CompareTag("Puerta"))
        {
            if (llaves >= 5)
            {
                if (colliderDelante.TryGetComponent(out Interactuable interactuable))
                {
                    interactuable.Interactuar();
                }
                // El jugador local abre la puerta y notifica a todos
                photonView.RPC("RPC_ResultadoPartida", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
            }
            else
            {
                Debug.Log("Llaves insuficientes");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Solo procesar colisiones si somos el dueño del objeto
        if (!photonView.IsMine) return;
        if (collision.gameObject.CompareTag("Key"))
        {            
            // Verificar si ya tienes 5 llaves
            if (llaves < 5)
            {
                llaves += 1;
                Debug.Log("Llaves: " + llaves + "/5");
                //Destroy(collision.gameObject);
                collision.gameObject.GetComponent<PhotonView>().RPC("DesactivarObjeto", RpcTarget.All);
            }
            else
            {
                Debug.Log("Ya tienes el máximo de llaves (5)");
            }
        }
        if (collision.gameObject.CompareTag("Velocidad"))
        {
            StartCoroutine(AumentoVelocidad());
            //Destroy(collision.gameObject);
            collision.gameObject.GetComponent<PhotonView>().RPC("DesactivarObjeto", RpcTarget.All);
        }
        if (collision.gameObject.CompareTag("Puerta"))
        {
            //Activa Canvas ganaste
            //Pausa game
            Debug.Log("Ganaste!!");
        }
    }

    IEnumerator AumentoVelocidad()
    {
        velocidadInicial = velocidad;
        velocidad = velocidad * 3;
        Debug.Log("Velocidad: " + velocidad);
        yield return new WaitForSeconds(5);
        velocidad = velocidadInicial;
        Debug.Log("Velocidad: " + velocidad);

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoInteraccion, radioInteraccion);
    }

    // Sincronización de Photon usando IPunObservable
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Enviar datos
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(llaves);
            stream.SendNext(velocidad);
        }
        else
        {
            // Recibir datos
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            llaves = (int)stream.ReceiveNext();
            velocidad = (float)stream.ReceiveNext();
        }
    }

    // Métodos RPC para sincronización de eventos importantes
    [PunRPC]
    public void RecogerLlave()
    {
        if (llaves < 5)
        {
            llaves += 1;
            Debug.Log("Llaves recogidas: " + llaves + "/5");
        }
    }

    [PunRPC]
    public void ActivarPowerUpVelocidad()
    {
        StartCoroutine(AumentoVelocidad());
    }

    [PunRPC]
    public void RPC_ResultadoPartida(int actorGanador)
    {
        // Usar el GameManager para mostrar el resultado
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MostrarResultado(actorGanador, gameObject.name);
        }
        else
        {
            Debug.LogError("GameManager no encontrado. Asegúrate de que existe en la escena.");
        }
    }

    // Métodos para obtener información del jugador
    public int GetLlaves()
    {
        return llaves;
    }

    public bool EsJugadorLocal()
    {
        return esJugadorLocal;
    }

    public string GetNombreJugador()
    {
        return PhotonNetwork.NickName;
    }

    public bool TieneMaximoLlaves()
    {
        return llaves >= 5;
    }
}
