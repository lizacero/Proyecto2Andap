using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }

    [Header("Paneles de UI")]
    [SerializeField] private GameObject panelGanaste1;
    [SerializeField] private GameObject panelGanaste2;
    [SerializeField] private GameObject panelPerdiste;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MostrarResultado(int actorGanador, string nombreJugador)
    {
        // Pausar el juego
        Time.timeScale = 0f;

        // Determinar qué panel mostrar según el jugador ganador
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorGanador)
        {
            // Este jugador ganó
            if (nombreJugador.Contains("Player1"))
            {
                if (panelGanaste1 != null)
                    panelGanaste1.SetActive(true);
                else
                    Debug.LogError("Panel Ganaste1 no asignado en GameManager");
            }
            else if (nombreJugador.Contains("Player2"))
            {
                if (panelGanaste2 != null)
                    panelGanaste2.SetActive(true);
                else
                    Debug.LogError("Panel Ganaste2 no asignado en GameManager");
            }
        }
        else
        {
            // Este jugador perdió
            if (panelPerdiste != null)
                panelPerdiste.SetActive(true);
            else
                Debug.LogError("Panel Perdiste no asignado en GameManager");
        }
    }

    public void OcultarTodosLosPaneles()
    {
        if (panelGanaste1 != null) panelGanaste1.SetActive(false);
        if (panelGanaste2 != null) panelGanaste2.SetActive(false);
        if (panelPerdiste != null) panelPerdiste.SetActive(false);
    }
} 