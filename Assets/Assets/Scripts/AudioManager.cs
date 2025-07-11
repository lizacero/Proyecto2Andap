using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Música")]
    [SerializeField] private AudioClip musicaMenu;
    [SerializeField] private AudioClip musicaGameplay;
    [SerializeField] private AudioClip musicaVictoria;
    [SerializeField] private AudioClip musicaDerrota;

    [Header("Configuración")]
    [SerializeField] private float volumenMusica = 0.5f;
    [SerializeField] private bool musicaActivada = true;

    private AudioSource audioSource;
    private string escenaActual;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            ConfigurarAudioSource();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Reproducir música del menú al inicio
        ReproducirMusicaMenu();
    }

    private void Update()
    {
        // Detectar cambios de escena
        string escenaNueva = SceneManager.GetActiveScene().name;
        if (escenaNueva != escenaActual)
        {
            escenaActual = escenaNueva;
            CambiarMusicaPorEscena(escenaNueva);
        }
    }

    private void ConfigurarAudioSource()
    {
        audioSource.loop = true;
        audioSource.volume = volumenMusica;
        audioSource.playOnAwake = false;
    }

    private void CambiarMusicaPorEscena(string nombreEscena)
    {
        switch (nombreEscena.ToLower())
        {
            case "menuprincipal":
            case "menu":
                ReproducirMusicaMenu();
                break;
            case "gameplay":
            case "pruebas":
                ReproducirMusicaGameplay();
                break;
            default:
                // Si no es una escena específica, mantener la música actual
                break;
        }
    }

    public void ReproducirMusicaMenu()
    {
        if (musicaActivada && musicaMenu != null)
        {
            CambiarMusica(musicaMenu);
        }
    }

    public void ReproducirMusicaGameplay()
    {
        if (musicaActivada && musicaGameplay != null)
        {
            CambiarMusica(musicaGameplay);
        }
    }

    public void ReproducirMusicaVictoria()
    {
        if (musicaActivada && musicaVictoria != null)
        {
            ReproducirMusicaUnaVez(musicaVictoria);
        }
    }

    public void ReproducirMusicaDerrota()
    {
        if (musicaActivada && musicaDerrota != null)
        {
            ReproducirMusicaUnaVez(musicaDerrota);
        }
    }

    private void CambiarMusica(AudioClip nuevaMusica)
    {
        if (audioSource.clip != nuevaMusica)
        {
            audioSource.clip = nuevaMusica;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void ReproducirMusicaUnaVez(AudioClip musica)
    {
        // Guardar la música actual para restaurarla después
        AudioClip musicaActual = audioSource.clip;
        bool loopActual = audioSource.loop;
        
        // Reproducir la música de victoria/derrota
        audioSource.clip = musica;
        audioSource.loop = false;
        audioSource.Play();
        
        // Restaurar la música anterior después de que termine
        StartCoroutine(RestaurarMusicaDespues(musica.length, musicaActual, loopActual));
    }

    private System.Collections.IEnumerator RestaurarMusicaDespues(float tiempo, AudioClip musicaAnterior, bool loopAnterior)
    {
        yield return new WaitForSeconds(tiempo);
        
        if (musicaAnterior != null)
        {
            audioSource.clip = musicaAnterior;
            audioSource.loop = loopAnterior;
            audioSource.Play();
        }
    }

    public void CambiarVolumen(float nuevoVolumen)
    {
        volumenMusica = Mathf.Clamp01(nuevoVolumen);
        audioSource.volume = volumenMusica;
    }

    public void ActivarDesactivarMusica(bool activar)
    {
        musicaActivada = activar;
        if (!activar)
        {
            audioSource.Stop();
        }
        else if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    public float GetVolumen()
    {
        return volumenMusica;
    }

    public bool IsMusicaActivada()
    {
        return musicaActivada;
    }
} 