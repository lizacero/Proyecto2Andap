using UnityEngine;
using Photon.Pun;

public class PickupObject : MonoBehaviourPun
{
    [PunRPC]
    public void DesactivarObjeto()
    {
        gameObject.SetActive(false);
    }
}
