using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
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
    //[SerializeField] private LayerMask interactuable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
                anim.SetBool("Mirror", true);
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
        if (colliderDelante) //si existe
        {
            if (colliderDelante.CompareTag("Puerta"))
            {
                Debug.Log("Puerta adelante");
                if (llaves >= 5)
                {
                    if (colliderDelante.TryGetComponent(out Interactuable interactuable))
                    {
                        interactuable.Interactuar();
                    }
                }
                else
                {
                    Debug.Log("Llaves insuficientes");
                }
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {            
            llaves += 1;
            Debug.Log(llaves);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Velocidad"))
        {
            StartCoroutine(AumentoVelocidad());
            Destroy(collision.gameObject);
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
}
