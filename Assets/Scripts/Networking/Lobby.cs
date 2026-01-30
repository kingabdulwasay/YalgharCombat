using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class Lobby : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomId;
    public GameObject loading;
    public GameObject lobby;
    public void CreateRoom()
    {
        lobby.SetActive(false);
        loading.SetActive(true);
        PhotonNetwork.CreateRoom(roomId.text);
    }

    public void JoinRoom()
    {
        lobby.SetActive(false);
        loading.SetActive(true);
        PhotonNetwork.JoinRoom(roomId.text);
    }

    public override void OnJoinedRoom(){
        
        PhotonNetwork.LoadLevel("Prototype");
    }
}
