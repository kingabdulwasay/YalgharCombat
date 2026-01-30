using UnityEngine;
using Photon.Pun;

public class PlayerNetwork : MonoBehaviour
{
   public PhotonView pview;

    void Start()
    {
        pview = GetComponent<PhotonView>();
    }

    
}
