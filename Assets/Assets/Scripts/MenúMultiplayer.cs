using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenúMultiplayer : MonoBehaviourPunCallbacks
{
    TMP_InputField usernameInput;

    TMP_Text ButtonText;

    public void OnclickConnect()
    {
        if (usernameInput != null)
        {
            PhotonNetwork.NickName = usernameInput.text;



            ButtonText.text = "Conectando...";
            PhotonNetwork.ConnectUsingSettings();
    }
}

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        SceneManager.LoadScene("Gameplay");
    }
}

