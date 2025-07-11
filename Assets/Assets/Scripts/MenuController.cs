using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuController : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private string nombreEscenaMenu = "MenuPrincipal";
    [SerializeField] private bool usarTransicion = true;
    [SerializeField] private float tiempoTransicion = 1f;

    public void VolverAlMenu()
    {
        // Reanudar el tiempo si estaba pausado
        Time.timeScale = 1f;
        
        // Desconectar de Photon si estamos en una sala
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        
        // Ocultar todos los paneles de resultado
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OcultarTodosLosPaneles();
        }
        
        // Cargar la escena del menú
        if (usarTransicion)
        {
            StartCoroutine(CargarEscenaConTransicion());
        }
        else
        {
            SceneManager.LoadScene(nombreEscenaMenu);
        }
    }

    private System.Collections.IEnumerator CargarEscenaConTransicion()
    {
        // Aquí podrías agregar una transición visual (fade out, etc.)
        // Por ahora solo esperamos un poco
        yield return new WaitForSeconds(tiempoTransicion);
        
        SceneManager.LoadScene(nombreEscenaMenu);
    }

    public void ReiniciarPartida()
    {
        // Reanudar el tiempo
        Time.timeScale = 1f;
        
        // Ocultar todos los paneles de resultado
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OcultarTodosLosPaneles();
        }
        
        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SalirDelJuego()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} 