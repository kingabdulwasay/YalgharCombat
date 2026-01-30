using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Network : MonoBehaviourPunCallbacks
{
    public GameObject loading;
    public GameObject lobby;
    void Start()
    {
        loading.SetActive(true);
        lobby.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinLobby();
    } 

    public override void OnJoinedLobby(){
        loading.SetActive(false);
        lobby.SetActive(true);
    }
}
