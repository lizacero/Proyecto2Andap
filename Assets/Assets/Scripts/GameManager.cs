using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }

    [Header("Paneles de UI")]
    [SerializeField] private GameObject panelGanaste1;
    [SerializeField] private GameObject videoGanaste1;
    [SerializeField] private GameObject panelGanaste2;
    [SerializeField] private GameObject videoGanaste2;
    [SerializeField] private GameObject panelPerdiste;
    [SerializeField] private GameObject videoPerdiste;

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
                if (videoGanaste1 != null)
                    videoGanaste1.SetActive(true);
                else
                    Debug.LogError("Panel Ganaste1 no asignado en GameManager");
            }
            else if (nombreJugador.Contains("Player2"))
            {
                if (panelGanaste2 != null)
                    panelGanaste2.SetActive(true);
                if (videoGanaste2 != null)
                    videoGanaste2.SetActive(true);
                else
                    Debug.LogError("Panel Ganaste2 no asignado en GameManager");
            }
            
            // Reproducir música de victoria
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ReproducirMusicaVictoria();
            }
        }
        else
        {
            // Este jugador perdió
            if (panelPerdiste != null)
                panelPerdiste.SetActive(true);
            if (videoPerdiste != null)
                videoPerdiste.SetActive(true);
            else
                Debug.LogError("Panel Perdiste no asignado en GameManager");
            
            // Reproducir música de derrota
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ReproducirMusicaDerrota();
            }
        }
    }

    public void OcultarTodosLosPaneles()
    {
        if (panelGanaste1 != null) panelGanaste1.SetActive(false);
        if (videoGanaste1 != null) videoGanaste1.SetActive(false);
        if (panelGanaste2 != null) panelGanaste2.SetActive(false);
        if (videoGanaste2 != null) videoGanaste2.SetActive(false);
        if (panelPerdiste != null) panelPerdiste.SetActive(false);
        if (videoPerdiste != null) videoPerdiste.SetActive(false);
    }
} 