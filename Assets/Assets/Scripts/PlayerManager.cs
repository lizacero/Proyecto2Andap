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
    private Interactuable interactuable;
    [SerializeField] private float velocidad;
    [SerializeField] private float radioInteraccion;
    [SerializeField] private LayerMask colisionable;
    //[SerializeField] private LayerMask interactuable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            ultimoInput = new Vector3(inputH, inputV, 0);
            puntoDestino = transform.position + ultimoInput;
            puntoInteraccion = puntoDestino;
            colliderDelante = LanzarCheck();
            if (!colliderDelante)
            {
                StartCoroutine(Mover());
            }
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
                Debug.Log("llave adelante");
                if (colliderDelante.TryGetComponent(out Interactuable interactuable))
                {
                    interactuable.Interactuar();
                    llaves += 1;
                    Debug.Log(llaves);
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
            // aumentar velocidad por cierto tiempo
            AumentoVelocidad();
        }
    }

    IEnumerator AumentoVelocidad()
    {
        velocidadInicial = velocidad;
        velocidad = velocidad * 2;
        yield return new WaitForSeconds(5);
        velocidad = velocidadInicial;

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoInteraccion, radioInteraccion);
    }
}
