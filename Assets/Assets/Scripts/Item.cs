using UnityEngine;

public class Item : MonoBehaviour, Interactuable
{
    [SerializeField] private GameObject panel;
    [SerializeField] private float distanciaInteraccion = 1f;

    private Transform player;
    private bool isPlayerNear = false;
    private float distance;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        panel.SetActive(false);
    }
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.position);
        if (distance <= distanciaInteraccion)
        {
            if (!isPlayerNear)
            {
                panel.SetActive(true);
                isPlayerNear = true;
            }
        }
        else
        {
            if (isPlayerNear)
            {
                panel.SetActive(false);
                isPlayerNear = false;
            }
        }
    }
    public void Interactuar()
    {
        Destroy(this.gameObject);
    }
}
