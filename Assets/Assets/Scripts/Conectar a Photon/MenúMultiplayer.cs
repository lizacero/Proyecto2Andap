using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenúMultiplayer : MonoBehaviourPunCallbacks
{
    public TMP_InputField usernameInput;

    public GameObject PanelMenú;

    public void OnclickConnect()
    {
        if (usernameInput != null)
        {
            PhotonNetwork.NickName = usernameInput.text;

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void ConnectedToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Nos conectamos a Photon");
    }

    private void OnConnectedToServer()
    {
        Debug.Log("Nos conectamos al servidor");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PanelMenú.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        
    }


    public void ConectarseSalaAleatoria()
    {
        RoomOptions opciones = new RoomOptions()
        {
            MaxPlayers = 2
        };

     
           

        if (PhotonNetwork.IsConnectedAndReady)
        {
            bool resultado = PhotonNetwork.JoinRandomOrCreateRoom(
            expectedCustomRoomProperties: null,
            expectedMaxPlayers: 0,
            matchingType: MatchmakingMode.FillRoom,
            typedLobby: TypedLobby.Default,
            sqlLobbyFilter: null,
            roomName: null,
            roomOptions: opciones,
            expectedUsers: null);

            
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
       
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Nos unimos a una sala");
        Debug.Log("Tamaño máximo de salas:" + PhotonNetwork.CurrentRoom.MaxPlayers);

        

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Error al entrar a una sala" + message);

    }

    public void CrearSala()
    {
        base.OnCreatedRoom();
        

    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("Otro jugador entró a la sala");
        
    }
}
