
using UnityEngine;
using Photon.Pun;

public class Spawning : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        for(int i = 0; i < 3; i++){
        PhotonNetwork.Instantiate(enemyPrefab.name, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        }
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
    }
}
                                                                                                   