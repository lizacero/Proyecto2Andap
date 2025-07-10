using Photon.Pun;
using UnityEngine;

public class GameSpawner : MonoBehaviour
{
    public GameObject player1Prefab;
    public GameObject player2Prefab;

    // Puedes ajustar estas posiciones como prefieras
    public Vector3 spawnPosPlayer1 = new Vector3(-1.5f, 0.5f, 0);
    public Vector3 spawnPosPlayer2 = new Vector3(1.5f, 0.5f, 0);

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(player1Prefab.name, spawnPosPlayer1, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(player2Prefab.name, spawnPosPlayer2, Quaternion.identity);
        }
    }
}
